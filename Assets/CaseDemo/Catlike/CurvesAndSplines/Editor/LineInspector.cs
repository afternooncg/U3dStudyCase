using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{
    Vector3 p3 = Vector3.one;
    private void OnSceneGUI()
    {
        Line line = target as Line;

        Handles.color = Color.white;
        
        Transform handleTransform = line.transform;
        
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = handleTransform.TransformPoint(line.p0);
        Vector3 p1 = handleTransform.TransformPoint(line.p1);


        Handles.DrawLine(p0, p1);



        Handles.DoPositionHandle(p0, handleRotation);
        Handles.DoPositionHandle(p1, handleRotation);


        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p0 = handleTransform.InverseTransformPoint(p0);
        }
        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p1 = handleTransform.InverseTransformPoint(p1);
        }



        Vector3 p2 =new  Vector3(3f, 0f, 0f);
        Vector3 p3 =new  Vector3(4f, 0f, 0f);

        Handles.DrawBezier(p0, p3, p1, p2, Color.red, null, 2f);

        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);
    }

}


