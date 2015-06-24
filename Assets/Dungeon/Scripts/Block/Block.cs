//#define TEST
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Memoria.Dungeon.Managers;

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

		public Image image { get; set; }

		public SpriteRenderer spriteRenderer { get; set; }

		public Animator animator { get; set; }

		public BlockData blockData
		{
			get
			{
				return new BlockData(location, shape, type, hasEvent);
			}
		}

		public Location location
		{
			get
			{
				return (isSpriteRenderer) ? mapManager.ToLocation(transform.localPosition) : new Location(0, 0);
			}

			set
			{
				if (isSpriteRenderer)
				{
					transform.localPosition = mapManager.ToPosition(value);
				}
				else
				{
					transform.localPosition = Vector3.zero;
				}
			}
		}

		public int shapeType
		{
			get
			{
				return shape.type;
			}
			set
			{
				BlockShape _shape = shape;
				_shape.type = value;
				shape = _shape;
				SetSprite(shape, type);
			}
		}

		public BlockShape shape { get; private set; }

		private BlockType _type = BlockType.None;

		public BlockType type
		{
			get
			{
				return _type;
			}

			set
			{
				_type = value;
				SetSprite(shape, _type);
			}
		}

		public bool hasEvent { get; private set; }

		private bool _isOperating = false;

		public bool isOperating
		{
			get { return _isOperating && dungeonManager.activeState == DungeonState.BlockOperating; }
			private set { _isOperating = value; }
		}

		private bool _isSpriteRenderer = false;

		private bool isSpriteRenderer
		{
			get { return _isSpriteRenderer; }
			set
			{
				_isSpriteRenderer = value;
				animator.SetBool("isSpriteRenderer", value);
			}
		}

		public bool putted
		{
			get { return animator.GetBool("putted"); }
			private set { animator.SetBool("putted", value); }
		}

		private BlockFactor _blockFactor;

		public BlockFactor blockFactor
		{
			get
			{
				return _blockFactor;
			}

			set
			{
				_blockFactor = value;
				transform.SetParent((value) ? value.transform : null);
			}
		}

		private Rect listRect = new Rect(8, -4.4f, 6, 8);

#region messages

		// Update is called once per frame
		void Update()
		{
			if (isOperating)
			{
				OnOperating();
			}
		}

		void OnMouseDown()
		{
			bool isNoneState = dungeonManager.activeState == DungeonState.None;
			bool inListRect = listRect.Contains(transform.position - Camera.main.transform.position);
			bool canOperation = isNoneState && inListRect;

			if (putted)
			{
				bool onPlayer = location == dungeonManager.player.location;
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				bool contains = mapManager.canPutBlockArea.Contains(touchPosition);
				if (!onPlayer && contains && isNoneState)
				{
					StartCoroutine("CoroutineBreak");
				}
			}
			else if (canOperation)
			{
				StartOperation();
			}
		}

		void OnMouseUp()
		{
			if (putted)
			{
				StopCoroutine("CoroutineBreak");
			}
			else if (isOperating)
			{
				StopOperation();

				if (CanPut())
				{
					Put();
				}
				else
				{
					Back();
				}
			}
		}

		// 操作を開始するとき
		void StartOperation()
		{
			isOperating = true;
			transform.SetParent(null);
			isSpriteRenderer = true;
			dungeonManager.operatingBlock = this;
			dungeonManager.EnterState(DungeonState.BlockOperating);
		}

		// 操作中のとき
		void OnOperating()
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0;
			transform.position = pos;
		}

		// 操作を終了するとき
		void StopOperation()
		{
			isOperating = false;
			dungeonManager.operatingBlock = null;
			dungeonManager.ExitState();
		}

		// ブロックイベントが発生したとき
		public void OnEnterBlockEvent()
		{
			if (!hasEvent)
			{
				return;
			}

			hasEvent = false;
		}

		public void OnExitBlockEvent()
		{
			type = BlockType.None;
			hasEvent = false;
		}

#endregion

		public void Initialize()
		{
			dungeonManager = DungeonManager.instance;
			blockManager = dungeonManager.blockManager;
			mapManager = dungeonManager.mapManager;

			image = GetComponent<Image>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			animator = GetComponent<Animator>();
		}

		public void SetAsDefault(Location location, BlockShape shape, BlockType type)
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
			this.shape = shape;
			this.type = type;

			hasEvent = type != BlockType.None;
		}

		// (Coroutine)ブロックを破壊する
		private IEnumerator CoroutineBreak()
		{
			float delay = 0.5f;
			float elapsed = 0;
			BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

			while (elapsed < delay)
			{
				Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if (boxCollider2D.OverlapPoint(touchPosition))
				{
					elapsed += Time.deltaTime;
				}
				else
				{
					yield break;
				}

				yield return null;
			}

			Break();
		}

		// ブロックを破壊する
		private void Break()
		{
			ParameterManager paramaterManager = dungeonManager.parameterManager;
			paramaterManager.parameter.sp -= 2;

			mapManager.map.Remove(location);
			Destroy(gameObject);
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
			for (int i = 0; i < Location.directions.Length; i++)
			{
				if (CheckConnectedRoad(i, Location.directions[i]))
				{
					return true;
				}
			}

			return false;
		}

		private bool CheckConnectedRoad(int direction, Location checkDirection)
		{
			Location checkLocation = location + checkDirection;

			bool opened1 = shape.directions[direction];
			bool exsits = mapManager.map.ContainsKey(checkLocation);
			bool opened2 = exsits && mapManager.map[checkLocation].shape.directions[direction ^ 1];

			return opened1 && exsits && opened2;
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

			hasEvent = type != BlockType.None;
			blockFactor.OnPutBlock();
		}

		// BlockFactor に戻す
		private void Back()
		{
			transform.SetParent(blockFactor.transform);
			transform.localPosition = Vector3.zero;
			isSpriteRenderer = false;
		}

		private void SetSprite(BlockShape blockShape, BlockType blockType)
		{
			Sprite sprite = blockManager.GetBlockSprite(blockShape, blockType);
			image.sprite = sprite;
			spriteRenderer.sprite = sprite;
		}
	}
}