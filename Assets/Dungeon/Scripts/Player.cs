using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.Managers;
using Memoria.Dungeon.BlockUtility;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon
{
	public class Player : MonoBehaviour
	{
		[SerializeField]
		private Vector3 offset;

		private DungeonManager dungeonManager;
		private MapManager mapManager;

		public Animator animator { get { return GetComponent<Animator>(); } }

		/// <summary>
		/// プレイヤーの向き
		/// 0 : left
		/// 1 : right
		/// 2 : down
		/// 3 : up
		/// </summary>
		public int direction
		{
			get { return (int)animator.GetFloat("direction"); }
			set { animator.SetFloat("direction", value); }
		}

		/// <summary>
		/// プレイヤーの移動中フラグ
		/// </summary>
		public bool isMoving
		{
			get { return animator.GetBool("moving"); }
			set { animator.SetBool("moving", value); }
		}

		/// <summary>
		/// プレイヤーの歩く速さ
		/// </summary>
		public float speed
		{
			get { return animator.speed; }
			set { animator.speed = value; }
		}

		[SerializeField]
		private float _speed;

#region messages

		/// <summary>
		/// プレイヤーのマップ上座標
		/// </summary>
		public Vector2Int location { get; set; }

		void Awake()
		{
			dungeonManager = DungeonManager.instance;
			mapManager = dungeonManager.mapManager;
		}

		// Use this for initialization
		void Start()
		{
			this.UpdateAsObservable()
				.Where(_ => Input.GetMouseButtonDown(0))
				.Where(_ => dungeonManager.activeState == DungeonState.None)
				.Subscribe(_ => StartCoroutine(CoroutineTouch()));

			speed = _speed;
		}

		void OnTouchMap(Vector2Int touchLocation)
		{
			Vector2Int distance = touchLocation - location;
			if (distance.magnitude < 1)
			{
				return;
			}

//			Vector2Int moveDirection = new Vector2Int();
//			int[] checkDirections = new [] { 0, 1 };
//        float ax = Mathf.Abs(distance.x);
//        float ay = Mathf.Abs(distance.y);
//        int[] checkDirections = (ax >= ay) ? (new [] { 0, 1 }) : (new [] { 1, 0 });

			(new List<Vector2Int>() { Vector2Int.up, Vector2Int.right })
			.Select(direction => ToNormalizeEachElement(distance * direction))
			.Where(CanMove)
			.ToList()
			.ForEach(Move);
		}

		private Vector2Int ToNormalizeEachElement(Vector2Int vector)
		{
			Func<int, int> normalize = (int element) => ((element > 0) ? 1 : (element < 0) ? -1 : 0);
			return new Vector2Int(
				normalize(vector.x),
				normalize(vector.y));
		}

		private void Move(Vector2Int normalizedMoveDirection)
		{
			dungeonManager.EnterState(DungeonState.PlayerMoving);

			Vector3 position = mapManager.ToPosition(location + normalizedMoveDirection);
			float yOffset = -0.2f;
			float time = 1 / speed;
			position.y += yOffset;
			iTween.MoveTo(gameObject, iTween.Hash(
				"position", position,
				"time", time,
				"oncomplete", "CompleteMove",
				"easetype", iTween.EaseType.linear));

			direction = ToDirection(normalizedMoveDirection);
			isMoving = true;
        
			location += normalizedMoveDirection;
		}

#endregion

		private IEnumerator CoroutineTouch()
		{
			float time = 0;
			while (Input.GetMouseButton(0))
			{
				time += Time.deltaTime;
				yield return null;
			}
        
			float limit = 0.5f;
			if (time < limit)
			{
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2Int touchLocation = mapManager.ToLocation(touchPosition);

				if (mapManager.canPutBlockArea.Contains(touchPosition) && mapManager.map.ContainsKey(touchLocation))
				{
					OnTouchMap(touchLocation);
				}
			}
        
			yield break;
		}

		private void CompleteMove()
		{
			isMoving = false;
			dungeonManager.ExitState();
		}

		private bool CanMove(Vector2Int moveDirection)
		{			
			if (moveDirection.sqrMagnitude == 0)
			{
				return false;
			}
			
			Vector2Int nextLocation = location + moveDirection;

			if (!mapManager.map.ContainsKey(nextLocation))
			{
				return false;
			}

			Block now = mapManager.map[location];
			Block next = mapManager.map[nextLocation];
			int dir = ToDirection(moveDirection);

			return now.shape.directions[dir] && next.shape.directions[dir ^ 1];
		}

		private int ToDirection(Vector2Int normalizedMoveDirection)
		{
			int[,] toDirectionTable = new int[,]
			{
				{ 0, 0, 1 },
				{ 2, 0, 3 },
			};

			int directionX = toDirectionTable[0, normalizedMoveDirection.x + 1];
			int directionY = toDirectionTable[1, normalizedMoveDirection.y + 1];

			return directionX + directionY;
		}

		public void SetPosition(Vector2Int location)
		{
			this.location = location;
			transform.position = mapManager.ToPosition(location) + offset;
		}
	}
}