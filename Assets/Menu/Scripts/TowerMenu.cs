using UnityEngine;
using System.Collections.Generic;

public class TowerMenu : MonoBehaviour {

    public int openFloor = 1;

    private List<GameObject> _menuItems;

    public List<GameObject> MenuItems {
        get {
            return _menuItems;
        }
    }

    void Start () {
        _menuItems = new List<GameObject>();

        foreach(Transform t in transform)
        {
            if(t.gameObject.name.Contains("level"))
            {
                t.gameObject.SetActive(false);
                _menuItems.Add(t.gameObject);
            }
        }
    }
    
    void Update () {
        
    }
}
