using UnityEngine;
using System.Collections;

public class TrailObject : MonoBehaviour {

    public GameObject objectToFollow;

    Vector3 pos;
    void Start()
    {
    }

    void Update ()
    {
        pos = objectToFollow.transform.position;
        this.transform.position = new Vector3(pos.x - 1.0f, pos.y, 1);
       
    }
}
