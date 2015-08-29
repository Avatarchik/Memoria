using UnityEngine;
using UnityEngine.UI;

namespace Memoria.Menu
{
    public class TowerButton : MonoBehaviour {

        private Button _towerButton;
        private TowerMenu _towerMenu;
        private bool _menuActive;

        void Start () {

            _towerMenu = GetComponentInParent<TowerMenu>();
        }
        
        public void SetMenuVisible()
        {
            for(int i = 0; i < _towerMenu.openFloor; i++)
            {
                _towerMenu.MenuItems[i].SetActive(_menuActive);
            }
            _menuActive = (_menuActive) ? false : true;
                
        }

        public void ClickButton(int floor)

        {
            PlayerPrefs.SetInt("floor", floor);
        }
    }
}