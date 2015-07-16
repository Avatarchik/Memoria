using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Memoria.Battle.GameActors
{
    public class DamageNumber : UIElement
    {
        public Image img;

        override public void Init()
        {
            img = GetComponent<Image>();
            img.sprite = (Sprite)Resources.Load<Sprite>(spriteResource);
        }

    }

}