using UnityEngine;
using System.Collections;


//本类存放一些常用Editor辅助方法
public class EditorHelper
{

    //清除Console的输出
    //[MenuItem("Tools/Clear Console %#c")] // CMD + SHIFT + C
    
    public static void ClearConsole()
    {
        // This simply does "LogEntries.Clear()" the long way:
        var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }

}
