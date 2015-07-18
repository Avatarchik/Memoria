using UnityEngine;
using System.Collections;
using Memoria.Dungeon.BlockComponent;

namespace Memoria.Dungeon.Items
{
    [SerializeField]
    public struct ItemData
    {
        public Vector2Int location;
        public BlockType attribute;
    }
	
    public class Item : MonoBehaviour
    {
        public ItemData data;

		[SerializeField]
        public Sprite thunderSprite;

        [SerializeField]
        public Sprite waterSprite;

        [SerializeField]
        public Sprite fireSprite;

        [SerializeField]
        public Sprite windSprite;

        // Use this for initialization
        void Start()
        {
            if (tag != "Key")
            {
                SetSprite(data.attribute);
            }
        }

        //  // Update is called once per frame
        //  void Update()
        //  {

        //  }

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
            }
        }
    }
}