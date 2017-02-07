using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.IO;
using System.Xml;

public static class BitmapFontImporter
{

    [MenuItem("Assets/Generate Bitmap Font")]
    public static void GenerateFont()
    {
        TextAsset selected = (TextAsset)Selection.activeObject;
        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selected));

        Debug.Log("rootPath:" + rootPath + " select.name:" + selected.name);

        Texture2D texture = AssetDatabase.LoadAssetAtPath(rootPath + "/" + selected.name + ".png", typeof(Texture2D)) as Texture2D;
        if (!texture) throw new UnityException("Texture2d asset doesn't exist for " + selected.name);

        string exportPath = rootPath + "/" + Path.GetFileNameWithoutExtension(selected.name);

       // Work(selected, exportPath, texture);
    }

    public static void Import(TextAsset rects, Font font, Vector3 size)
    {
        /*
        string[] lines = rects.text.Split(_splitFile, StringSplitOptions.None);
        CharacterInfo[] info = new CharacterInfo[lines.Length];
        for (var i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(_splitLine, StringSplitOptions.None);
            int x = Convert.ToInt32(line[0]);
            int y = Convert.ToInt32(line[1]);
            int width = Convert.ToInt32(line[2]);
            int height = Convert.ToInt32(line[3]);
            int offset = Convert.ToInt32(line[4]);
            info[i].uv.x = x / size.x;
            info[i].uv.y = y / size.y;
            info[i].uv.width = width / size.x;
            info[i].uv.height = height / size.y;
            info[i].vert.x = 0;
            info[i].vert.y = -offset;
            info[i].vert.width = width;
            info[i].vert.height = -height;
            info[i].width = width;
            info[i].index = i;
        }
        font.characterInfo = info;
        AssetDatabase.SaveAssets();
         */
    }



    private static void Work1(TextAsset import, string exportPath, Texture2D texture)
    {
      
    }

    private static int ToInt(XmlNode node, string name)
    {
        return Convert.ToInt32(node.Attributes.GetNamedItem(name).InnerText);
    }
}