using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(ColorPoint))]
public class ColorPointDrawer : UnityEditor.PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Debug.Log("label.text:" + label.text);
        int oldIndentLevel = EditorGUI.indentLevel;
        /*
        EditorGUI.PrefixLabel(position, label);
         
        //EditorGUI.PropertyField(position, property.FindPropertyRelative("position"));
        EditorGUI.PropertyField(position, property.FindPropertyRelative("position"), GUIContent.none); //隐藏lable
         */

        label = EditorGUI.BeginProperty(position, label, property);  //BeginProperty EndProperty 标签允许对当前块做删除和复制 右键菜单
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);  //contentPosition，不包含filed左边标签占有区域的剩余Rect

        if (position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18f;
        }

        contentPosition.width *= 0.75f; //整个标签的宽度
        EditorGUI.indentLevel = 0; //控制层级表现
       EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("position"), GUIContent.none);
       contentPosition.x += contentPosition.width; 
       contentPosition.width /= 3f;
       EditorGUIUtility.labelWidth = 14f;//指定左侧标签长度
       EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("color"), new GUIContent("C"));               
       EditorGUI.EndProperty();
       EditorGUI.indentLevel = oldIndentLevel;
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {//默认分格线是16f
        return label != GUIContent.none && Screen.width < 333 ? (16f + 18f) : 16f;
    }
}
