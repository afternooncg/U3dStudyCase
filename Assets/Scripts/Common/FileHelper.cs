using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// 文件辅助类
/// </summary>

public class FileHelper
{

    // Use this for initialization
    #region  创建文本文件
    public static void CreateTxtFile(string path, string content, bool isAppend = false)
    {
        try
        {
            //文件流信息  
            StreamWriter sw;
            FileInfo t = new FileInfo(path);
            if (!t.Exists || !isAppend)
            {
                //如果此文件不存在则创建  
                sw = t.CreateText();
            }
            else
            {
                //如果此文件存在则打开  
                sw = t.AppendText();

            }

            //以行的形式写入信息  
            sw.Write(content);
            //关闭流  
            sw.Close();
            //销毁流  
            sw.Dispose();
        }
        catch (Exception e)
        {
            Debug.LogWarning("写文本文件失败" + e.Message);
        }

    }
    #endregion

    #region 创建二进制文件
    public static void CreateBinFile(string path, byte[] info, int length)
    {
        try
        {
            //文件流信息  
            //StreamWriter sw;  
            Stream sw;
            FileInfo t = new FileInfo(path);
            if (t.Exists)
                File.Delete(path);

            sw = t.Create();

            sw.Write(info, 0, length);
            //关闭流  
            sw.Close();
            //销毁流  
            sw.Dispose();
        }
        catch (Exception e)
        {
            Debug.LogWarning("写二进制文件失败" + e.Message);
        }
    }
    #endregion

    #region 删除文件
    public static void DeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        catch (Exception e)
        {
            Debug.LogWarning(" 删除文件失败" + e.Message);
        }
    }
    #endregion


    #region 读取文本文件
    public static string ReadTxtFile(string path)
    {
        try
        {
            if (!File.Exists(path))
                return "";

            FileInfo fi = new FileInfo(path);

            StreamReader sr = fi.OpenText();
            string str = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            return str;
        }
        catch (Exception e)
        {
            Debug.LogWarning("读取文件失败" + e.Message);
            return "";
        }
    }
    #endregion

    #region 递归复制目录
    public static void CopyDirectory(string source, string destination)
    {
        if (Directory.Exists(source))
        {
            if (Directory.Exists(destination) == false)
            {
                Directory.CreateDirectory(destination);//创建目录
            }
            string[] files = Directory.GetFiles(source);//获取所有子文件
            for (int i = 0; i < files.Length; i++)
            {
                //复制文件
                File.Copy(files[i], destination + files[i].Substring(files[i].LastIndexOf('\\')), true);
            }
            string[] directories = Directory.GetDirectories(source);//获取所有子目录
            for (int i = 0; i < directories.Length; i++)
            {
                //递归复制子目录
                CopyDirectory(directories[i], destination + directories[i].Substring(directories[i].LastIndexOf('\\')));
            }
        }
    }
    #endregion
}
