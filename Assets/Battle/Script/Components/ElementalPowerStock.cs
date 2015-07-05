using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Memoria.Battle.Managers;


namespace Memoria.Battle.GameActors
{
    public enum ObjectType
    {
        UI_OBJECT = 0,
        NORMAL = 1
    }

    public enum Element
    {
        FIRE,
        WIND,
        WATER,
        THUNDER
    }

    public class ElementalPowerStock : MonoBehaviour
    {
        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private int stockLength = 3;

        [SerializeField]
        private float offsetX = -0.8f;

        [SerializeField]
        private float offsetY = -0.8f;

        [SerializeField]
        private float spaceOffset = 0.8f;

        public int stock;
        public ObjectType objType;
        public Element elementType;

        public bool Full { get; private set; }

        private Type _imageType;
        private GameObject[] _stockObj;

        public void Awake()
        {
            _stockObj = new GameObject[stockLength];

        }

        void Start()
        {
            Full = false;

            _sprite = Resources.Load<Sprite>(elementType.ToString().ToLower() + "_power");

            _imageType = GetImageType(objType);

            for(int i = 0; i < _stockObj.Length; i++)
            {
                _stockObj[i] = new GameObject();
                _stockObj[i].name = "PowerStock_" + i;
                _stockObj[i].AddComponent(_imageType);
                _stockObj[i].transform.SetParent(this.transform, false);

                switch(objType)
                {
                    case ObjectType.UI_OBJECT:
                        _stockObj[i].GetComponent<Image>().sprite = _sprite;
//                        _stockObj[i].GetComponent<Image>().enabled = false;
                        break;
                    case ObjectType.NORMAL:
                        _stockObj[i].GetComponent<SpriteRenderer>().sprite = _sprite;
//                        _stockObj[i].GetComponent<SpriteRenderer>().enabled = false;
                        break;
                }

                var pos = _stockObj[i].transform.position;
                _stockObj[i].transform.position = new Vector3(pos.x + offsetX + (i * spaceOffset), pos.y + offsetY);
            }
        }

        void Update ()
        {
            Full = (stock >= stockLength) ? true : false;

            for(int i = 0; i < stock; i++)
            {
                switch(objType)
                {
                    case ObjectType.UI_OBJECT:
                        _stockObj[i].GetComponent<Image>().enabled = true;
                        break;
                    case ObjectType.NORMAL:
                        _stockObj[i].GetComponent<SpriteRenderer>().enabled = true;
                        break;
                }
            }

            for(int i = stockLength - 1; i >= stock; i--)
            {
                switch(objType)
                {
                    case ObjectType.UI_OBJECT:
                        _stockObj[i].GetComponent<Image>().enabled = false;
                        break;
                    case ObjectType.NORMAL:
                        _stockObj[i].GetComponent<SpriteRenderer>().enabled = false;
                        break;
                }
            }
        }

        public void AddStock(int value = 1)
        {
            if(stock < stockLength)
            {
                stock += value;
            }
        }

        public void UseStock(int value)
        {
            stock -= value;
            if(stock - value < 0)
            {
                stock = 0;
            }
        }

        private Type GetImageType(ObjectType objType)
        {
            return (objType == 0) ? typeof(Image) : typeof(SpriteRenderer);
        }
    }
}