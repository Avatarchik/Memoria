using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.Managers;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon.BlockUtility
{
	public enum BlockType
	{
		None = 0,
		Fire = 1,
		Wind = 2,
		Thunder = 3,
		Water = 4,
		Recovery = 5,
	}

	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(BlockMover))]
	[RequireComponent(typeof(BlockSetter))]
	public class Block : MonoBehaviour
	{
		private DungeonManager dungeonManager;
		private BlockManager blockManager;
		private MapManager mapManager;
		private ParameterManager parameterManager;

		public Image image { get; set; }

		public SpriteRenderer spriteRenderer { get; set; }

		public Animator animator { get; set; }

		public BlockData blockData { get { return new BlockData(location, shapeData, blockType); } }

		public Vector2Int location
		{
			get { return mapManager.ToLocation(transform.localPosition); }
			set { transform.localPosition = (Vector3)mapManager.ToPosition(value); }
		}

		private ShapeData _shapeData;

		public ShapeData shapeData
		{
			get { return _shapeData; }

			set
			{
				_shapeData = value;
				SetSprite(_shapeData, _blockType);
			}
		}

		private BlockType _blockType;

		public BlockType blockType
		{
			get { return _blockType; }

			set
			{
				_blockType = value;
				SetSprite(_shapeData, _blockType);
			}
		}

		private BlockFactor _blockFactor;

		public BlockFactor blockFactor
		{
			get { return _blockFactor; }

			set
			{
				_blockFactor = value;
				transform.SetParent((value) ? value.transform : null);
			}
		}

		// TODO: private BlockMover mover;
		public BlockMover mover { get; private set; }

		// TOOD: private BlockSetter setter;
		public BlockSetter setter { get; private set; }

		// TODO: private BlockBreaker breaker;
		public BlockBreaker breaker { get; private set; }

		public IObservable<Unit> OnBeginDragAndDropAsObservable()
		{
			return mover.OnBeginDragAndDropAsObservable();
		}

		public IObservable<Unit> OnDragAndDropAsObservable()
		{
			return mover.OnDragAndDropAsObservable();
		}

		public IObservable<Unit> OnEndDragAndDropAsObservable()
		{
			return mover.OnEndDragAndDropAsObservable();
		}

		public IObservable<Unit> OnPutBlockAsObservable()
		{
			return setter.OnPutBlockAsObservable();
		}

		public IObservable<Unit> OnBreakBlockAsObservable()
		{
			return breaker.OnBreakBlockAsObservable();
		}

		public void Initialize()
		{
			dungeonManager = DungeonManager.instance;
			blockManager = dungeonManager.blockManager;
			mapManager = dungeonManager.mapManager;
			parameterManager = dungeonManager.parameterManager;

			image = GetComponent<Image>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			animator = GetComponent<Animator>();

			mover = GetComponent<BlockMover>();
			setter = GetComponent<BlockSetter>();
			breaker = GetComponent<BlockBreaker>();

			// 移動開始時
			mover.OnBeginDragAndDropAsObservable()
			.Subscribe(_ =>
			{
				transform.SetParent(null);
				animator.SetBool("isSpriteRenderer", true);
				dungeonManager.operatingBlock = this;
				dungeonManager.EnterState(DungeonState.BlockOperating);
			});

			// 移動終了時
			mover.OnEndDragAndDropAsObservable()
			.Subscribe(_ =>
			{
				dungeonManager.operatingBlock = null;
				dungeonManager.ExitState();
			});

			// 設置時
			setter.OnPutBlockAsObservable()
			.Subscribe(_ =>
			{
				animator.SetBool("putted", true);
				animator.SetBool("isSpriteRenderer", true);

				mapManager.map[location] = this;
				spriteRenderer.sortingOrder = 0;

				Vector3 target = mapManager.ToPosition(location);
				float time = 1;
				iTween.MoveTo(gameObject, target, time);

				if (blockFactor != null)
				{
					blockFactor.OnPutBlock();
				}
			});

			// 返却時
			setter.OnBackBlockAsObservable()
			.Subscribe(_ =>
			{
				transform.SetParent(blockFactor.transform);
				transform.localPosition = Vector3.zero;
				animator.SetBool("isSpriteRenderer", false);
			});

//			// 破壊イベントの登録
//			var onMouseLongDownComponent = gameObject.AddComponent<ObservableOnMouseLongDownTrigger>();
//			onMouseLongDownComponent.intervalSecond = 0.5f;
//			onMouseLongDownComponent.OnMouseLongDownAsObservable()
//			.Where(CanBreak)
//			.Subscribe(Break);

			// タップイベントの追加
			var onMouseShortUpAsButtonInCollider = gameObject.AddComponent<ObservableOnMouseShortUpAsButtonInColliderTrigger>();
			onMouseShortUpAsButtonInCollider.limitSecond = 0.5f;
			onMouseShortUpAsButtonInCollider.OnMouseShortUpAsButtonInColliderAsObservable()
			.Where(_ => setter.putted)
			.Subscribe(_ => dungeonManager.eventManager.OnTapBlock(this));
		}

		public void SetAsDefault(Vector2Int location, ShapeData shape, BlockType type)
		{
			if (mapManager.map.ContainsKey(location))
			{
				throw new UnityException("指定した場所にはすでにブロックがあります " + location);
			}

			setter.Put();

			// ブロックの位置, 形状, 種類を設定
			this.location = location;
			this.shapeData = shape;
			this.blockType = type;
		}

#region Break

//		private bool CanBreak(Unit _ = null)
//		{
//			if (!setter.putted)
//			{
//				return false;
//			}
//
//			bool isNoneState = dungeonManager.activeState == DungeonState.None;
//			bool onPlayer = location == dungeonManager.player.location;
//			Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			bool contains = mapManager.canPutBlockArea.Contains(touchPosition);
//			return isNoneState && !onPlayer && contains;
//		}

//		// ブロックを破壊する
//		private void Break(Unit _ = null)
//		{
//			dungeonManager.eventManager.OnBreakBlcok();
//			mapManager.map.Remove(location);
//			Destroy(gameObject);
//		}

#endregion

		// ブロックイベントが発生したとき
		public void OnEnterBlockEvent()
		{
			if (blockType == BlockType.None)
			{
				return;
			}
		}

		public void OnExitBlockEvent()
		{
			blockType = BlockType.None;
		}

		private void SetSprite(ShapeData blockShape, BlockType blockType)
		{
			Sprite sprite = blockManager.GetBlockSprite(blockShape, blockType);
			image.sprite = sprite;
			spriteRenderer.sprite = sprite;
		}
	}
}