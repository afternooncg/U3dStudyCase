using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestChangeSprite : MonoBehaviour {

	public Image img1;
	public Image img2;
	// Use this for initialization
	void Start () {
	
		img1.sprite = img2.sprite;
		img1.overrideSprite = img2.sprite;
		img1.color = new Color (255, 0, 0, 122);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
