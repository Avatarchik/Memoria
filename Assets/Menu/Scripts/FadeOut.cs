using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Memoria.Battle.Utility;

public class FadeOut : Singleton<FadeOut> {

    [SerializeField]
    private Canvas _canvasPrefab;

    private Image fadeScreen;

    private bool fading;

    private Canvas _fadeCanvas;

    private Color c;

    public enum FadeTo
    {
        FADE_OUT = 0,
        FADE_IN = 1
    }

    public FadeTo fadeTo;
    public float speed;
    public bool useFading;

    public void LoadLevel(string name)
    {
        if(!useFading) {
            Application.LoadLevel(name);
        } else {

            if(_fadeCanvas == null) {
                _fadeCanvas = Instantiate(_canvasPrefab, _canvasPrefab.transform.position, Quaternion.identity) as Canvas;
            }

            fadeScreen = _fadeCanvas.GetComponentInChildren<Image>();
            c = fadeScreen.color;
            c.a = (float)fadeTo;
            fadeScreen.color = c;

            StartCoroutine(FadeRoutine(name));
        }
    }

    private IEnumerator FadeRoutine(string name)
    {
        float elapsed = 0;
        yield return new WaitForSeconds(2.0f);

        while(elapsed < speed)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp((float)fadeTo, 1 - (float)fadeTo, elapsed / speed);
            fadeScreen.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        Application.LoadLevel(name);
    }
}
