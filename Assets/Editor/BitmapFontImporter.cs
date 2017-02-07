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
        if (!import) throw new UnityException(import.name + "is not a valid font-xml file");

        Font font = new Font();

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(import.text);

        XmlNode info = xml.GetElementsByTagName("info")[0];
        XmlNode common = xml.GetElementsByTagName("common")[0];
        XmlNodeList chars = xml.GetElementsByTagName("chars")[0].ChildNodes;

        float texW = texture.width;
        float texH = texture.height;

        CharacterInfo[] charInfos = new CharacterInfo[chars.Count];
        Rect r;

        for (int i = 0; i < chars.Count; i++)
        {
            XmlNode charNode = chars[i];
            CharacterInfo charInfo = new CharacterInfo();

            charInfo.index = ToInt(charNode, "id");
            charInfo.width = ToInt(charNode, "xadvance");
            charInfo.flipped = false;

            r = new Rect();
            r.x = ((float)ToInt(charNode, "x")) / texW;
            r.y = ((float)ToInt(charNode, "y")) / texH;
            r.width = ((float)ToInt(charNode, "width")) / texW;
            r.height = ((float)ToInt(charNode, "height")) / texH;
            r.y = 1f - r.y - r.height;
            charInfo.uv = r;


            r = new Rect();
            r.x = (float)ToInt(charNode, "xoffset");
            r.y = (float)ToInt(charNode, "yoffset");
            r.width = (float)ToInt(charNode, "width");
            r.height = (float)ToInt(charNode, "height");
            r.y = -r.y;
            r.height = -r.height;
            charInfo.vert = r;

            charInfos[i] = charInfo;
        }

        // Create material
        Shader shader = Shader.Find("UI/Default");
        Material material = new Material(shader);
        material.mainTexture = texture;
        AssetDatabase.CreateAsset(material, exportPath + ".mat");

        // Create font
        font.material = material;
        font.name = info.Attributes.GetNamedItem("face").InnerText;
        font.characterInfo = charInfos;
        AssetDatabase.CreateAsset(font, exportPath + ".fontsettings");
    }

    private static int ToInt(XmlNode node, string name)
    {
        return Convert.ToInt32(node.Attributes.GetNamedItem(name).InnerText);
    }
}