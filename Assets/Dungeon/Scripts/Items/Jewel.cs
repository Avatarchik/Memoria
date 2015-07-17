using UnityEngine;
using Memoria.Dungeon.BlockComponent;

namespace Memoria.Dungeon.Items
{
    public struct JewelData
    {
        public BlockType attribute { get; set; }
        public Vector2Int location { get; set; }
    }

    public class Jewel : MonoBehaviour
    {
        [SerializeField]
        private Sprite thunderSprite;

        [SerializeField]
        private Sprite waterSprite;

        [SerializeField]
        private Sprite fireSprite;

        [SerializeField]
        private Sprite windSprite;

        private JewelData _jewelData;
        public JewelData jewelData
        {
            get { return _jewelData; }
            set
            {
                _jewelData = value;
                SetSprite(_jewelData.attribute);
            }
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
            }

        }
    }
}