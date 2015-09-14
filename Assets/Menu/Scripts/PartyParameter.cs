using UnityEngine;
using System.Collections;

namespace Memoria.Menu
{
    public class PartyParameter : MonoBehaviour
    {
        public int value;
        ParamLabel label;
        void Start ()
        {
            label = GetComponent<ParamLabel>();
            label.GenerateLabel(value, this.transform.position, 1.0f);
        }    
    }
}