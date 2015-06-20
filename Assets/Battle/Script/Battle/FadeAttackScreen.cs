using UnityEngine;
using System.Collections;

public class FadeAttackScreen : MonoBehaviour {

    public static Sprite[] bgSprites;
    public static GameObject bgObj;
    // Use this for initialization
    void Start () {
        bgSprites = new Sprite[2];
        for (int i = 0 ; i < 2 ; i++) 
        {
            bgSprites[i] = Resources.Load<Sprite>("bg" + i);
        }
        bgObj = GameObject.FindGameObjectWithTag("Bg") as GameObject;
        bgObj.GetComponent<SpriteRenderer>().sprite = bgSprites[0];
    }
    // Update is called once per frame
    void Update () {

    }

    public static void Flash() 
    {
        bgObj.GetComponent<SpriteRenderer>().sprite = bgSprites[1];
    }

    public static void DeFlash()
    {
        bgObj.GetComponent<SpriteRenderer>().sprite = bgSprites[0];
    }
}
