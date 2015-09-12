using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Memoria.Battle.GameActors;




namespace Memoria.Menu
{
    public class PopupWindow : MonoBehaviour {

        public enum Contents
        {
            STORY = 0,
            TIPS = 1,
            AMELIA = 2,
            DHIEL = 3,
            RIZEL = 4,
            ISKA = 5,
            DIANA = 6,
            NONE = 7
        }

        private Animator _animator;
        private GameObject _contents;
        private ParamContent _paramContent;
        public List<Sprite> spriteList = new List<Sprite>();

        void Start () {
            _animator = GetComponent<Animator>();
            _paramContent = GetComponentInChildren<ParamContent>();
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

        public void SetContent(int contentInt)
        {

            Contents contents = (Contents)contentInt;

            switch(contents)
            {
                case Contents.STORY:
//                    paramContent.SetStory();
                    break;
                case Contents.TIPS:
//                    paramContent.SetTips();
                    break;
                case Contents.AMELIA:
                    _paramContent.SetProfile(typeof(Amelia));
                    break;
                case Contents.RIZEL:
                    _paramContent.SetProfile(typeof(Rizel));
                    break;
                case Contents.DHIEL:
                    _paramContent.SetProfile(typeof(Dhiel));
                    break;
                case Contents.ISKA:
                    _paramContent.SetProfile(typeof(Iska));
                    break;
                case Contents.NONE:
                    break;
            }
        }
        public void ShowContent(int value)
        {
            bool visible = (value > 0) ? true : false;
            _paramContent.gameObject.SetActive(visible);
        }
    }
}