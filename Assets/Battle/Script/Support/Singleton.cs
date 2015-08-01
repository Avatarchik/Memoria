using UnityEngine;

abstract public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance =  GameObject.FindObjectOfType(typeof(T)) as T;

                if(_instance == null) {
                    Debug.LogError("[E] Instance" + typeof(T).ToString()
                                   + "not found!");
                    return null;
                }
                _instance.Init();
            }
            return _instance;
        }
    }

    protected virtual void Init() {}

    void Awake() {
        if(_instance == null) {
            _instance = this as T;
            _instance.Init();
        }
    }


    void OnApplicationQuit()
    {
        _instance = null;
    }

}

