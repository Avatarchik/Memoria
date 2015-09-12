using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Memoria.Battle.GameActors;

namespace Memoria.Menu
{
    public class PopupWindow : MonoBehaviour
    {

        public enum Contents
        {
            STORY = 0,
            TIPS = 1,
            AMELIA = 2,
            DHIEL = 3,
            RIZEL = 4,
            ISKA = 5,
            NONE = 6
        }

        private Animator _animator;
        private ParamContent _paramContent;
        private bool _story;

        GameObject parent;
        Contents _contents;

        public List<Sprite> spriteList = new List<Sprite>();

        void Start ()
        {
            _animator = GetComponent<Animator>();
            _paramContent = GetComponentInChildren<ParamContent>();
            _contents = Contents.NONE;
            parent = this.transform.parent.gameObject;
        }

        void LateUpdate ()
        {
            if(Input.GetMouseButtonDown(0) && _story) {
                var c = GameObject.FindObjectOfType<MainCharacter>();
                c.Switch();
                foreach(Image obj in parent.GetComponentsInChildren<Image>())
                {
                    if(!obj.transform.gameObject.name.Equals("parameterbox"))
                    {
                        StartCoroutine(FadeTo(obj, Color.white));
                    }
                }
                foreach(Button obj in parent.GetComponentsInChildren<Button>())
                {
                    obj.enabled = true;
                }
                OpenWindow(false);
                _story = false;
            }
        }

        public void OpenWindow(bool open)
        {
            UnloadWindow();
            _animator.SetBool("popupWindow", open);
        }

        public void UnloadWindow()
        {
            var labels = GetComponentsInChildren<ParamLabel>();
            for(int i = 0; i < labels.Length; i++)
            {
                labels[i].Unload();
            }
        }

        public void SetSprite(int id)
        {
            GetComponent<Image>().sprite = spriteList[id];
        }

        public void SetContent(int contentInt)
        {
            Contents contents = (Contents)contentInt;
            _contents = contents;
            switch(contents)
            {
                case Contents.STORY:
                    foreach(Image obj in parent.GetComponentsInChildren<Image>())
                    {
                        if(!obj.transform.gameObject.name.Equals("parameterbox"))
                        {
                            StartCoroutine(FadeTo(obj, Color.gray));
                        }
                    }
                    foreach(Button obj in parent.GetComponentsInChildren<Button>())
                    {
                        obj.enabled = false;
                    }
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
                    print("test");
                    _paramContent.SetProfile(typeof(Iska));
                    break;
                case Contents.NONE:
                    break;
            }
        }

        public void SetStory(int i)
        {
            if(_contents == Contents.STORY)
                _story = (i > 0) ? true : false;
        }

        private IEnumerator FadeTo(Image img, Color color)
        {
            float time = 0;
            float end = 0.5f;
            while(time < end)
            {
                img.color = Color.Lerp(img.color, color, time / end);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}