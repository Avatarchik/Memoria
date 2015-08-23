using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Memoria.Menu
{
    public class PopupWindow : MonoBehaviour {

        enum Contents
        {
            STORY = 0,
            TIPS = 1,
            AMELIA = 2,
            DHIEL = 3,
            ARIA = 4,
            ISKA = 5,
            DIANA = 6,
            NONE = 7
        }

        private Animator _animator;
        private GameObject _contents;
        public List<Sprite> spriteList = new List<Sprite>();

        void Start () {
            _animator = GetComponent<Animator>();
            _contents = GameObject.Find("content");
        }
    
        void Update () {
        
        }

        public void OpenWindow(bool open)
        {
            _animator.SetBool("popupWindow", open);
        }

        public void SetSprite(int id)
        {
            GetComponent<Image>().sprite = spriteList[id];
        }

        private void SetContent(Contents contents)
        {
            switch(contents)
            {
                case Contents.STORY:
                    break;
                case Contents.TIPS:
                    break;
                case Contents.AMELIA:
                    break; 
                case Contents.ARIA:
                    break;
                case Contents.DHIEL:
                    break;
                case Contents.ISKA:
                    break;
                case Contents.NONE:
                    break;
            }
        }
        public void ShowContent(int value)
        {
            bool visible = (value > 0) ? true : false;
            _contents.gameObject.SetActive(visible);
        }
    }
}