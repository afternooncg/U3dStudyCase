using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PubConfig {

#if UNITY_EDITOR || UNITY_WINDOWS
    public static string PersiterPath = Application.dataPath.Replace("Assets", "PersiterData");
#else 
    public static string  PersiterPath = Application.persistentDataPath;
#endif
    public const string RemoteWWWRoot = "http://10.0.16.49:92";

    public static int NGUILayer = LayerMask.NameToLayer("NGUI");
    public static int NGUIHIDDENLayer = LayerMask.NameToLayer("NGUIHIDDEN");


    public static VersionInfo VERSION_INFO = new VersionInfo("com.river.u3dstuycase","u3dStudy","1.0",1);
}



public struct VersionInfo
{
    public string BundleIdentifier;
    public string ProductName;
    public string VersionBigCode;
    public int VersionCode;

    public VersionInfo(string bundleIdentifier, string productName, string versionBigCode, int versionCode)
    {
        BundleIdentifier = bundleIdentifier;
        ProductName = productName;
        VersionBigCode = versionBigCode;
        VersionCode = versionCode;
    }
}
