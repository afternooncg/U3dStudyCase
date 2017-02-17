using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Profiler : MonoBehaviour {

    int[] fpsbuff;
    int fpsIndex;
    static string[] stringsFrom00To99 = {
		"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
		"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
		"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
		"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
		"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
		"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
		"60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
		"70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
		"80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
		"90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
	};
	// Use this for initialization
	void Start () {
        fpsbuff = new int[60];
        fpsIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {

        string a = stringsFrom00To99[Random.Range(0, 99)];
        //this.GetComponent<Text>().text = (1 / Time.unscaledDeltaTime).ToString();

        fpsbuff[fpsIndex++] =(int)(1.0f / Time.unscaledDeltaTime);
        if(fpsIndex>=60)
        {
            fpsIndex = 0;
            CalculateFPS();
        }

/*
        for (int i = 0; i < 1000; i++)
        {
            int j =  i;
            this.GetComponent<Text>().text = "abc" + j.ToString();
        }
 */
	}

    void CalculateFPS()
    {
        int sum = 0;
        for (int i = 0; i < fpsbuff.Length; i++)
        {
            sum += fpsbuff[i];
        }
        this.GetComponent<Text>().text = ((int)((float)sum / 60)).ToString();
    }
}
