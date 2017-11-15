using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

public class PrjAssetPostprocessor : AssetPostprocessor {

    // Use this for initialization
    public void OnPreprocessAudio()
    {
       // Debug.Log("音频导前预处理");       
        

        AudioImporterSampleSettings androidSetting = new AudioImporterSampleSettings();        
        //加载方式选择
        androidSetting.loadType = AudioClipLoadType.CompressedInMemory;
        //压缩方式选择
        androidSetting.compressionFormat = AudioCompressionFormat.Vorbis;
        //设置播放质量
        androidSetting.quality = 0.1f;
        //优化采样率
        androidSetting.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;


        AudioImporterSampleSettings iosSetting = new AudioImporterSampleSettings();
        //加载方式选择
        iosSetting.loadType = AudioClipLoadType.CompressedInMemory;
        //压缩方式选择
        iosSetting.compressionFormat = AudioCompressionFormat.MP3;
        //设置播放质量
        iosSetting.quality = 0.5f;
        //优化采样率
        iosSetting.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;


        AudioImporter audio = assetImporter as AudioImporter;

        
        //开启单声道 
        audio.forceToMono = true;
        audio.preloadAudioData = true;
        //audio.defaultSampleSettings = AudioSetting;

        // "WebPlayer", "Standalone", "iOS", "Android", "WebGL", "PS4", "PSP2", "XBoxOne", "Samsung TV".
        audio.SetOverrideSampleSettings("Android", androidSetting);
        audio.SetOverrideSampleSettings("iOS", iosSetting);


        Debug.Log(System.IO.Path.GetDirectoryName(this.assetPath));
        
        
    }

    public void OnPostprocessAudio(AudioClip clip)
    {
        Debug.Log(System.IO.Path.GetDirectoryName(this.assetPath));
       // Debug.Log("音频导后处理");
        if (Profiler.GetRuntimeMemorySizeLong(clip) / 1024 > 100f)
        {           
            AudioImporter audio = assetImporter as AudioImporter;
            AudioImporterSampleSettings iosSetting = audio.GetOverrideSampleSettings("iOS");
            AudioImporterSampleSettings androidSetting = audio.GetOverrideSampleSettings("Android");
            iosSetting.loadType = AudioClipLoadType.Streaming;
            androidSetting.loadType = AudioClipLoadType.Streaming;
            audio.SetOverrideSampleSettings("Android", androidSetting);
            audio.SetOverrideSampleSettings("iOS", iosSetting);
            AssetDatabase.SaveAssets();
        }
        


    }
}
