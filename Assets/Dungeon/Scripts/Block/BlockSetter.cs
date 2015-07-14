using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockUtility
{
	public class BlockSetter : MonoBehaviour
	{		
		public bool putted { get; set; }

		private MapManager mapManager;
		private Block block;

		private Subject<Unit> onPutBlock;
		private Subject<Unit> onBackBlock;

		public IObservable<Unit> OnPutBlockAsObservable()
		{
			return onPutBlock ?? (onPutBlock = new Subject<Unit>());
		}

		public IObservable<Unit> OnBackBlockAsObservable()
		{
			return onBackBlock ?? (onBackBlock = new Subject<Unit>());
		}

		// Use this for initialization
		void Start()
		{
			mapManager = DungeonManager.instance.mapManager;
			block = GetComponent<Block>();
			
			var mover = GetComponent<BlockMover>();
			mover.OnEndDragAndDropAsObservable()
			.Subscribe(_ =>
			{
				if (CanPut())
				{
					Put();
				}
				else
				{
					Back();
				}
			});
		}

		public bool CanPut()
		{
			// 範囲外チェック
			if (!mapManager.canPutBlockArea.Contains(transform.position))
			{
				return false;
			}

			// 置くところのブロックチェック
			if (mapManager.map.ContainsKey(block.location))
			{
				return false;
			}

			// 隣接ブロックのチェック
			return new []
			{
				Vector2Int.left,
				Vector2Int.right,
				Vector2Int.down,
				Vector2Int.up,
			}
				.Any(dir => Connected(dir));
		}

		// 指定した向きの道とつながるかどうか
		public bool Connected(Vector2Int checkBaseDirection)
		{	
			if (!block.shapeData.Opend(checkBaseDirection))
			{
				return false;
			}

			Vector2Int checkLocation = block.location + checkBaseDirection;
			if (!mapManager.map.ContainsKey(checkLocation))
			{
				return false;
			}

			Block checkBlock = mapManager.map[checkLocation];
			return checkBlock.shapeData.Opend(-checkBaseDirection);
		}

		public void Put()
		{
			putted = true;
//			mapManager.map[block.location] = block;
			onPutBlock.OnNext(Unit.Default);
		}

		private void Back()
		{
			onBackBlock.OnNext(Unit.Default);
		}
	}
}