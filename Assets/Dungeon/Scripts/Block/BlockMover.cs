using UnityEngine;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockUtility
{
	public class BlockMover : MonoBehaviour
	{
		private Subject<Unit> onBeginDragAndDrop;
		private Subject<Unit> onDragAndDrop;
		private Subject<Unit> onEndDragAndDrop;

		//		public Func<Vector3, Vector3> SelectPosition { get; set; }

		public Subject<Unit> OnBeginDragAndDropAsObservable()
		{			
			return onBeginDragAndDrop ?? (onBeginDragAndDrop = new Subject<Unit>());
		}

		public Subject<Unit> OnDragAndDropAsObservable()
		{
			return onDragAndDrop ?? (onDragAndDrop = new Subject<Unit>());
		}

		public Subject<Unit> OnEndDragAndDropAsObservable()
		{
			return onEndDragAndDrop ?? (onEndDragAndDrop = new Subject<Unit>());
		}

		void Start()
		{
			var dungeonManager = DungeonManager.instance;
			bool isDragAndDrop = false;

			var block = GetComponent<Block>();
			var setter = GetComponent<BlockSetter>();

			// 開始
			this.OnMouseDownAsObservable()
				.Where(_ => dungeonManager.activeState == DungeonState.None)
				.Where(_ => !setter.putted)
				.Do(_ => isDragAndDrop = true)
				.Do(OnBeginDragAndDrop)
				.Subscribe(_ => SetPositionAtTouchPosition());

			// D&D
			this.OnMouseDragAsObservable()
			.Where(_ => isDragAndDrop)
				.Do(OnDragAndDrop)
				.Subscribe(_ => SetPositionAtTouchPosition());

			// 終了
			this.OnMouseUpAsObservable()
			.Where(_ => isDragAndDrop)
			.Do(_ => isDragAndDrop = false)
			.Subscribe(OnEndDragAndDrop);

			// Putされるときの移動
			setter.OnPutBlockAsObservable()
			.Subscribe(_ =>
			{
				Vector3 target = dungeonManager.mapManager.ToPosition(block.location);
				float time = 1;
				iTween.MoveTo(block.gameObject, target, time);
			});
		}

		private void SetPositionAtTouchPosition()
		{
			var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			position.z = transform.position.z;
			transform.position = position;
		}

		private void OnBeginDragAndDrop(Unit unit)
		{
			if (onBeginDragAndDrop != null)
			{
				onBeginDragAndDrop.OnNext(unit);
			}
		}

		private void OnDragAndDrop(Unit unit)
		{
			if (onDragAndDrop != null)
			{
				onDragAndDrop.OnNext(unit);
			}
		}

		private void OnEndDragAndDrop(Unit unit)
		{
			if (onEndDragAndDrop != null)
			{
				onEndDragAndDrop.OnNext(unit);
			}
		}
	}
}