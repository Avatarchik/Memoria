using UnityEngine;
using System.Collections;
using Memoria.Dungeon.BlockComponent;

namespace Memoria.Dungeon.Items
{
    [SerializeField]
    public struct ItemData
    {
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