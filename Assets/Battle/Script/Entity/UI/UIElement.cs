using UnityEngine;

namespace Memoria.Battle.GameActors
{
    abstract public class UIElement : MonoBehaviour
    {
        public string spriteResource;

        public bool moveable { get; private set; }

        abstract public void Init();

        public void ParentToUI()
        {
            transform.SetParent(GameObject.FindObjectOfType<Canvas>().gameObject.transform , false);
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}