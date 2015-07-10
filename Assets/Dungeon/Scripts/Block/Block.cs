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
			get { return (isSpriteRenderer) ? mapManager.ToLocation(transform.localPosition) : new Vector2Int(0, 0); }
			set { transform.localPosition = isSpriteRenderer ? (Vector3)mapManager.ToPosition(value) : Vector3.zero; }
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

		private bool isSpriteRenderer
		{
			get { return animator.GetBool("isSpriteRenderer"); }
			set { animator.SetBool("isSpriteRenderer", value); }
		}

		public bool putted
		{
			get { return animator.GetBool("putted"); }
			private set { animator.SetBool("putted", value); }
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

		public void Initialize()
		{
			dungeonManager = DungeonManager.instance;
			blockManager = dungeonManager.blockManager;
			mapManager = dungeonManager.mapManager;
			parameterManager = dungeonManager.parameterManager;

			image = GetComponent<Image>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			animator = GetComponent<Animator>();

			// 操作イベントの登録
			this.OnMouseDownAsObservable()
			.Where(CanOperation)
			.Do(StartOperation)
			.Subscribe(_ =>
			{
				var onMouseDrag =
					this.OnMouseDragAsObservable()
					.Subscribe(Operate);
				
				this.OnMouseUpAsObservable()
				.First()
				.Do(__ => onMouseDrag.Dispose())
				.Do(CheckAndPut)
				.Subscribe(StopOperation);
			});

			// 破壊イベントの登録
			var onMouseLongDownComponent = gameObject.AddComponent<ObservableOnMouseLongDownTrigger>();
			onMouseLongDownComponent.intervalSecond = 0.5f;
			onMouseLongDownComponent.OnMouseLongDownAsObservable()
			.Where(CanBreak)
			.Subscribe(Break);

			// タップイベントの追加
			var onMouseShortUpAsButtonInCollider = gameObject.AddComponent<ObservableOnMouseShortUpAsButtonInColliderTrigger>();
			onMouseShortUpAsButtonInCollider.limitSecond = 0.5f;
			onMouseShortUpAsButtonInCollider.OnMouseShortUpAsButtonInColliderAsObservable()
			.Where(_ => putted)
			.Subscribe(_ => dungeonManager.eventManager.OnTapBlock(this));
		}

		public void SetAsDefault(Vector2Int location, ShapeData shape, BlockType type)
		{
			if (mapManager.map.ContainsKey(location))
			{
				throw new UnityException("指定した場所にはすでにブロックがあります " + location);
			}

			// 各種設定
			mapManager.map[location] = this;
			isSpriteRenderer = true;
			putted = true;
			spriteRenderer.sortingOrder = 0;

			// ブロックの位置, 形状, 種類を設定
			this.location = location;
			this.shapeData = shape;
			this.blockType = type;
		}



#region Operating

		private bool CanOperation(Unit _ = null)
		{
			if (putted)
			{
				return false;
			}

			bool isNoneState = dungeonManager.activeState == DungeonState.None;
			return isNoneState;
		}

		// 操作を開始するとき
		private void StartOperation(Unit _ = null)
		{
			transform.SetParent(null);
			Operate();
			isSpriteRenderer = true;
			dungeonManager.operatingBlock = this;
			dungeonManager.EnterState(DungeonState.BlockOperating);
		}

		// 操作を終了するとき
		private void StopOperation(Unit _ = null)
		{
			dungeonManager.operatingBlock = null;
			dungeonManager.ExitState();
		}

		private void Operate(Unit _ = null)
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0;
			transform.position = pos;
		}

		private void CheckAndPut(Unit _ = null)
		{
			if (CanPut())
			{
				Put();
			}
			else
			{
				Back();
			}
		}

		// ブロックを置くことができるか判定する
		public bool CanPut()
		{
			// 範囲外チェック
			if (!mapManager.canPutBlockArea.Contains(transform.position))
			{
				return false;
			}

			// 置くところのブロックチェック
			if (mapManager.map.ContainsKey(location))
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
			if (!shapeData.Opend(checkBaseDirection))
			{
				return false;
			}

			Vector2Int checkLocation = location + checkBaseDirection;
			if (!mapManager.map.ContainsKey(checkLocation))
			{
				return false;
			}

			Block checkBlock = mapManager.map[checkLocation];
			return checkBlock.shapeData.Opend(-checkBaseDirection);
		}

		// ブロックを置く
		private void Put()
		{
			mapManager.map[location] = this;
			putted = true;
			spriteRenderer.sortingOrder = 0;

			Vector3 target = mapManager.ToPosition(location);
			float time = 1;
			iTween.MoveTo(gameObject, target, time);

			blockFactor.OnPutBlock();
		}

		// BlockFactor に戻す
		private void Back()
		{
			transform.SetParent(blockFactor.transform);
			transform.localPosition = Vector3.zero;
			isSpriteRenderer = false;
		}

#endregion

#region Break

		private bool CanBreak(Unit _ = null)
		{
			if (!putted)
			{
				return false;
			}

			bool isNoneState = dungeonManager.activeState == DungeonState.None;
			bool onPlayer = location == dungeonManager.player.location;
			Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			bool contains = mapManager.canPutBlockArea.Contains(touchPosition);
			return isNoneState && !onPlayer && contains;
		}

		// ブロックを破壊する
		private void Break(Unit _ = null)
		{
			dungeonManager.eventManager.OnBreakBlcok();
			mapManager.map.Remove(location);
			Destroy(gameObject);
		}

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