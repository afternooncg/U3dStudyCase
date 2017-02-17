using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvStarTest : MonoBehaviour {


    private Transform[] hips; 
	// Use this for initialization
	void Start () {

        Inits1();
	}

    //生成单独GameObject用
    private void Inits()
    {
       GameObject prefab = Resources.Load("Female") as GameObject;
       GameObject role = Instantiate<GameObject>(prefab);
      // role.GetComponent<Anim>().Play("walk");

       prefab = Resources.Load("targetmodel") as GameObject;
       GameObject target = Instantiate<GameObject>(prefab);
       target.transform.localPosition = (new Vector3(1.0f, 0, 0));


       hips = target.GetComponentsInChildren<Transform>();
       SkinnedMeshRenderer[] sms = role.GetComponentsInChildren<SkinnedMeshRenderer>();

       for (int i = 0; i < sms.Length; i++)
       {
           GameObject go = new GameObject();
           SkinnedMeshRenderer sm = go.AddComponent<SkinnedMeshRenderer>();
           sm.sharedMesh = sms[i].sharedMesh;
           sm.materials = sms[i].sharedMaterials;

           List<Transform> bones1 = new List<Transform>();
           Transform[] bones =  sms[i].bones;

           for (int j = 0; j < bones.Length; j++)
           {
               for (int k = 0; k < hips.Length; k++)
               {
                   if (hips[k].name == bones[j].name)
                   {
                       bones1.Add(hips[k]);
                       break;
                   }
               }
           }

           sm.bones = bones1.ToArray();
           go.name = sms[i].name;
          go.transform.parent = target.transform;


           

       
       }

       if (target.GetComponent<Animation>() != null)
       {
           target.GetComponent<Animation>().wrapMode = WrapMode.Loop;
          target.GetComponent<Animation>().Play("01");
       }
       else

           Debug.Log("Empty Animation");
    }


    //合并Mesh用 当子mesh的材质个数不同，就会出现渲染异常,必须用生成GameObject的模式
    private void Inits1()
    {

        GameObject CubeGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
        CubeGo.transform.localScale = Vector3.one * 0.5f;

        GameObject prefab = Resources.Load("Female") as GameObject;
        GameObject role = Instantiate<GameObject>(prefab);
        // role.GetComponent<Anim>().Play("walk");

        prefab = Resources.Load("targetmodel") as GameObject;
        GameObject target = Instantiate<GameObject>(prefab);
        target.transform.localPosition = (new Vector3(1.0f, 0, 0));
        target.AddComponent<SkinnedMeshRenderer>();

        hips = target.GetComponentsInChildren<Transform>();
       

        SkinnedMeshRenderer[] sms = role.GetComponentsInChildren<SkinnedMeshRenderer>();



        List<CombineInstance> combineInstances = new List<CombineInstance>();

        List<Material> materials = new List<Material>();

        List<Transform> bones1 = new List<Transform>();

        for (int i = 0; i < sms.Length; i++)
        {

            CombineInstance ci = new CombineInstance();
            ci.mesh = sms[i].sharedMesh;
            combineInstances.Add(ci);

            materials.AddRange(sms[i].materials);
                      

        
            Transform[] bones = sms[i].bones;

            for (int j = 0; j < bones.Length; j++)
            {
                for (int k = 0; k < hips.Length; k++)
                {
                    if (sms[i].name == "hand-001" && (CubeGo.transform.parent == null))
                    {
                        CubeGo.transform.parent = hips[k];
                    }

                    if (hips[k].name == bones[j].name)
                    {
                        bones1.Add(hips[k]);
                        break;
                    }
                }
            }

        }

        SkinnedMeshRenderer sm =  target.GetComponent<SkinnedMeshRenderer>();
        sm.sharedMesh = new Mesh();
        sm.sharedMesh.CombineMeshes(combineInstances.ToArray(),false,false); //false flase参数很重要
        sm.materials = materials.ToArray();
        sm.bones = bones1.ToArray();

        if (target.GetComponent<Animation>() != null)
        {
            target.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            target.GetComponent<Animation>().Play("01");
        }
        else

            Debug.Log("Empty Animation");
    }

	// Update is called once per frame
	void Update () {
	
	}
}
