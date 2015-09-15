using UnityEngine;
using System.Collections;
using Memoria.Managers;

public class Aud : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SoundManager.instance.PlayBGM (4);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
