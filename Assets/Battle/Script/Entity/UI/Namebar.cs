using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Memoria.Battle.GameActors
{
    public class Namebar : UIElement
    {
        Vector3 pos;

        public float X
        {
            get
            {
                return  -8.0f;
            }
        }
        public float Y
        {
            get
            {
                return 0.5f;
            }
        }
        override public void Init()
        {

        }

        void Update()
        {
        }


        public void FallDown(Vector3 slot)
        {

            StartCoroutine(MoveDown(slot));
        }

        public void CurvedMove(Vector3 slot)
        {
            StartCoroutine(CurveUp(slot));
        }

        private IEnumerator MoveDown(Vector3 slot)
        {
            yield return new WaitForSeconds(0.2f);

            Vector3 newPos = transform.position;
            while(this.transform.position.y != slot.y)
            {
                newPos.y -= 0.1f;
                if(newPos.y < slot.y)
                {
                    newPos.y = slot.y;
                }

                transform.position = newPos;
                yield return null;
            }
        }

        private IEnumerator CurveUp(Vector3 slot)
        {
            yield return new WaitForSeconds(0.0f);

            Vector3 thisPos = transform.position;

            float startX = thisPos.x;

            float fromY = thisPos.y;
            float toY = slot.y;

            float yDiff = fromY - toY;
            
//            float angle = (Mathf.Atan2(midpoint.y - thisPos.y, midpoint.x - thisPos.x) * 180 / Mathf.PI);

            while(this.transform.position.y != slot.y)
            {
                thisPos.y += 0.1f;
                thisPos.x = startX - Mathf.Sin(Mathf.PI * ((thisPos.y - fromY )/ yDiff));

                //Assure coordinates does not go to far
                if(thisPos.y > slot.y)
                {
                    thisPos.y = slot.y;
                    thisPos.x = slot.x;
                }
                transform.position = thisPos;
                yield return null;
            }
        }

        public Vector3 Lerp(Vector3 start, Vector3 end, float p)
        {
            return (start + p * (end - start));
        }
    }
}