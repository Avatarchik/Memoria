using UnityEngine;
using System.Collections.Generic;

namespace Memoria.Menu
{
    public class TowerMenu : MonoBehaviour
    {

        public int openFloor = 1;

        private List<GameObject> _menuItems;

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
            SetVisible();
        }
    
        public void SetVisible()
        {
            for(int i = 0; i < openFloor; i++)
            {
                _menuItems[i].SetActive(true);
            }                
        }

        public void ClickButton(int floor)
        {
            PlayerPrefs.SetInt("floor", floor);
        }
    }
}