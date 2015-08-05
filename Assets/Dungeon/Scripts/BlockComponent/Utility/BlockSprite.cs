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

            // 操作し始めたとき
            // SpriteRenderer に切り替える
            var onMoveBegin = block.OnMoveBeginAsObservable()
                .Subscribe(_ => animator.SetBool("isSpriteRenderer", true));

            // 操作を終えるとき
            // Image に切り替える
            var onBack = block.OnBackAsObservable()
                .Subscribe(_ => animator.SetBool("isSpriteRenderer", false));

            // 設置されたとき
            // SpriteRendererに切り替える
            // 解放処理
            block.OnPutAsObservable()
                .Subscribe(_ =>
                {
                    animator.SetBool("isSpriteRenderer", true);
                    renderer.sortingOrder = 0;

                    onMoveBegin.Dispose();
                    onBack.Dispose();
                });

            // ブロックの形状が変化した時
            // Sprite を更新する
            block.ObserveEveryValueChanged(_ => block.shapeData.typeCode)
                .Select(_ => GetSprite(block.shapeData, block.blockType))
                .Subscribe(sprite =>
                {
                    image.sprite = sprite;
                    renderer.sprite = sprite;
                });

            // ブロックの属性が変化した時
            // Sprite を更新する
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