using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.Managers;
using UniRx;

namespace Memoria.Dungeon
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Vector2 offset;

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
        /// プレイヤーの歩く速さ
        /// </summary>
        public float speed
        {
            get { return animator.speed; }
            set { animator.speed = value; }
        }

        [SerializeField]
        private float _speed;

        /// <summary>
        /// プレイヤーのマップ上座標
        /// </summary>
        public Vector2Int location { get; set; }

        private Subject<Unit> onWalkBegin;
        private Subject<Unit> onWalkEnd;

        public IObservable<Unit> OnWalkBeginAsObservable()
        {
            return onWalkBegin ?? (onWalkBegin = new Subject<Unit>());
        }

        public IObservable<Unit> OnWalkEndAsObservable()
        {
            return onWalkEnd ?? (onWalkEnd = new Subject<Unit>());
        }

        private void OnWalkBegin()
        {
            if (onWalkBegin != null)
            {
                onWalkBegin.OnNext(Unit.Default);
            }
        }

        private void OnWalkEnd()
        {
            if (onWalkEnd != null)
            {
                onWalkEnd.OnNext(Unit.Default);
            }
        }

        void Awake()
        {
            dungeonManager = DungeonManager.instance;
            mapManager = dungeonManager.mapManager;
            var createBlock = BlockManager.instance.OnCreateBlockAsObservable();

            createBlock.Subscribe(block =>
                {
                    var onTap = block.OnTapAsObservable()
                        .Where(_ => dungeonManager.activeState == DungeonState.None)
                        .Select(_ => block.location)
                        .Subscribe(OnTap);

                    block.OnBreakAsObservable()
                        .Subscribe(_ => onTap.Dispose());
                });

            OnWalkBeginAsObservable()
                .Do(_ => animator.SetBool("moving", true))
                .Select(_ => DungeonState.PlayerMoving)
                .Subscribe(dungeonManager.EnterState);

            OnWalkEndAsObservable()
                .Do(_ => animator.SetBool("moving", false))
                .Subscribe(_ => dungeonManager.ExitState());

            speed = _speed;
        }

        void OnTap(Vector2Int touchLocation)
        {
            Vector2Int distance = touchLocation - location;
            if (distance.SqrMagnitude() < 1)
            {
                return;
            }

            //			Vector2Int moveDirection = new Vector2Int();
            //			int[] checkDirections = new [] { 0, 1 };
            //        float ax = Mathf.Abs(distance.x);
            //        float ay = Mathf.Abs(distance.y);
            //        int[] checkDirections = (ax >= ay) ? (new [] { 0, 1 }) : (new [] { 1, 0 });

            var canMoveDirections =
                (new List<Vector2Int>() { Vector2Int.up, Vector2Int.right })
                .Select(direction => ToNormalizeEachElement(distance * direction))
                .Where(CanMove);

            if (canMoveDirections.Any())
            {
                Move(canMoveDirections.First());
            }
        }

        private bool CanMove(Vector2Int moveDirection)
        {
            return (moveDirection != Vector2Int.zero) && mapManager.GetBlock(location).Connected(moveDirection);
        }

        private void Move(Vector2Int normalizedMoveDirection)
        {
            OnWalkBegin();

            Vector3 position = mapManager.ToPosition(location + normalizedMoveDirection);
            float time = 1 / speed;
            position += (Vector3)offset;
            iTween.MoveTo(gameObject, iTween.Hash(
                "position", position,
                "time", time,
                "oncomplete", "CompleteMove",
                "easetype", iTween.EaseType.linear));

            direction = ToDirection(normalizedMoveDirection);

            location += normalizedMoveDirection;
        }

        private void CompleteMove()
        {
            OnWalkEnd();
        }

        private Vector2Int ToNormalizeEachElement(Vector2Int vector)
        {
            Func<int, int> normalize = (int element) => ((element > 0) ? 1 : (element < 0) ? -1 : 0);
            return new Vector2Int(
                normalize(vector.x),
                normalize(vector.y));
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