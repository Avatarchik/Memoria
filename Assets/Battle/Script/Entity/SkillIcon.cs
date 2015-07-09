using UnityEngine.UI;
using UnityEngine;
using System;

namespace Memoria.Battle.GameActors
{
    public class SkillIcon : UIElement {
        private Button _button;

        override public void Init()
        {
//            transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform , false);
            _button = GetComponent<Button>();
        }

        public void SetOnClick(Action<string> setAttack, string skill)
        {
            _button.onClick.AddListener(() => setAttack(skill));
        }
    }
}