using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Director;

public class PrefabEditWin : EditorWindow
{
    public enum ePrefabType
    {
        PREFABTYPE_BLUE,    //藍色小兵prefab
        PREFABTYPE_RED,       //紅色小兵prefab
    }





    [MenuItem("JustTest/辅助编辑器")]
    public static void OpenMyWindow()
    {
        Rect rect = new Rect(0, 0, 500, 500);
        PrefabEditWin window = (PrefabEditWin)EditorWindow.GetWindowWithRect(typeof(PrefabEditWin), rect, true, "辅助编辑器");
        window.Show();

    }

    int count = 0;
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();


        if (GUILayout.Button(new GUIContent("解析动画")))
        {
            ParseAndSaveAnimation();
        }

        if (GUILayout.Button(new GUIContent("解析动画并生成状态机")))
        {
            CreateAnimatorController();
        }




        if (GUILayout.Button(new GUIContent("动画混合")))
        {
            GameObject go = Selection.activeGameObject;
            if (!go)
                return;

            Animator amtor = go.GetComponent<Animator>();

            RuntimeAnimatorController controll = amtor.runtimeAnimatorController;

            AnimationClip[] clips = controll.animationClips;

            for (int i = 0; i < clips.Length; i++)
            {
                //  Debug.Log(clips[i].name);
            }


            AnimationClip attack = clips[0];
            AnimationClip idle = clips[1];

            Debug.Log(amtor.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Attack"));

            AnimatorClipInfo[] infos = amtor.GetCurrentAnimatorClipInfo(0);
            for (int i = 0; i < infos.Length; i++)
            {
                Debug.Log(infos[i].clip.name);
                
            }

            
            


            //amtor.SetLayerWeight(1, 0.5f);
            amtor.enabled = true;
            //amtor.Stop();

            amtor.Rebind();
            amtor.Play("Attack");
            EditorApplication.update += delegate()
            {
                count++;

                if (count >= 400)
                {

                   // amtor.Stop();

                    amtor.Rebind();
                    amtor.Play("Idle");

                    EditorApplication.update = null;
                }


            };

            
           

            
          //  amtor.Rebind();
            
        }
        if (GUILayout.Button(new GUIContent("更新Prefab")))
        {
            UpdatePrefab();
        }

        if (GUILayout.Button(new GUIContent("DisconnectPrefabInstance")))
        {
            DisconnectPrefabInstance();
        }

        


        EditorGUILayout.EndVertical();
    }




    void ParseAndSaveAnimation()
    { 
        string fbxpath = "Assets/JustTest/PrefabEdit/Shield_0/Solider_shield_0.fbx";
        string savePath = "Assets/JustTest/PrefabEdit/Animation";

        string[] allas = AssetDatabase.FindAssets("",new string[]{savePath});
        for (int i = 0; i < allas.Length; i++)
            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(allas[i]));
                
        

        GameObject fbx  =  AssetDatabase.LoadAssetAtPath<GameObject>(fbxpath);



        UnityEditor.Animations.AnimatorController newcontroller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(savePath + "/" + fbx.name + ".controller");

              


          //读取AnimationClip 并创建 Controller;
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(fbxpath);
                
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].GetType() != typeof(AnimationClip))
                continue;

            AnimationClip sourcesClip = objs[i] as AnimationClip;
            if (sourcesClip.name.Contains("preview") == true)       // use "preview" or "__"
                continue;

           Debug.Log(objs[i].name + "  " + objs[i].GetType() + "  " + objs[i].ToString());

            AnimationClip  targetClip = new AnimationClip();
            targetClip.name = objs[i].name;
            AnimationUtility.SetAnimationEvents(targetClip, sourcesClip.events);
            EditorUtility.CopySerialized(sourcesClip, targetClip);

            newcontroller.AddMotion(targetClip);
            AssetDatabase.CreateAsset(targetClip, savePath + "/" + sourcesClip.name + ".anim");
        }


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    void UpdatePrefab()
    {
        if (Selection.activeGameObject)
        {
            GameObject go = Selection.activeGameObject;

            PrefabUtility.ReplacePrefab(GameObject.CreatePrimitive(PrimitiveType.Sphere), PrefabUtility.GetPrefabParent(go),ReplacePrefabOptions.ConnectToPrefab);

            //ReplacePrefabOptions.ReplaceNameBased, ReplacePrefabOptions.ConnectToPrefab 区别 前者保留自定义属性,后者全部初始初始化为Prefab默认属性
        }
    }


    void DisconnectPrefabInstance()
    {
        if (Selection.activeGameObject)
        {
            GameObject go = Selection.activeGameObject;
            PrefabUtility.DisconnectPrefabInstance(go);     //切断 prefab源的自动影响        
            //PrefabUtility.CreatePrefab(AssetDatabase.GetAssetPath(go), go, ReplacePrefabOptions.ConnectToPrefab);
        }
    }


    void DelAssetFiles(string path)
    {
      string[] allas = AssetDatabase.FindAssets("",new string[]{path});
        for (int i = 0; i < allas.Length; i++)
            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(allas[i]));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    void CreateAnimatorController()
    {    
        string fbxpath = "Assets/JustTest/PrefabEdit/Shield_0/Solider_shield_0.fbx";
        string savePath = "Assets/JustTest/PrefabEdit/Animation1";

        if(!System.IO.Directory.Exists(savePath))
            System.IO.Directory.CreateDirectory(savePath);

        DelAssetFiles(savePath);
        
        GameObject fbx  =  AssetDatabase.LoadAssetAtPath<GameObject>(fbxpath);

        


          //读取AnimationClip 并创建 Controller;
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(fbxpath);
        List<AnimationClip> list = new List<AnimationClip>();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].GetType() != typeof(AnimationClip))
                continue;

            AnimationClip sourcesClip = objs[i] as AnimationClip;
            if (sourcesClip.name.Contains("preview") == true)       // use "preview" or "__"
                continue;

            list.Add(sourcesClip);
        }

         
	    //创建一个控制器
	    UnityEditor.Animations.AnimatorController animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(savePath + "/test.controller");
	    //获取控制器的layer
	    UnityEditor.Animations.AnimatorControllerLayer layer = animatorController.layers[0];
	    //获取状态机
	    AnimatorStateMachine machine = layer.stateMachine;
	    //添加一个坐标，让状态从这个位置开始摆放，防止状态乱摆，巨丑
	    Vector3 dpos = new Vector3(300,0,0);
        foreach (AnimationClip clip in list)
	    {
		    AnimatorState state = new AnimatorState(); //machine.AddState(clip.name);
		    state.motion = clip;
		    state.name = clip.name;
		    machine.AddState(state, dpos);
            machine.AddAnyStateTransition(state);

		    dpos += new Vector3(0,50,0);//改变下一个坐标，让状态排成一列
		    if(state.name.Equals("Idle"))
		    {                              

			    machine.defaultState = state;//设置默认状态

                AnimationClip targetClip = new AnimationClip();
                targetClip.name = clip.name;
                AnimationUtility.SetAnimationEvents(targetClip, clip.events);
                EditorUtility.CopySerialized(clip, targetClip);
                AssetDatabase.CreateAsset(targetClip, savePath + "/" + clip.name + ".anim");
                state.motion = targetClip;

               // machine.RemoveState(machine.states[0].state);
		    }
	    }

        /*
        animatorController.AddLayer("1");
        animatorController.AddLayer("2");
        animatorController.AddLayer("3");
        animatorController.AddLayer("3");
        animatorController.RemoveLayer(1);

        while (animatorController.layers.Length > 0)
            animatorController.RemoveLayer(0);
         */
    }
}