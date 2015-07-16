using UnityEngine;
using UniRx;
using Memoria.Dungeon.BlockComponent;

namespace Memoria.Dungeon.Managers
{
    public class BlockManager : MonoBehaviour
    {
        public static BlockManager instance { get { return DungeonManager.instance.blockManager; } }

        [SerializeField]
        private GameObject blockPrefab;

        public readonly int NumberOfBlockShapeType = 11;
        public readonly int NumberOfBlockType = 6;

        [SerializeField]
        private BlockList blockList;

        [SerializeField]
        private BlockSprites _blockSprites = new BlockSprites();

        public Sprite[][] blockSprites { get { return _blockSprites.blockSprites; } }

        private Subject<Block> onCreateBlock;

        public IObservable<Block> OnCreateBlockAsObservable()
        {
            return onCreateBlock ?? (onCreateBlock = new Subject<Block>());
        }

        public IObservable<Unit> OnRandomizeAsObservable()
        {
            return blockList.OnRandomizeAsObservable();
        }

        public Block CreateBlock()
        {
            Block block = Instantiate<GameObject>(blockPrefab).GetComponent<Block>();
            block.Initialize();

            if (onCreateBlock != null)
            {
                onCreateBlock.OnNext(block);
            }

            return block;
        }

        public Block CreateBlock(BlockFactor blockFactor, ShapeData shapeData, BlockType blockType)
        {
            Block block = CreateBlock();
            block.blockFactor = blockFactor;
            block.shapeData = shapeData;
            block.blockType = blockType;
            block.transform.SetParent(blockFactor.transform);
            block.transform.localPosition = Vector3.zero;
            return block;
        }

        public Block CreateBlockAsDefault(Vector2Int location, ShapeData shapeData, BlockType blockType)
        {
            Block block = CreateBlock();
            block.SetAsDefault(location, shapeData, blockType);
            return block;
        }

        public Block CreateBlockAsDefault(BlockData blockData)
        {
            return CreateBlockAsDefault(blockData.location, blockData.shapeData, blockData.blockType);
        }

        private T GetRandomType<T>(System.Func<T> getRandomType, System.Predicate<T> selector = null)
        {
            if (selector == null)
            {
                selector = _ => true;
            }

            T result;
            do
            {
                result = getRandomType();
            }
            while (!selector(result));

            return result;
        }

        public ShapeData GetRandomShapeData(System.Predicate<int> selector = null)
        {
            int min = 0;
            int max = NumberOfBlockShapeType;
            return new ShapeData(GetRandomType(() => Random.Range(min, max), selector));
        }

        public BlockType GetRandomBlockType(System.Predicate<BlockType> selector = null)
        {
            int min = 0;
            int max = NumberOfBlockType;
            return GetRandomType(() => (BlockType)Random.Range(min, max), selector);
        }

        public Sprite GetBlockSprite(ShapeData shape, BlockType type)
        {
            return blockSprites[(int)type][shape.typeID];
        }
    }
}