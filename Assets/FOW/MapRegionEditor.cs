using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapRegion))]
public class MapRegionEditor : Editor
{
    bool m_bExpand = false;
    private GUIStyle m_SceneStyle = new GUIStyle();
    //-----------------------------------------------------
    public class sTriIndexData
    {
        public int verId1;
        public int verId2;

        public long combineId;

        public sTriIndexData(int id1, int id2)
        {
            if (id1 < id2)
                combineId = id1 * 1000000 + id2;
            else
                combineId = id2 * 1000000 + id1;

            verId1 = id1;
            verId2 = id2;
        }
    }
    //-----------------------------------------------------
    static public int ColorToInt(Color c)
    {
        int retVal = 0;
        retVal |= Mathf.RoundToInt(c.r * 255f) << 24;
        retVal |= Mathf.RoundToInt(c.g * 255f) << 16;
        retVal |= Mathf.RoundToInt(c.b * 255f) << 8;
        retVal |= Mathf.RoundToInt(c.a * 255f);
        return retVal;
    }
    //-----------------------------------------------------
    private void UpdateRegion()
    {
        MapRegion mapRegion = (MapRegion)target;
        MeshFilter filter = mapRegion.gameObject.GetComponent<MeshFilter>();
        if (filter.sharedMesh == null) return;

        Mesh mesh = filter.sharedMesh;
        //  MeshUtility.SetMeshCompression(mesh, ModelImporterMeshCompression.Low);

        List<int> tris = new List<int>(mesh.triangles);
        Dictionary<Vector3, int> vMapPoints1 = new Dictionary<Vector3, int>();

        for (int i = 0; i < mesh.triangles.Length; ++i)
        {
            if (vMapPoints1.ContainsKey(mesh.vertices[mesh.triangles[i]]))
            {
                tris[i] = vMapPoints1[mesh.vertices[mesh.triangles[i]]];
            }
            else
                vMapPoints1.Add(mesh.vertices[mesh.triangles[i]], mesh.triangles[i]);
        }
        // mesh.triangles = tris.ToArray();

        List<sTriIndexData> vTemp = new List<sTriIndexData>();
        Dictionary<long, sTriIndexData> vSet = new Dictionary<long, sTriIndexData>();
        for (int i = 0; i < tris.Count; i+=3)
        {
            int index0 = tris[i+0];
            int index1 = tris[i+1];
            int index2 = tris[i+2];


            sTriIndexData data1 = new sTriIndexData(index0, index1);
            sTriIndexData data2 = new sTriIndexData(index0, index2);
            sTriIndexData data3 = new sTriIndexData(index1, index2);

            if (vSet.ContainsKey(data1.combineId))
                vSet.Remove(data1.combineId);
            else
                vSet.Add(data1.combineId, data1);

            if (vSet.ContainsKey(data2.combineId))
                vSet.Remove(data2.combineId);
            else
                vSet.Add(data2.combineId, data2);

            if (vSet.ContainsKey(data3.combineId))
                vSet.Remove(data3.combineId);
            else
                vSet.Add(data3.combineId, data3);
        }

        List<int> PloygonList = new List<int>();
        foreach (var db in vSet)
        {
            vTemp.Add(db.Value);
        }
        if(vTemp.Count > 0)
        {
            if (!PloygonList.Contains(vTemp[0].verId2))
                PloygonList.Add(vTemp[0].verId2);

            int curIndex = 0;
            int findNext = vTemp[curIndex].verId1;
            while (curIndex < vTemp.Count)
            {
                bool bFind = false;
                for(int i = 0; i < vTemp.Count; ++i)
                {
                    if(curIndex != i && (vTemp[i].verId1 == findNext || vTemp[i].verId2 == findNext))
                    {
                        PloygonList.Add(findNext);
                        if (vTemp[i].verId1 == findNext)
                            findNext = vTemp[i].verId2;
                        else
                            findNext = vTemp[i].verId1;

                        curIndex = i;

                        bFind = true;
                        break;
                    }
                }

                if (!bFind || PloygonList.Count >= vTemp.Count)
                    break;
            }
        }

        mapRegion.region = new Vector3[PloygonList.Count];
        mapRegion.center = mesh.bounds.center;
        for(int i = 0; i < PloygonList.Count; ++i)
        {
            Vector3 vPos1 = mesh.vertices[PloygonList[i]];
            
            vPos1.x *= mapRegion.transform.localScale.x;
            vPos1.y *= mapRegion.transform.localScale.y;
            vPos1.z *= mapRegion.transform.localScale.z;
            mapRegion.region[i] = vPos1;
        }

        mapRegion.center.x *= mapRegion.transform.localScale.x;
        mapRegion.center.y *= mapRegion.transform.localScale.y;
        mapRegion.center.z *= mapRegion.transform.localScale.z;
    }
    //-----------------------------------------------------
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        MapRegion mapRegion = (MapRegion)target;

