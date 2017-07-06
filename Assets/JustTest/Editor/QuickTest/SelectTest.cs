using UnityEngine;
using System.Collections;
using UnityEditor;

public class SelectTest  {

    [MenuItem("QuickTest/Select/transforms")]
    //只能选中视图成的物体，其他的都为null。与Selection.activeGameObject
    //如果同时选择父子物体，只会取到父对象
    public static void Execute()
    {
        Transform[] transforms = Selection.transforms;
        for (int iter = 0; iter < transforms.Length; ++iter)
        {
            Debug.Log(transforms[iter].gameObject.name);
        }
    }
    

    [MenuItem("QuickTest/Select/gameObjects")]
    //Selection.gameObjects 返回的是被你选中的游戏物体，可以包括预设。
    //可以是在Assets文件下也可以在视图层里的物体
    //如果同时选择父子物体，都会取到
    public static void Execute2()
    {
        GameObject[] gameObjects = Selection.gameObjects;
        for (int iter = 0; iter < gameObjects.Length; ++iter)
        {
            Debug.Log(gameObjects[iter].name);
        }
    }

    [MenuItem("QuickTest/Select/activeObject")]
    //他返回的可以是任何物体，比如文件，贴图等等。
    public static void Execute1()
    {
        Debug.Log(Selection.activeObject);
    }
    

    [MenuItem("QuickTest/Select/Objects")]
    //他返回的可以是任何物体，比如文件，贴图等等。与Selection.activeObject类似
    public static void Execute3()
    {
        
        Object[] objects = Selection.objects;
        for (int iter = 0; iter < objects.Length; ++iter)
        {
            Debug.Log(objects[iter].name);
        }
    }


    [MenuItem("QuickTest/Select/SelectionMode")]
    public static void Execute4()
    {
        for (int iter = (int)SelectionType.UNFILTERED; iter < (int)SelectionType.TOTAL; ++iter)
        {
            switch ((SelectionType)iter)
            {
                case SelectionType.UNFILTERED:
                    Object[] object1 = Selection.GetFiltered(typeof(object), SelectionMode.Unfiltered);
                    _Print(object1, "Unfiltered");
                    break;
                case SelectionType.TOPLEVEL:
                    Object[] object2 = Selection.GetFiltered(typeof(object), SelectionMode.TopLevel);
                    _Print(object2, "TopLevel");
                    break;
                case SelectionType.ASSESTS:
                    Object[] object3 = Selection.GetFiltered(typeof(object), SelectionMode.Assets);
                    _Print(object3, "Assests");
                    break;
                case SelectionType.DEEP:
                    Object[] object4 = Selection.GetFiltered(typeof(object), SelectionMode.Deep);
                    _Print(object4, "Deep");
                    break;
                case SelectionType.DEEPASSETS:
                    Object[] object5 = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
                    _Print(object5, "DeepAssets");
                    break;
                case SelectionType.EXCLUDEPREFAB:
                    Object[] object6 = Selection.GetFiltered(typeof(object), SelectionMode.ExcludePrefab);
                    _Print(object6, "ExcludePrefab");
                    break;
                case SelectionType.EDITABLE:
                    Object[] object7 = Selection.GetFiltered(typeof(object), SelectionMode.Editable);
                    _Print(object7, "Editable");
                    break;
            }
        }
    }



    [MenuItem("QuickTest/Select/ReplacePrefab")]
    public static void Execute5()
    {
        //PrefabUtility.ReplacePrefab(Selection.activeGameObject, PrefabUtility.GetPrefabParent(Selection.activeGameObject), ReplacePrefabOptions.ConnectToPrefab);
        PrefabUtility.ReplacePrefab(Selection.activeGameObject, PrefabUtility.GetPrefabParent(Selection.activeGameObject), ReplacePrefabOptions.ReplaceNameBased);
      //  PrefabUtility.ReplacePrefab(Selection.activeGameObject, PrefabUtility.GetPrefabParent(Selection.activeGameObject));
    }

    static void _Print(Object[] objects, string type)
    {
        Debug.Log(type + "-----------------------------");
        for (int iter = 0; iter < objects.Length; ++iter)
        {
            Debug.Log(objects[iter].name);             
        }
        Debug.Log("*****************************");
    }


    [MenuItem("Assets/Select/GetTransforms 选取模式")]
    static void GetTransformsAndAddChild()
    {
        Debug.Log("craet empty game object");
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);

        foreach (Transform transform in transforms)
        {
            GameObject newChild = new GameObject("_Child");
            newChild.transform.parent = transform;
        }
    }


    enum SelectionType
    {
        UNFILTERED,
        TOPLEVEL,
        DEEP,
        EXCLUDEPREFAB,
        EDITABLE,
        ASSESTS,
        DEEPASSETS,
        TOTAL
    }
}
