using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareMatMain : MonoBehaviour {

	// Use this for initialization

    public GameObject c1;
    public GameObject c2;
    public GameObject c3;
    public GameObject c4;

    Texture tex;
	void Start () {
		
	}


    enum GetType
    { 
        material,
        shareMaterial,
        materialsArray,
        shareMaterialArray
    }
    void watchAccessGet(GetType type)
    {
        Material mat;
        switch (type)
        { 
            case GetType.material:
                mat = c1.GetComponent<Renderer>().material;  //生成1个mat instance;
                break;
            case GetType.shareMaterial:
                mat = c1.GetComponent<Renderer>().sharedMaterial;  //不生成新mat instance;
                break;
            case GetType.materialsArray:
                mat = c1.GetComponent<Renderer>().materials[0];  //生成1个新mat instance;
                break;
            case GetType.shareMaterialArray:
                mat = c1.GetComponent<Renderer>().sharedMaterials[0];  //不生成新mat instance;
                break;
        }


    }

	// Update is called once per frame
	void OnGUI () {

        if (GUI.Button(new Rect(10, 10, 150, 20), "访问Cube1 mat"))
            watchAccessGet(GetType.material);

        if (GUI.Button(new Rect(170, 10, 150, 20), "访问Cube1 sharemat"))
            watchAccessGet(GetType.shareMaterial);

        if (GUI.Button(new Rect(320, 10, 150, 20), "访问Cube1 mat ary"))
            watchAccessGet(GetType.materialsArray);

        if (GUI.Button(new Rect(470, 10, 150, 20), "访问Cube1 sharemat art"))
            watchAccessGet(GetType.shareMaterialArray);


        if (GUI.Button(new Rect(10, 40, 150, 20), "sharemat批量修改"))
        {
            Material mat1 = c1.GetComponent<Renderer>().sharedMaterial;
            mat1.SetColor("_Color", Color.white);   //注意editor模式，会直接修改asset 
        }


        if (GUI.Button(new Rect(10, 70, 150, 20), "重复修改mat"))
        {

            //只会生成1个新的mat
            Material mat1 = c1.GetComponent<Renderer>().material;
            mat1.SetColor("_Color", Color.white);
            mat1 = c1.GetComponent<Renderer>().material;
            mat1.SetColor("_Color", Color.blue);
            mat1 = c1.GetComponent<Renderer>().material;
            mat1.SetColor("_Color", Color.green);     
        }


        if (GUI.Button(new Rect(170, 70, 150, 20), "修改mat后再改share"))
        {
            Material mat1 = c1.GetComponent<Renderer>().material;
            mat1.SetColor("_Color", Color.white);
            mat1 = c1.GetComponent<Renderer>().material;
            mat1.SetColor("_Color", Color.blue);
            mat1 = c1.GetComponent<Renderer>().material;
            mat1.SetColor("_Color", Color.green);     //只会改1次

            c1.GetComponent<Renderer>().material = c2.GetComponent<Renderer>().sharedMaterial;
            mat1 = c1.GetComponent<Renderer>().material;
            Resources.UnloadUnusedAssets();
        }


        if (GUI.Button(new Rect(10, 100, 150, 20), "访问单例mat"))
        {
            
            Material sharmat = c3.GetComponent<Renderer>().sharedMaterial;
            Debug.Log(c3.GetComponent<Renderer>().material == sharmat);    //flase 生成新mat

            Debug.Log(c3.GetComponent<Renderer>().material == sharmat);     //false不再生成  

            Material sharmat1 = c3.GetComponent<Renderer>().sharedMaterial;  //此时已是新生的sharemat了

            Debug.Log(c3.GetComponent<Renderer>().material == sharmat1);     //true不再生成  


            Resources.UnloadUnusedAssets();


            c3.GetComponent<Renderer>().sharedMaterial = c1.GetComponent<Renderer>().sharedMaterial;
            Debug.Log(c3.GetComponent<Renderer>().material == c3.GetComponent<Renderer>().sharedMaterial);     //true 没有新生成  

            c3.GetComponent<Renderer>().material.SetColor("_Color", Color.green); //不再生成新材质
            //Debug.Log();  

        }


        if (GUI.Button(new Rect(10, 130, 150, 20), "修改mat后销毁go"))
        {
            Material sharmat = c1.GetComponent<Renderer>().material;
            GameObject.Destroy(c1);
            Resources.UnloadUnusedAssets();     //clone出来的可以被回收

        }

        if (GUI.Button(new Rect(320, 130, 150, 20), "单独加载tex"))
        {
            tex = (Resources.Load<Texture>("Common/UISkinSpace"));
            
        }

        if (GUI.Button(new Rect(170, 130, 150, 20), "修改mat后销毁go_非clone"))
        {
            
#if UNITY_EDITOR
            GameObject asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/JustTest/MemTest/ShareMat/Cube1.prefab");
            Debug.Log(UnityEditor.AssetDatabase.Contains(asset) + " " + asset.GetInstanceID());            
            Debug.Log(UnityEditor.AssetDatabase.Contains(c1) + " " + c1.GetInstanceID());
#endif

            c1.GetComponent<Renderer>().material.mainTexture = tex;

            if(tex!=null)
                Resources.UnloadAsset(tex);
                        
           // tex = null;
            Debug.Log("tex:" + (tex == null) + " "  + tex.GetInstanceID());            
            c2.GetComponent<Renderer>().material.mainTexture = tex;   //很神奇,又恢复了
            Debug.Log("tex:" + (tex == null) + " " + tex.GetInstanceID());            
            return;
            if (c1 == null)
                return;

            Material sharmat = c1.GetComponent<Renderer>().material;
            c1.GetComponent<Renderer>().material = new Material(Shader.Find("MyTest/ChangeColorShader"));
            

            Texture tex1 = Resources.Load<Texture>("Common/UISkinSpace");

            Debug.Log("tex1 == tex " + (tex == tex1));
            c1.GetComponent<Renderer>().sharedMaterial.mainTexture = tex1;
            
            GameObject.Destroy(c1);
           // Resources.UnloadUnusedAssets();     //clone出来的可以被回收

        }


        return;
        {
            //Material mat = c1.GetComponent<Renderer>().material;  //生成1个mat instance;
            //Material mat = c1.GetComponent<Renderer>().sharedMaterial;  //不生成1个mat instance;
            //Material mat = c1.GetComponent<Renderer>().sharedMaterials[0];  //不会生成1个mat instance;
            //Material mat = c1.GetComponent<Renderer>().materials[0];  //生成1个mat instance;

            Material mat = c1.GetComponent<Renderer>().sharedMaterial;

            c1.GetComponent<Renderer>().material.SetColor("_Color", Color.green); //只改自己，生成新instance;
            c1.GetComponent<Renderer>().material.SetColor("_Color", Color.blue); //只改自己，生成新instance;
            


            Material mat1 = c1.GetComponent<Renderer>().sharedMaterial;

            c3.GetComponent<Renderer>().material = mat1;

            mat1.SetColor("_Color", mat.color);


            Debug.Log("mat1 == mat " + (mat == mat1));   //不成立
            Debug.Log("mat1 == c3mat share mat" + (mat1 == c3.GetComponent<Renderer>().sharedMaterial));   //成立
            Debug.Log("mat1 == c3mat mat" + (mat1 == c3.GetComponent<Renderer>().material));   //不成立

            c3.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            Debug.Log("mat1 == c3mat share mat" + (mat1 == c3.GetComponent<Renderer>().sharedMaterial));   //不成立 
            Debug.Log("mat1 == c3mat mat" + (mat1 == c3.GetComponent<Renderer>().material));   //不成立

            //c1.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red); //批量改，不生成新instance;
        }

	}
}
