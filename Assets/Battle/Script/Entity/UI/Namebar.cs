using UnityEngine;

namespace Memoria.Battle.GameActors
{
    public class Namebar : UIElement
    {
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

        public void CurvedMove(Vector3[] param)
        {
        }
    }
}