        mapRegion.range = EditorGUILayout.Vector2Field("range", mapRegion.range);
        mapRegion.lineOfSightCheck = (FogSystem.LOSChecks)EditorGUILayout.EnumPopup("line of sight check", mapRegion.lineOfSightCheck);
        mapRegion.isActive = EditorGUILayout.Toggle("active", mapRegion.isActive);

        mapRegion.mapColor = EditorGUILayout.ColorField("map color", mapRegion.mapColor);

        List<Vector3> vList;
        if (mapRegion.region != null)
            vList = new List<Vector3>(mapRegion.region);
        else
            vList = new List<Vector3>();

        m_bExpand = EditorGUILayout.Foldout(m_bExpand, "region");
        if(m_bExpand)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();

            int size = EditorGUILayout.IntField("size", vList.Count);
            if(size != vList.Count)
            {
                for (int i = vList.Count; i < size; ++i)
                    vList.Add(Vector3.zero);
                for (int i = size; i < vList.Count; ++i)
                    vList.RemoveAt(i);
            }

            for(int i = 0; i < vList.Count; ++i)
            {
                EditorGUILayout.LabelField("element " + i.ToString());
                Vector3 vpos = EditorGUILayout.Vector3Field("",vList[i]);
                vList[i] = vpos;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        mapRegion.region = vList.ToArray();

        serializedObject.ApplyModifiedProperties();
    }
    //----------------------------------------------
    private void OnSceneGUI()
    {
        UpdateRegion();
        MapRegion mapRegion = target as MapRegion;

        Handles.color = Color.blue;
        if (mapRegion.region == null || mapRegion.region.Length < 2) return;

        Vector3 vCenter = mapRegion.center;
        //vCenter.x *= mapRegion.transform.localScale.x;
        //vCenter.y *= mapRegion.transform.localScale.y;
        //vCenter.z *= mapRegion.transform.localScale.z;
        vCenter += mapRegion.transform.position;

        Handles.PositionHandle(vCenter, Quaternion.identity);
        for (int i = 0; i < mapRegion.region.Length; i++)
        {
            Vector3 vPos = mapRegion.region[i];
            //vPos.x *= mapRegion.transform.localScale.x;
            //vPos.y *= mapRegion.transform.localScale.y;
            //vPos.z *= mapRegion.transform.localScale.z;
            vPos += mapRegion.transform.position;
            if (i < mapRegion.region.Length && i + 1 < mapRegion.region.Length)
            {
                Vector3 vPos1 = mapRegion.region[i+1];
                //vPos1.x *= mapRegion.transform.localScale.x;
                //vPos1.y *= mapRegion.transform.localScale.y;
                //vPos1.z *= mapRegion.transform.localScale.z;
                vPos1 += mapRegion.transform.position;
                Handles.DrawLine(vPos, vPos1);
            }
        }

        Vector3 vTempPos = mapRegion.region[0];
        //vTempPos.x *= mapRegion.transform.localScale.x;
        //vTempPos.y *= mapRegion.transform.localScale.y;
        //vTempPos.z *= mapRegion.transform.localScale.z;
        vTempPos += mapRegion.transform.position;

        Vector3 vTempPos1 = mapRegion.region[mapRegion.region.Length-1];
        //vTempPos1.x *= mapRegion.transform.localScale.x;
        //vTempPos1.y *= mapRegion.transform.localScale.y;
        //vTempPos1.z *= mapRegion.transform.localScale.z;
        vTempPos1 += mapRegion.transform.position;
        Handles.DrawLine(vTempPos, vTempPos1);
    }
    //----------------------------------------------
    private void OnEnable()
    {
        m_SceneStyle.fontStyle = FontStyle.Normal;
        m_SceneStyle.fontSize = 15;
    }
    //----------------------------------------------
    private void OnDisable()
    {
        
    }
}