using UnityEngine;
using UnityEngine.UI;
using Memoria.Battle.Managers;
using System.Collections;


namespace Memoria.Battle.GameActors
{
    public class CancelButton : MonoBehaviour
    {
        public bool Visible { get; set; }

        Image img;
        void Start ()
        {
            img = GetComponent<Image>();
            Visible = false;
        }
        void Update ()
        {
            if(Visible)
            {
                img.enabled = true;
            }
            else
            {
                img.enabled = false;
            }
        }

        void CancelSkill()
        {
        }
    }
}