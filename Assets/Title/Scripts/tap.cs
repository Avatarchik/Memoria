using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Memoria.Title
{
    public class tap : MonoBehaviour
    {
        [SerializeField]
        private float scrollSpeed = 0.05f;

        [SerializeField]
        private float yMin = -13;
        [SerializeField]
        private float yMax = 13;

        Vector3 mp;
        Vector3 mp2;
        int c = 0;
        Vector2 Position;

        void Start()
        {
            Position = transform.position;
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (c % 2 == 0)
                {
                    mp = Input.mousePosition;
                    if (mp.y - mp2.y < 0)
                    {
                        if (Position.y > yMin)
                        {
                            Position.y -= scrollSpeed;
                        }
                    }
                    if (mp.y - mp2.y > 0)
                    {
                        if (Position.y < yMax)
                        {
                            Position.y += scrollSpeed;
                        }
                    }
                }
                if (c % 2 == 1)
                {
                    mp2 = Input.mousePosition;
                    if (mp2.y - mp.y < 0)
                    {
                        if (Position.y > yMin)
                        {
                            Position.y -= scrollSpeed;
                        }
                    }
                    if (mp2.y - mp.y > 0)
                    {
                        if (Position.y < yMax)
                        {
                            Position.y += scrollSpeed;
                        }
                    }
                }
            }
            
            c = c + 1;
            transform.position = Position;
        }
    }
}