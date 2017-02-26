using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileListData : ScriptableObject {

    [SerializeField]
    public List<string> Files;

    /*
    public FileListData()
    {
        Files = new List<string>();
    }
     */
}
