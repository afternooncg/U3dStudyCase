using UnityEngine;
using UnityEditor;


public class BuildDefine
{
    public static string[] VersionDefine = new string[]
    {
        "ANDROID_DEBUG_TW",
        
    };

    public static string[] Android_Debug_TW_defineLabels = new string[]
    {
        "DEBUG_MODE",
        "SHOW_LOGIN_LIST",
      
	};

    

#if ANDROID_DEBUG_TW    
    public static string[] VersionDefineLabels = Android_Debug_TW_defineLabels;
#else
    public static string[] VersionDefineLabels = new string[]{};
#endif

}


#if UNITY_EDITOR
[InitializeOnLoad]
public class AutoSetDefine
{

    static AutoSetDefine()
    {
        SetDefines();
    }

    public static void SetDefines()
    {
        string defineCommand = "";

        for (int i = 0; i < BuildDefine.VersionDefine.Length; i++)
        {
            defineCommand = defineCommand + BuildDefine.VersionDefine[i] + ";";
        }

        for (int i = 0; i < BuildDefine.VersionDefineLabels.Length; i++)
        {
            defineCommand = defineCommand + BuildDefine.VersionDefineLabels[i] + ";";
        }


#if UNITY_ANDROID
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defineCommand);
#elif UNITY_IPHONE
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iPhone, defineCommand);	
#elif UNITY_STANDALONE
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defineCommand);
#endif

        // display orientation
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

        // bundle ID
        PlayerSettings.bundleIdentifier = PubConfig.VERSION_INFO.BundleIdentifier;
        //  App Name
        PlayerSettings.productName = PubConfig.VERSION_INFO.ProductName;

        // version header + version middle
        PlayerSettings.bundleVersion = PubConfig.VERSION_INFO.VersionBigCode.ToString();
        // version end
        PlayerSettings.Android.bundleVersionCode = PubConfig.VERSION_INFO.VersionCode;
        /*
        // bundle version(big bundle version)
        PlayerSettings.bundleVersion = GlobalTypeDefine.VERSION_INFO.bundleVersion;
        // bundle version code(little bundle version)
        PlayerSettings.Android.bundleVersionCode = GlobalTypeDefine.VERSION_INFO.bundleVersionCode; 
        */
#if UNITY_IOS
		PlayerSettings.shortBundleVersion = VersionInfo.BUNDLEVERSION;
#endif
    }
}
#endif


