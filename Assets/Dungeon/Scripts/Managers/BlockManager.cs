using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.BlockUtility;

namespace Memoria.Dungeon.Managers
{
	public class BlockManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject blockPrefab;

		public readonly int NumberOfBlockShapeType = 11;
		public readonly int NumberOfBlockType = 6;

		[SerializeField]
		private BlockSprites _blockSprites = new BlockSprites();

		public Sprite[][] blockSprites { get { return _blockSprites.blockSprites; } }

		public Block CreateBlock(BlockFactor blockFactor, int shapeType = 0, BlockType type = BlockType.None, bool isDefault = false, Vector2Int location = default(Vector2Int))
		{
			GameObject blockObject = Instantiate<GameObject>(blockPrefab);
			Block block = blockObject.GetComponent<Block>();
			block.Initialize();
			block.blockFactor = blockFactor;

			BlockShape shape = new BlockShape();
			shape.type = shapeType;

			if (isDefault)
			{
				block.SetAsDefault(location, shape, type);
			}
			else
			{
				block.shapeType = shapeType;
				block.type = type;
				block.location = location;
			}

			return block;
		}

		public Block CreateBlock(BlockFactor blockFactor, BlockData blockData, bool isDefault = false)
		{
			int shapeType = blockData.shape.type;
			return CreateBlock(blockFactor, shapeType, blockData.type, isDefault, blockData.location);
		}

		public int GetRandomBlockShapeType()
		{
			int min = 0;
			int max = NumberOfBlockShapeType;
			return Random.Range(min, max);
		}

		public BlockType GetRandomBlockType()
		{
			int min = 0;
			int max = NumberOfBlockType;
			return (BlockType)Random.Range(min, max);
		}

		public Sprite GetBlockSprite(BlockShape shape, BlockType type)
		{
			return blockSprites[(int)type][shape.type];
		}
	}
}