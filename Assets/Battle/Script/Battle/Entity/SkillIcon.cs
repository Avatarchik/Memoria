using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Memoria.Battle.GameActors
{
    public class SkillIcon : UIElement {
        private Button _button;


        override public void Init()
        {
            _button = GetComponent<Button>();
        }

        public void SetOnClick(Action<string> setAttack, string skill)
        {
            _button.onClick.AddListener(() => setAttack(skill));
        }
    }
}