using UnityEngine;
using UniRx;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockComponent
{
    public class BlockFactor : MonoBehaviour
    {
        private static BlockManager blockManager { get { return BlockManager.instance; } }
        private Block block;

        public void CreateBlock(ShapeData shapeData, BlockType blockType)
        {
            block = blockManager.CreateBlock(this, shapeData, blockType);

            block.OnPutAsObservable()
                .Subscribe(_ =>
                {
                    ShapeData nextShapeData = blockManager.GetRandomShapeData();
                    BlockType nextBlockType = blockManager.GetRandomBlockType(type => type != BlockType.None);
                    CreateBlock(nextShapeData, nextBlockType);
                });
        }

        public void SetBlock(ShapeData shapeData, BlockType blockType)
        {
            block.shapeData = shapeData;
            block.blockType = blockType;
        }
    }
}