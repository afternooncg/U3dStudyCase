using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileListData : ScriptableObject {


    public List<string> Files;

    public FileListData()
    {
        Files = new List<string>();
    }
}
