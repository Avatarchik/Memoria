using UnityEngine;
using UnityEngine.UI;

namespace Memoria.Battle.GameActors
{
    public class DescriptionFrame : UIElement
    {
        override public void Init()
        {
            spriteFolder = "Skills/";
            transform.position = new Vector3(-0.0f, 4.25f, 1);
            GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteFolder + spriteResource);

        }
    }
}