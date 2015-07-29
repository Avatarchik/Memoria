using System;
using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.Items
{
    [Serializable]
    public enum ItemType
    {
        Key,
        Jewel,
        Soul,
        MagicPlate
    }

    [Serializable]
    public struct ItemData
    {
        [SerializeField]
        private ItemType _type;
        public ItemType type
        {
            get { return _type; }
            set { _type = value; }
        }

        [SerializeField]
        private BlockType _attribute;
        public BlockType attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        [SerializeField]
        private Vector2Int _location;
        public Vector2Int location
        {
            get { return _location; }
            set { _location = value; }
        }
    }

    public class Item : MonoBehaviour
    {
        private ItemData _itemData;
        public ItemData itemData
        {
            get { return _itemData; }
            set
            {
                _itemData = value;
                if (tag != "Key")
                {
                    SetSprite(_itemData.attribute);
                }
                
                transform.position = (Vector3)MapManager.instance.ToPosition(_itemData.location);
            }
        }

        [SerializeField]
        public Sprite thunderSprite;

        [SerializeField]
        public Sprite waterSprite;

        [SerializeField]
        public Sprite fireSprite;

        [SerializeField]
        public Sprite windSprite;

        [SerializeField]
        public Sprite recoverySprite;
        
        Subject<Unit> onTake;
        
        public IObservable<Unit> OnTakeAsObservable()
        {
            return onTake ?? (onTake = new Subject<Unit>());
        }
            
        private void OnTake()
        {
            if (onTake != null)
            {
                onTake.OnNext(Unit.Default);
            }
        }
        
        public void Take()
        {
            OnTake();
            Destroy(gameObject);  
        }
        
        private void SetSprite(BlockType attribute)
        {
            var renderer = GetComponent<SpriteRenderer>();

            switch (attribute)
            {
                case BlockType.Thunder:
                    renderer.sprite = thunderSprite;
                    break;

                case BlockType.Water:
                    renderer.sprite = waterSprite;
                    break;

                case BlockType.Fire:
                    renderer.sprite = fireSprite;
                    break;

                case BlockType.Wind:
                    renderer.sprite = windSprite;
                    break;

                case BlockType.Recovery:
                    renderer.sprite = recoverySprite;
                    break;
            }
        }
    }
}