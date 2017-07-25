using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LightMapMain : MonoBehaviour {

	// Use this for initialization
	void Awake () {

        LightmapData[] lightmapData = LightmapSettings.lightmaps;
                
        GameObjectHelper.CopyLightMapData(GameObject.Find("Cube1").transform, GameObject.Find("Cube1Clone").transform);

        GameObject go = GameObject.Instantiate<GameObject>(GameObject.Find("Cube1"));
        go.transform.Translate(Vector3.left);
        go.transform.Translate(Vector3.up*2);
        go.transform.parent = gameObject.transform;
        

        GameObjectHelper.CopyLightMapData(GameObject.Find("Cube1").transform, go.transform);

        
        
        /*
        for (int i = 0; i < lightmapData.Length; i++)
        {
            LightmapData lightmap = new LightmapData();
            string path = string.Format("Lightmap/LightmapFar-{0}", i);
            lightmap.lightmapLight = Resources.Load<Texture2D>(path);
            lightmapData[i] = lightmap;
        }
         */
        LightmapSettings.lightmaps = lightmapData;

        StaticBatchingUtility.Combine(gameObject);
	}
	
	// Update is called once per frame
	void Update () {

        

	}
}
