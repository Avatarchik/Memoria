using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockComponent.Utility
{
	public class BlockMover
	{
		private Block block;
		
		private Subject<Unit> onMoveBegin;
		private Subject<Unit> onMove;
		private Subject<Unit> onMoveEnd;

		public Subject<Unit> OnMoveBeginAsObservable()
		{			
			return onMoveBegin ?? (onMoveBegin = new Subject<Unit>());
		}

		public Subject<Unit> OnMoveAsObservable()
		{
			return onMove ?? (onMove = new Subject<Unit>());
		}

		public Subject<Unit> OnMoveEndAsObservable()
		{
			return onMoveEnd ?? (onMoveEnd = new Subject<Unit>());
		}

		public void Bind(Block block)
		{
			this.block = block;

			// 開始
			block.OnMouseDownAsObservable()
				.Where(_ => (DungeonManager.instance.activeState == DungeonState.None) && !block.putted)
				.Do(OnMoveBegin)
				.Do(SetPositionAtTapPosition)
				.Subscribe(_ =>
				{
					// 移動
					var onMove = block.OnMouseDragAsObservable()
						.Do(OnMove)
						.Subscribe(SetPositionAtTapPosition);
	
					// 終了
					block.OnMouseUpAsObservable()
						.First()
						.Do(__ => onMove.Dispose())
						.Subscribe(OnMoveEnd);
				});
		}

		private void SetPositionAtTapPosition(Unit unit)
		{
			var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			position.z = block.transform.position.z;
			block.transform.position = position;
		}

		private void OnMoveBegin(Unit unit)
		{
			if (onMoveBegin != null)
			{
				onMoveBegin.OnNext(unit);
			}
		}

		private void OnMove(Unit unit)
		{
			if (onMove != null)
			{
				onMove.OnNext(unit);
			}
		}

		private void OnMoveEnd(Unit unit)
		{
			if (onMoveEnd != null)
			{
				onMoveEnd.OnNext(unit);
			}
		}
	}
}