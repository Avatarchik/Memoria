using UnityEngine;
using UnityEngine.UI;
using System.Collections;


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

        public void SetColor(Color color)
        {
            img.color = color;
        }
        public void FallDown(float pos)
        {
            StartCoroutine(FallTo(pos));
        }
        public void FoldOut(float pos)
        {
            StartCoroutine(FoldTo(pos));
        }

        private IEnumerator FoldTo(float offsetPos)
        {
            var newPos = Position;
            while(Position.x < offsetPos)
            {
                newPos.x += 15;
                if(newPos.x > offsetPos)
                    newPos.x = offsetPos;                
                Position = newPos;
                yield return null;
            }

        }
        private IEnumerator FallTo(float offsetPos)
        {
            var newPos = Position;
            while(Position.y > offsetPos)
            {
                newPos.y -= 5f;
                if(newPos.y < offsetPos)
                    newPos.y = offsetPos;
                Position = newPos;
                yield return null;
            }
        }
    }
}
