using UnityEngine;
using System.Collections;
using UnityEditor;

public class BeginEndScrollView : EditorWindow {

    Vector2 scrollPosi;
    string t = "This is a string inside a Scroll view!";

    [MenuItem("QuickTest/EditorWindow/ScrollViewMyWindow")]
	// Use this for initialization
	public static void ShowWin() {
	
        EditorWindow.GetWindow<BeginEndScrollView>();
	}
	
	// Update is called once per frame
	void OnGUI () 
    {
        EditorGUILayout.BeginHorizontal();

        scrollPosi = EditorGUILayout.BeginScrollView(scrollPosi, GUILayout.Width(100), GUILayout.Height(100));
        EditorGUILayout.Space();
        GUILayout.Label(t);
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("add more button"))
        {
            t += "add more string \n";
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear"))
            t = "";


        GUILayout.Box("盒子", GUILayout.Width(200), GUILayout.Height(100));
        GUILayout.Label("盒子里？");
        GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");
        GUILayout.BeginHorizontal();
        GUILayout.BeginArea(new Rect(100, 500, 200, 200));
        GUILayout.Label("Area");
        GUILayout.EndArea();
        GUILayout.EndHorizontal();

	}
}
