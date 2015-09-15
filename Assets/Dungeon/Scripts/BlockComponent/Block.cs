using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Memoria.Dungeon.Managers;
using Memoria.Dungeon.BlockComponent.Utility;

namespace Memoria.Dungeon.BlockComponent
{
	[Serializable]
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
        private static DungeonManager dungeonManager { get { return DungeonManager.instance; } }

        private static MapManager mapManager { get { return MapManager.instance; } }

        public BlockData blockData { get { return new BlockData(location, shapeData, blockType); } }

        public Vector2Int location
        {
            get { return mapManager.ToLocation(transform.localPosition); }
            set { transform.localPosition = (Vector3)mapManager.ToPosition(value); }
        }

        public ShapeData shapeData { get; set; }

        public BlockType blockType { get; set; }

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

        private BlockMover mover = new BlockMover();

        private BlockSetter setter = new BlockSetter();

        private BlockBreaker breaker = new BlockBreaker();

        private BlockTapListener tapListener = new BlockTapListener();

        private BlockSprite sprite = new BlockSprite();

        #region wrapper

        public IObservable<Unit> OnMoveBeginAsObservable()
        {
            return mover.OnMoveBeginAsObservable();
        }

        public IObservable<Unit> OnMoveAsObservable()
        {
            return mover.OnMoveAsObservable();
        }

        public IObservable<Unit> OnMoveEndAsObservable()
        {
            return mover.OnMoveEndAsObservable();
        }

        public IObservable<Unit> OnPutAsObservable()
        {
            return setter.OnPutAsObservable();
        }

        public IObservable<Unit> OnBackAsObservable()
        {
            return setter.OnBackAsObservable();
        }

        public IObservable<Unit> OnBreakAsObservable()
        {
            return breaker.OnBreakAsObservable();
        }

        public IObservable<Unit> OnTapAsObservable()
        {
            return tapListener.OnTapAsObservable();
        }

        public bool putted { get { return setter.putted; } }

        public bool CanPut()
        {
            return setter.CanPut();
        }

        public bool Connected(Vector2Int checkBaseDirection)
        {
            return setter.Connected(checkBaseDirection);
        }

        #endregion

        public void Initialize()
        {
            var animator = GetComponent<Animator>();

            shapeData = new ShapeData(0);
            blockType = BlockType.None;

            setter.Bind(this);
            mover.Bind(this);
            breaker.Bind(this);
            tapListener.Bind(this);
            sprite.Bind(this);

            // 移動開始時
            var onMoveBegin = OnMoveBeginAsObservable()
                .Subscribe(_ =>
                {
                    transform.SetParent(null);
                    dungeonManager.EnterState(DungeonState.BlockOperating);
                });

            // 移動終了時
            var onMoveEnd = OnMoveEndAsObservable()
                .Subscribe(_ => dungeonManager.ExitState());

            // 設置時
            OnPutAsObservable()
                .Subscribe(_ =>
                {
                    animator.SetBool("putted", true);
                    onMoveBegin.Dispose();
                    onMoveEnd.Dispose();
                });

            // 破壊時
            OnBreakAsObservable()
                .Subscribe(_ =>
                {
                    var effectPosition = (Vector3)mapManager.ToPosition(location);
                    EffectManager.instance.InstantiateEffect(13, effectPosition, 2f);
                });
        }

        public void SetAsDefault(Vector2Int location, ShapeData shape, BlockType type)
        {
			if (mapManager.ExistsBlock(location))
            {
                throw new UnityException("指定した場所にはすでにブロックがあります " + location);
            }

            // ブロックの位置, 形状, 種類を設定
            this.location = location;
            this.shapeData = shape;
            this.blockType = type;

            setter.Put();
        }
		
		public void TakeStock()
		{
			blockType = BlockType.None;
		}
    }
}