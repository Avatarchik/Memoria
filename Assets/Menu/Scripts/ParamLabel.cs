using UnityEngine;
using System;
using Memoria.Battle;

namespace Memoria.Menu
{
    public class ParamLabel : MonoBehaviour
    {
        [SerializeField]
        public GameObject numberLabel;

        virtual public void Unload(){}
        virtual public void Init(int i){}

        public void GenerateLabel(int value, Vector3 pos, float scale  = 0.5f)
        {
            float offset = 60 * scale;
            var splitValue = value.ToArray();
            for(int i = 0; i < splitValue.Length; i++)
            {
                var number = Instantiate(numberLabel).GetComponent<ParamNumber>();
                number.Init(splitValue[i]);
                number.transform.position = new Vector3(pos.x, pos.y, pos.z);
                number.transform.SetParent(this.transform, false);
                var localPos = number.transform.localPosition;
                number.transform.localPosition = new Vector3(localPos.x + (i * offset), localPos.y, localPos.z);
                number.transform.localScale *= scale;
            }
        }
    }
}