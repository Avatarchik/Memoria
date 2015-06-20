using UnityEngine;
using System.Collections;

public class FadeAttackScreen : MonoBehaviour {

	public static GUITexture texture; 
		// Use this for initialization
	void Start () {
		texture = GetComponent<GUITexture> ();
		texture.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void Flash() 
	{
		if (!texture.enabled) {
			texture.enabled = true;
		}
	}

	public static void DeFlash()
	{	
		 texture.enabled = false;
	}
}
