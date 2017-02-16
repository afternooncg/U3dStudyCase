using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICC2Load
{

    int Total { get; }
    bool Loading(ref int loadCount);
}

public class GameLoad : MonoBehaviour
{



    public enum GameLoadStep
    {
        GameData,
        BuildingData,
        Module1,
        Module2,
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
