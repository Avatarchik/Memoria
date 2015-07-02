﻿using UnityEngine;
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
			int shapeType = blockManager.GetRandomShapeType();
			BlockType blockType;

			do
			{
				blockType = blockManager.GetRandomBlockType();
			}
			while (blockType == BlockType.None);

			CreateBlock(shapeType, blockType);
		}

		public void CreateBlock(int shapeType = 0, BlockType type = BlockType.None)
		{
			block = blockManager.CreateBlock(this, shapeType, type);
		}

		public void SetBlock(int shapeType = 0, BlockType type = BlockType.None)
		{
			block.shapeType = shapeType;
			block.type = type;
		}
	}
}