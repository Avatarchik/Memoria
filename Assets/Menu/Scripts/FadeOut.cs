﻿using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {

    public void LoadLevel(string name)
    {
        Application.LoadLevel(name);
    }

    public IEnumerator Fade(string level)
    {
        yield return null;
    }
}
