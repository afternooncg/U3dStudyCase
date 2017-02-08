using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PubConfig {

#if UNITY_EDITOR || UNITY_WINDOWS
    public static string PersiterPath = Application.dataPath.Replace("Assets", "PersiterData");
#else 
    public static string  PersiterPath = Application.persistentDataPath;
#endif
    public const string RemoteWWWRoot = "http://127.0.0.1:92";
}
