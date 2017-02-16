//----------------------------------------------
//            MeshBaker
// Copyright © 2011-2012 Ian Deane
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Text;
using DigitalOpus.MB.Core;

/// <summary>
/// Root class of all the baking Components
/// </summary>
public abstract class MB3_MeshBakerRoot : MonoBehaviour {
	[HideInInspector] public abstract MB2_TextureBakeResults textureBakeResults{
		get;
		set;
	}
	
	//todo switch this to List<Renderer>
	public virtual List<GameObject> GetObjectsToCombine(){
		return null;	
	}
	
	public static bool DoCombinedValidate(MB3_MeshBakerRoot mom, MB_ObjsToCombineTypes objToCombineType, MB2_EditorMethodsInterface editorMethods, MB2_ValidationLevel validationLevel){
		if (mom.textureBakeResults == null){
			Debug.LogError("Need to set Material Bake Result on " + mom);
			return false;
		}
		if (mom is MB3_MeshBakerCommon){
			MB3_MeshBakerCommon momMB = (MB3_MeshBakerCommon) mom;
			MB3_TextureBaker tb = momMB.GetTextureBaker();
			if (tb != null && tb.textureBakeResults != mom.textureBakeResults){
				Debug.LogWarning("Material Bake Result on this component is not the same as the Material Bake Result on the MB3_TextureBaker.");
			}
		}

		Dictionary<int,MB_Utility.MeshAnalysisResult> meshAnalysisResultCache = null;
		if (validationLevel == MB2_ValidationLevel.robust){
			meshAnalysisResultCache = new Dictionary<int, MB_Utility.MeshAnalysisResult>();
		}
		List<GameObject> objsToMesh = mom.GetObjectsToCombine();
		for (int i = 0; i < objsToMesh.Count; i++){
			GameObject go = objsToMesh[i];
			if (go == null){
				Debug.LogError("The list of objects to combine contains a null at position." + i + " Select and use [shift] delete to remove");
				return false;					
			}
			for (int j = i + 1; j < objsToMesh.Count; j++){
				if (objsToMesh[i] == objsToMesh[j]){
					Debug.LogError("The list of objects to combine contains duplicates at " + i + " and " + j);
					return false;	
				}
			}
			if (MB_Utility.GetGOMaterials(go) == null){
				Debug.LogError("Object " + go + " in the list of objects to be combined does not have a material");
				return false;
			}
			Mesh m = MB_Utility.GetMesh(go);
			if (m == null){
				Debug.LogError("Object " + go + " in the list of objects to be combined does not have a mesh");
				return false;
			}
			if (m != null){ //This check can be very expensive and it only warns so only do this if we are in the editor.
				if (!Application.isEditor && 
				    Application.isPlaying &&
					mom.textureBakeResults.doMultiMaterial && 
					validationLevel >= MB2_ValidationLevel.robust){
					MB_Utility.MeshAnalysisResult mar;
					if (!meshAnalysisResultCache.TryGetValue(m.GetInstanceID(),out mar)){
						MB_Utility.doSubmeshesShareVertsOrTris(m,ref mar);
						meshAnalysisResultCache.Add (m.GetInstanceID(),mar);
					}
					if (mar.hasOverlappingSubmeshVerts){
						Debug.LogWarning("Object " + objsToMesh[i] + " in the list of objects to combine has overlapping submeshes (submeshes share vertices). If the UVs associated with the shared vertices are important then this bake may not work. If you are using multiple materials then this object can only be combined with objects that use the exact same set of textures (each atlas contains one texture). There may be other undesirable side affects as well. Mesh Master, available in the asset store can fix overlapping submeshes.");	
					}
				}
			}
		}

		
		List<GameObject> objs = objsToMesh;
		
		if (mom is MB3_MeshBaker)
		{
			objs = mom.GetObjectsToCombine();
			//if (((MB3_MeshBaker)mom).useObjsToMeshFromTexBaker && tb != null) objs = tb.GetObjectsToCombine(); 
			if (objs == null || objs.Count == 0)
			{
				Debug.LogError("No meshes to combine. Please assign some meshes to combine.");
				return false;
			}
			if (mom is MB3_MeshBaker && ((MB3_MeshBaker)mom).meshCombiner.renderType == MB_RenderType.skinnedMeshRenderer){
				if (!editorMethods.ValidateSkinnedMeshes(objs))
				{
					return false;
				}
			}
		}
		
		if (editorMethods != null){
			editorMethods.CheckPrefabTypes(objToCombineType, objsToMesh);
		}
		return true;
	}

