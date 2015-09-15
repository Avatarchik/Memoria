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

    public enum StockType
    {
        FIRE = 0,
        WIND = 1,
        WATER = 2,
        THUNDER = 3
    }

    public class ElementalPowerStock : MonoBehaviour
    {
        private Sprite _sprite;
        private float offsetX = -0.8f;
        private float offsetY = -1.2f;
        private float spaceOffset = 0.8f;
        private int stockLength = 3;
        private int layerOffset = 10;
        private Type _imageType;
        private GameObject[] _stockObj;

        public int stock;
        public ObjectType objType;
        public StockType elementType;

        public bool Full;
        public float scale = 1.5f;


        public float OffsetX {
            get
            {
                return offsetX;
            }
        }
        public float OffsetY {
            get
            {
                return offsetY;
            }
        }
        public float SpaceOffset {
            get
            {
                return spaceOffset;
            }
        }
        public float LayerOffset {
            get
            {
                return layerOffset;
            }
        }
        public string SortingLayer
        {
            get;
            set;
        }
        

        public void Awake()
        {
            _stockObj = new GameObject[stockLength];
            for(int i = 0; i < _stockObj.Length; i++)
            {
                _stockObj[i] = new GameObject();
                _stockObj[i].transform.localScale *= scale;
            }
        }

        void Start()
        {
            Full = false;

            _sprite = Resources.Load<Sprite>("ElementalPowerStock/"+ elementType.ToString().ToLower() + "_power");

            _imageType = GetImageType(objType);

            var i = 0;
            foreach (GameObject obj in _stockObj)
            {
                obj.name = "PowerStock_" + i;
                obj.AddComponent(_imageType);
                obj.transform.SetParent(this.transform, false);
                switch(objType)
                {
                    case ObjectType.UI_OBJECT:
                        var img = obj.GetComponent<Image>();
                        img.sprite = _sprite;
                        break;
                    case ObjectType.NORMAL:
                        var sr = obj.GetComponent<SpriteRenderer>();
                        sr.sprite = _sprite;
                        sr.sortingOrder = 0;
                        sr.sortingLayerName = SortingLayer;
                        break;
                }

                var pos = obj.transform.position;
                obj.transform.position = new Vector3(pos.x + offsetX + (i * spaceOffset),
                                                     pos.y + offsetY,
                                                     transform.parent.position.z + layerOffset
                                                     );
                i++;
            }
        }

        void LateUpdate ()
        {
            UpdateStatus();
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

        public void UpdateStatus()
        {
            Full = (stock >= stockLength) ? true : false;
        }

        private Type GetImageType(ObjectType objType)
        {
            return (objType == 0) ? typeof(Image) : typeof(SpriteRenderer);
        }
    }
}