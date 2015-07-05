using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Memoria.Battle.GameActors
{
    abstract public class UIElement : MonoBehaviour
    {

        public string spriteResource;

        abstract public void Init();

        public void SetParent()
        {
            this.transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform, false);
        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}