    bool onlyStaticObjects = false;
    bool onlyEnabledObjects = false;
    bool excludeMeshesWithOBuvs = true;
    bool excludeMeshesAlreadyAddedToBakers = false;
    int lightmapIndex = -2;
    Material shaderMat = null;
    Material mat = null;
    List<GameObject> GetFilteredList()
    {
        List<GameObject> newMomObjs = new List<GameObject>();
        MB3_MeshBakerRoot mom = (MB3_MeshBakerRoot)this;
        if (mom == null)
        {
            Debug.LogError("Must select a target MeshBaker to add objects to");
            return newMomObjs;
        }
        GameObject dontAddMe = null;
        Renderer r = MB_Utility.GetRenderer(mom.gameObject);
        if (r != null)
        { //make sure that this MeshBaker object is not in list
            dontAddMe = r.gameObject;
        }

        MB3_MeshBakerRoot[] allBakers = FindObjectsOfType<MB3_MeshBakerRoot>();
        HashSet<GameObject> objectsAlreadyIncludedInBakers = new HashSet<GameObject>();
        for (int i = 0; i < allBakers.Length; i++)
        {
            List<GameObject> objsToCombine = allBakers[i].GetObjectsToCombine();
            for (int j = 0; j < objsToCombine.Count; j++)
            {
                if (objsToCombine[j] != null) objectsAlreadyIncludedInBakers.Add(objsToCombine[j]);
            }
        }

        int numInSelection = 0;
        int numStaticExcluded = 0;
        int numEnabledExcluded = 0;
        int numLightmapExcluded = 0;
        int numOBuvExcluded = 0;
        int numMatExcluded = 0;
        int numShaderExcluded = 0;
        int numAlreadyIncludedExcluded = 0;

        //GameObject[] gos =Selection.gameObjects;
        GameObject[] gos = new GameObject[] { this.gameObject };
        if (gos.Length == 0)
        {
            Debug.LogWarning("No objects selected in hierarchy view. Nothing added. Try selecting some objects.");
        }
        Dictionary<int, MB_Utility.MeshAnalysisResult> meshAnalysisResultsCache = new Dictionary<int, MB_Utility.MeshAnalysisResult>(); //cache results
        for (int i = 0; i < gos.Length; i++)
        {
            GameObject go = gos[i];
            Renderer[] mrs = go.GetComponentsInChildren<Renderer>();
            for (int j = 0; j < mrs.Length; j++)
            {
                if (mrs[j] is MeshRenderer || mrs[j] is SkinnedMeshRenderer)
                {
                    if (mrs[j].GetComponent<TextMesh>() != null)
                    {
                        continue; //don't add TextMeshes
                    }
                    numInSelection++;
                    if (!newMomObjs.Contains(mrs[j].gameObject))
                    {
                        bool addMe = true;
                        if (!mrs[j].gameObject.isStatic && onlyStaticObjects)
                        {
                            numStaticExcluded++;
                            addMe = false;
                            continue;
                        }

                        if (!mrs[j].enabled && onlyEnabledObjects)
                        {
                            numEnabledExcluded++;
                            addMe = false;
                            continue;
                        }

                        if (lightmapIndex != -2)
                        {
                            if (mrs[j].lightmapIndex != lightmapIndex)
                            {
                                numLightmapExcluded++;
                                addMe = false;
                                continue;
                            }
                        }

                        if (excludeMeshesAlreadyAddedToBakers && objectsAlreadyIncludedInBakers.Contains(mrs[j].gameObject))
                        {
                            numAlreadyIncludedExcluded++;
                            addMe = false;
                            continue;
                        }

                        Mesh mm = MB_Utility.GetMesh(mrs[j].gameObject);
                        if (mm != null)
                        {
                            MB_Utility.MeshAnalysisResult mar;
                            if (!meshAnalysisResultsCache.TryGetValue(mm.GetInstanceID(), out mar))
                            {
                                MB_Utility.hasOutOfBoundsUVs(mm, ref mar);
                                meshAnalysisResultsCache.Add(mm.GetInstanceID(), mar);
                            }
                            if (mar.hasOutOfBoundsUVs && excludeMeshesWithOBuvs)
                            {
                                numOBuvExcluded++;
                                addMe = false;
                                continue;
                            }
                        }

                        if (shaderMat != null)
                        {
                            Material[] nMats = mrs[j].sharedMaterials;
                            bool usesShader = false;
                            foreach (Material nMat in nMats)
                            {
                                if (nMat != null && nMat.shader == shaderMat.shader)
                                {
                                    usesShader = true;
                                }
                            }
                            if (!usesShader)
                            {
                                numShaderExcluded++;
                                addMe = false;
                                continue;
                            }
                        }

                        if (mat != null)
                        {
                            Material[] nMats = mrs[j].sharedMaterials;
                            bool usesMat = false;
                            foreach (Material nMat in nMats)
                            {
                                if (nMat == mat)
                                {
                                    usesMat = true;
                                }
                            }
                            if (!usesMat)
                            {
                                numMatExcluded++;
                                addMe = false;
                                continue;
                            }
                        }

                        if (addMe && mrs[j].gameObject != dontAddMe)
                        {
                            if (!newMomObjs.Contains(mrs[j].gameObject))
                            {
                                //for( int k = 0; k < newMomObjs.Count; k++)
                                //{
                                //    if (newMomObjs[k].name == mrs[j].name && newMomObjs[k].GetComponent<Renderer>().sharedMaterial != mrs[j].sharedMaterial)
                                //        newMomObjs[k].name = newMomObjs[k].name + "_"+ mrs[j].sharedMaterial.name;
                                //}
                                newMomObjs.Add(mrs[j].gameObject);
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Total objects in selection " + numInSelection);
        if (numStaticExcluded > 0) Debug.Log(numStaticExcluded + " objects were excluded because they were not static");
        if (numEnabledExcluded > 0) Debug.Log(numEnabledExcluded + " objects were excluded because they were disabled");
        if (numOBuvExcluded > 0) Debug.Log(numOBuvExcluded + " objects were excluded because they had out of bounds uvs");
        if (numLightmapExcluded > 0) Debug.Log(numLightmapExcluded + " objects did not match lightmap filter.");
        if (numShaderExcluded > 0) Debug.Log(numShaderExcluded + " objects were excluded because they did not use the selected shader.");
        if (numMatExcluded > 0) Debug.Log(numMatExcluded + " objects were excluded because they did not use the selected material.");
        if (numAlreadyIncludedExcluded > 0) Debug.Log(numAlreadyIncludedExcluded + " objects were excluded because they did were already included in other bakers.");

        return newMomObjs;
    }
    public void addSelectedObjects()
    {
        MB3_MeshBakerRoot mom = (MB3_MeshBakerRoot)this;
        if (mom == null)
        {
            Debug.LogError("Must select a target MeshBaker to add objects to");
            return;
        }
        List<GameObject> newMomObjs = GetFilteredList();


        List<GameObject> momObjs = mom.GetObjectsToCombine();
        int numAdded = 0;
        for (int i = 0; i < newMomObjs.Count; i++)
        {
            if (!momObjs.Contains(newMomObjs[i]))
            {
                momObjs.Add(newMomObjs[i]);
                numAdded++;
            }
        }

        if (numAdded == 0)
        {
            Debug.LogWarning("Added 0 objects. Make sure some or all objects are selected in the hierarchy view. Also check ths 'Only Static Objects', 'Using Material' and 'Using Shader' settings");
        }
        else {
            Debug.Log("Added " + numAdded + " objects to " + mom.name);
        }
    }
}

