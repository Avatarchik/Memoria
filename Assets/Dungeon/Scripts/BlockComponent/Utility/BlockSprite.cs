using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockComponent.Utility
{
    public class BlockSprite
    {
        public void Bind(Block block)
        {
            var animator = block.GetComponent<Animator>();
            var image = block.GetComponent<Image>();
            var renderer = block.GetComponent<SpriteRenderer>();

            var onMoveBegin = block.OnMoveBeginAsObservable()
                .Subscribe(_ => animator.SetBool("isSpriteRenderer", true));

            var onBack = block.OnBackAsObservable()
                .Subscribe(_ => animator.SetBool("isSpriteRenderer", false));

            block.OnPutAsObservable()
                .Subscribe(_ =>
                {
                    animator.SetBool("isSpriteRenderer", true);
                    renderer.sortingOrder = 0;

                    onMoveBegin.Dispose();
                    onBack.Dispose();
                });

            block.ObserveEveryValueChanged(_ => block.shapeData.typeCode)
                .Select(_ => GetSprite(block.shapeData, block.blockType))
                .Subscribe(sprite =>
                {
                    image.sprite = sprite;
                    renderer.sprite = sprite;
                });

            block.ObserveEveryValueChanged(_ => block.blockType)
                .Select(_ => GetSprite(block.shapeData, block.blockType))
                .Subscribe(sprite =>
                {
                    image.sprite = sprite;
                    renderer.sprite = sprite;
                });
        }

        private Sprite GetSprite(ShapeData shapeData, BlockType blockType)
        {
            return BlockManager.instance.GetBlockSprite(shapeData, blockType);
        }
    }
}