using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockUtility
{
	public class BlockFactor : MonoBehaviour
	{
		public Block block { get; set; }

		private BlockManager blockManager;

		void Awake()
		{
			blockManager = DungeonManager.instance.blockManager;
		}

		public void OnPutBlock()
		{
			ShapeData shapeData = blockManager.GetRandomShapeData();
			BlockType blockType;

			do
			{
				blockType = blockManager.GetRandomBlockType();
			}
			while (blockType == BlockType.None);

			CreateBlock(shapeData, blockType);
		}

		public void CreateBlock(ShapeData shapeData, BlockType blockType)
		{
			block = blockManager.CreateBlock(this, shapeData, blockType);
		}

		public void SetBlock(ShapeData shapeData, BlockType blockType)
		{
			block.shapeData = shapeData;
			block.blockType = blockType;
		}
	}
}