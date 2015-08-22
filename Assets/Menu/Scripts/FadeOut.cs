using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {

    public void LoadLevel(string name)
    {
        StartCoroutine(Fade(name));
    }

    public IEnumerator Fade(string level)
    {
        Application.LoadLevel(name);
        return null;
    }
}
