using UnityEngine;
using UnityEditor;

using System;
using System.Collections.Generic;

[AssetBundleGraph.CustomModifier("MyModifier0", typeof(TextureImporter))]
public class MyModifier0 : AssetBundleGraph.IModifier {

	[SerializeField] private bool doSomething;

	// Test if asset is different from intended configuration 
	public bool IsModified (UnityEngine.Object[] assets) {
		return false;
	}

	// Actually change asset configurations. 
	public void Modify (UnityEngine.Object[] assets) {
	}

	// Draw inspector gui 
	public void OnInspectorGUI (Action onValueChanged) {

		EditorGUILayout.HelpBox("This is the inspector of your custom Modifier. You can customize by implementing OnInspectorGUI().", MessageType.Info);

		var newValue = GUILayout.Toggle(doSomething, "Example toggle");
		if(newValue != doSomething) {
			doSomething = newValue;
			onValueChanged();
		}
	}

	// serialize this class to JSON 
	public string Serialize() {
		return JsonUtility.ToJson(this);
	}
}
