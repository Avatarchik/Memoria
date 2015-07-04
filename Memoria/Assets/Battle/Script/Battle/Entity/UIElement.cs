using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Memoria.Battle.GameActors
{
    Xabstract public class UIElement : MonoBehaviour
    {
        protected ActorSpawner spawner;

        public string spriteResource;

        abstract public void Init();

        public void SetParent()
        {
            this.transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform, false);
        }

        public void SetPosition(Vector2 pos) 
        {
            this.transform.position = pos;
        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}