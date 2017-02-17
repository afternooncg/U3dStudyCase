using UnityEngine;
using System.Collections;

public class DataHolder : ScriptableObject
{

    public string[] strings;
    public int[] integers;
    public float[] floats;
    public bool[] booleans;
    public byte[] bytes;

    public void Init()
    {
        strings = new string[10];
        integers = new int[10];

        for (int i = 0; i < 10; i++)
        {
            strings[i] = "文本" + i.ToString();
            integers[i] = i;
        }
    }
}