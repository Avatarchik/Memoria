using UnityEngine;
using System.Collections;

public class LightEffect : MonoBehaviour {

    [SerializeField]
    private GameObject prefab;

    public void Execute()
    {
        Vector3 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tapPoint.z = 0;
        var effect = Instantiate(prefab, tapPoint, Quaternion.identity);
        Destroy(effect, 2.0f);
    }

}
