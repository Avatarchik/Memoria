using UnityEngine.UI;
using UnityEngine;
using System;

namespace Memoria.Battle.GameActors
{
    public class SkillIcon : UIElement
    {
        private Button _button;

        override public void Init()
        {
            _button = GetComponent<Button>();
            spriteFolder = "Skills/";
            GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteFolder + spriteResource);
        }

        public void SetOnClick(Action<string> setAttack, string skill)
        {
            _button.onClick.AddListener(() => setAttack(skill));
        }
    }
}