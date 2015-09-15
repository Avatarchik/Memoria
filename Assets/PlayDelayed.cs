using UnityEngine;
using System.Collections;

public class PlayDelayed : MonoBehaviour {

    AudioSource _audio;

    public float delay;
	// Use this for initialization
	void Start () {
        _audio = GetComponent<AudioSource>();
        _audio.PlayDelayed(delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
