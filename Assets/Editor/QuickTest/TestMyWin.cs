using UnityEngine;
using System.Collections;
using UnityEditor;

public class TestMyWin : EditorWindow {



	// Use this for initialization    
    [MenuItem("QuickTest/EditorWindow/TestMyWin")]
	 static void Start () {

       EditorWindow win =  EditorWindow.GetWindow(typeof(TestMyWin));
       win.titleContent = new GUIContent("hello", "just test");

        
	}

    private enum BundleType
    {
        None = 0,
        Buildings = 1,
        Soldiers = 2,
        Bosses = 4,
        DestroyedBuildings = 8,
        Audio = 16,
        All = 0xFF
    }


    Texture2D texture ;
    bool isOk = false;
    BundleType type = BundleType.None;
    void OnGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Output Directory:");
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Input:", GUILayout.Width(100f));
        EditorGUILayout.FloatField(100.0f);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Input1:", GUILayout.Width(EditorGUIUtility.labelWidth+20));
        EditorGUILayout.FloatField(100.0f);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("点击", GUILayout.Width(EditorGUIUtility.labelWidth - 4)))
        {
            type |= BundleType.Buildings;
            type |= BundleType.Soldiers;
            Debug.Log("click ok!   " + isOk + " " + type);
        }

        texture = EditorGUILayout.ObjectField("add Texture:", texture, typeof(Texture),false) as Texture2D;
        
        GUILayout.BeginArea(new Rect(0, 200, 100, 100));
        isOk = GUILayout.Toggle(isOk, new GUIContent("ok"), GUILayout.Width(100f));
        GUILayout.EndArea();



        AssetBundle ab = AssetBundle.LoadFromFile(Application.dataPath + "/StreamAssets/exp2.assetbundle");
        Debug.Log(ab + "  " + ab.LoadAsset<Texture>("exp2"));
        Texture tex = ab.LoadAsset<Texture>("exp2");
        EditorGUI.DrawPreviewTexture(new Rect(25, 60, tex.width, tex.height), tex);
        ab.Unload(true);

    }

}
