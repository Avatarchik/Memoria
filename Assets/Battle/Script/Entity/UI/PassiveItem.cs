using UnityEngine;
using UnityEngine.UI;

namespace Memoria.Battle.GameActors
{
    public class PassiveItem : UIElement
    {
        override public void Init()
        {
            spriteFolder = "UI";
            image = GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>(spriteFolder +"/"+ spriteResource);
        }
    }
}