using UnityEngine;

abstract public class Singleton<T> : MonoBehaviour where T : class
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance =  GameObject.FindObjectOfType(typeof(T)) as T;
            }
            return _instance;
        }
    }
    
}
