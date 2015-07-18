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
//            StartCoroutine(DetatchedMove(slot));

        }

        private IEnumerator MoveDown(Vector3 slot)
        {
            yield return new WaitForSeconds(0.1f);

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
            yield return new WaitForSeconds(0.1f);

            Vector3 thisPos = transform.position;

            float startX = thisPos.x;

            float fromY = thisPos.y;
            float toY = slot.y;

            float yDiff = fromY - toY;
            
            while(this.transform.position.y != slot.y)
            {
                thisPos.y += 0.1f;
                thisPos.x = startX - Mathf.Sin(Mathf.PI * ((thisPos.y - fromY) / yDiff));

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

        public void Attach(Vector3 endPos)
        {
            var pos = transform.position;
            pos.x = endPos.x;
            pos.y = endPos.y;
            transform.position = pos;
        }

        public void Detach(float x, float y)
        {
            var pos = transform.position;
            pos.x += x;
            pos.y += y;
            transform.position = pos;
        }


        public IEnumerator DetatchedMove(Vector3 endPos)
        {
            yield return new WaitForSeconds(0.1f);
            float xOffset = -0.3f;
            float yOffset = -0.3f;


            Detach(xOffset, yOffset);
            Vector3 thisPos = transform.position;

            while(transform.position.y < endPos.y + yOffset)
            {
                thisPos.y += 0.1f;
                transform.position = thisPos;
                if(thisPos.y > endPos.y + yOffset)
                {
                    Attach(endPos);
                    break;
                }
                yield return null;
            }
        }

    }
}