using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LookLightMapData 
{
    [MenuItem("Batch/" + "测试Lightmapping信息 ")]
    static void TestLightmapingInfo()
    {
        GameObject[] tempObject;
        if (Selection.activeGameObject)
        {
            tempObject = Selection.gameObjects;
            for (int i = 0; i < tempObject.Length; i++)
            {
                Debug.Log("Object name: " + tempObject[i].name);
                Debug.Log("Lightmaping Index: " + tempObject[i].GetComponent<Renderer>().lightmapIndex);
                Debug.Log("Lightmaping Offset: " + tempObject[i].GetComponent<Renderer>().lightmapScaleOffset);
            }
        }


        copy(GameObject.Find("Cube1").transform, GameObject.Find("Cube1Clone").transform);
    }


    static void Test()
    {
        /*
        LightmapData[] lightmapData = LightmapSettings.lightmaps;
        for (int i = 0; i < lightmapData.Length; i++)
        {
            LightmapData lightmap = new LightmapData();
            string path = string.Format("Lightmap/LightmapFar-{0}", i);
            lightmap.lightmapFar = Resources.Load<Texture2D>(path);
            lightmapData[i] = lightmap;
        }
        LightmapSettings.lightmaps = lightmapData;  
         */
    
    }




    //拷贝LightMap信息
    static void copy(Transform frome, Transform to)
    {
        if (frome && to)
        {
            if (frome.childCount == to.childCount)
            {
                Renderer f = frome.GetComponent<MeshRenderer>();
                Renderer t = to.GetComponent<MeshRenderer>();
                if (f && t)
                {
                    t.lightmapIndex = f.lightmapIndex;
                    t.lightmapScaleOffset = f.lightmapScaleOffset;
                }
                for (int i = 0; i < frome.childCount; i++)
                {
                    if (frome.childCount == to.childCount)
                    {
                        Transform cf = frome.GetChild(i);
                        Transform ct = to.GetChild(i);
                        if (frome.childCount == to.childCount)
                        {
                            f = cf.GetComponent<MeshRenderer>();
                            t = ct.GetComponent<MeshRenderer>();
                            if (f && t)
                            {
                                t.lightmapIndex = f.lightmapIndex;
                                t.lightmapScaleOffset = f.lightmapScaleOffset;
                            }
                            copy(cf, ct);
                        }
                    }
                }
            }
        }
    }


}
