using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestDataAsset : ScriptableObject {

    public List<string> PubData = new List<string>();

    public void CreateData()
    {

        for (int i = 0; i < 5; i++)
        {
            PubData.Add(i.ToString());
        }
    
    }
}
