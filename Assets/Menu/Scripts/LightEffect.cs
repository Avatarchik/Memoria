using UnityEngine;

public class LightEffect : MonoBehaviour {

    [SerializeField]
    private GameObject _effect;

    public void Update()
    {
    }

    public void Execute()
    {
        Vector3 tapPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        tapPoint.z = 0;
        var effect = Instantiate(_effect, tapPoint, Quaternion.identity);
        DestroyObject(effect, 2f);
    }
}
