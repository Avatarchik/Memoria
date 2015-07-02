using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.Managers;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon.BlockUtility
{
	public class BlockList : MonoBehaviour
	{
		public List<BlockFactor> blockFactors = new List<BlockFactor>();

		private DungeonManager dungeonManager;

		private BlockManager blockManager;

		private ParameterManager paramaterManager;

		private bool[] flags;

		[SerializeField]
		private Button randomizeButton;

		// Use this for initialization
		private void Start()
		{
			dungeonManager = DungeonManager.instance;
			blockManager = dungeonManager.blockManager;
			paramaterManager = dungeonManager.parameterManager;

			CreateBlockList();

			// ランダマイズの登録
			randomizeButton.OnClickAsObservable()
			.Where(_ => dungeonManager.activeState == DungeonState.None)
			.Subscribe(RandomizeBlockList);
		}

		private void CreateBlockList()
		{
			flags = new bool[blockManager.NumberOfBlockShapeType];

			blockFactors.ForEach(blockFactor =>
			{
				int randomShapeType = GetRandomShapeType(shapeType => !flags[shapeType]);
				BlockType randomBlockType = GetRandomBlockType(blockType => blockType != BlockType.None);

				blockFactor.CreateBlock(randomShapeType, randomBlockType);

				flags[randomShapeType] = true;
			});
		}

		public void RandomizeBlockList(Unit _ = null)
		{
			bool[] nextFlags = new bool[blockManager.NumberOfBlockShapeType];

			blockFactors.ForEach(blockFactor =>
			{
				int randomShapeType = GetRandomShapeType(shapeType => !flags[shapeType] && !nextFlags[shapeType]);
				BlockType randomBlockType = GetRandomBlockType(blockType => blockType != BlockType.None);

				blockFactor.SetBlock(randomShapeType, randomBlockType);

				nextFlags[randomShapeType] = true;
			});

			paramaterManager.parameter.sp -= 2;
			flags = nextFlags;
		}

		private int GetRandomShapeType(Predicate<int> selector)
		{
			int randomShapeType;
			do
			{
				randomShapeType = blockManager.GetRandomBlockShapeType();
			}
			while(!selector(randomShapeType));

			return randomShapeType;
		}

		private BlockType GetRandomBlockType(Predicate<BlockType> selector)
		{
			BlockType randomBlockType;
			do
			{
				randomBlockType = blockManager.GetRandomBlockType();
			}
			while (!selector(randomBlockType));

			return randomBlockType;
		}
	}
}