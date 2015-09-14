using UnityEngine;
using UnityEngine.UI;
//using Memoria.Battle;

namespace Memoria.Menu
{
    public class ParamNumber : ParamLabel
    {

        [SerializeField]
        private Sprite[] _numberSprites;

        override public void Init(int i)
        {
            Image img = GetComponent<Image>();
            img.sprite = _numberSprites[i];
        }

        override public void Unload()
        {
            var paramNumbers = GetComponentsInChildren<ParamNumber>();
            foreach(var go in paramNumbers)
            {
                Destroy(go.transform.gameObject);
            }
        }
    }
}