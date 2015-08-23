using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Memoria.Battle.GameActors
{
    public class CutIn : UIElement
    {
        private Vector3 pos;
        private Vector3 start = new Vector3(ScreenWidth, 0, 0);
        private Animator _animator;

        public List<Sprite> spriteList = new List<Sprite>();
        public int id { get; set; }
        public bool played = true;
        

        void Start()
        {
//            pos = start;
            _animator = GetComponent<Animator>();
        }

        override public void Init()
        {
            pos = start;
            played = false;
            GetComponent<Image>().sprite = spriteList[id];
        }


        public void StartSequence()
        {
            _animator.SetBool("enter", true);
        }

        public IEnumerator StopSequence()
        {
            yield return new WaitForSeconds(1.0f);

            _animator.SetBool("enter", false);
            pos = start;
            transform.position = pos;
            played = true;
        }
    }
}