using UnityEngine;
using System.Linq;
using UniRx;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockComponent.Utility
{
    public class BlockSetter
    {
        private static Vector2Int[] checkDirections = new Vector2Int[]
        {
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.up,
        };

        private static MapManager mapManager { get { return DungeonManager.instance.mapManager; } }

        private Block block;

        public bool putted { get; private set; }

        private Subject<Unit> onPut;
        private Subject<Unit> onBack;

        public IObservable<Unit> OnPutAsObservable()
        {
            return onPut ?? (onPut = new Subject<Unit>());
        }

        public IObservable<Unit> OnBackAsObservable()
        {
            return onBack ?? (onBack = new Subject<Unit>());
        }

        public void Bind(Block block)
        {
            this.block = block;
            // 設置もしくはリストに戻す
            block.OnMoveEndAsObservable()
                .Subscribe(_ => PutOrBack());

            // 設置したとき
            // 設置位置に移動させる
            block.OnPutAsObservable()
                .Do(_ => putted = true)
                .Subscribe(_ => iTween.MoveTo(
                    target: block.gameObject,
                    position: DungeonManager.instance.mapManager.ToPosition(block.location),
                    time: 1));

            // リストに戻されたとき
            // 初期位置に移動させる
            block.OnBackAsObservable()
                .Subscribe(_ =>
                {
                    block.transform.SetParent(block.blockFactor.transform);
                    block.transform.localPosition = Vector3.zero;
                });
        }

        public bool CanPut()
        {
            if (!mapManager.canPutBlockArea.Contains(block.transform.position)
                || mapManager.ExistsBlock(block.location))
            {
                return false;
            }

            return checkDirections.Any(Connected);
        }

        public bool Connected(Vector2Int checkBaseDirection)
        {
            Vector2Int checkLocation = block.location + checkBaseDirection;

            if (!block.shapeData.Opend(checkBaseDirection)
                || !mapManager.ExistsBlock(checkLocation))
            {
                return false;
            }

            Block checkBlock = mapManager.GetBlock(checkLocation);
            return checkBlock.shapeData.Opend(-checkBaseDirection);
        }

        private void PutOrBack()
        {
            if (CanPut())
            {
                Put();
                var effectPoisition = (Vector3)mapManager.ToPosition(block.location);
                EffectManager.instance.InstantiateEffect(11, effectPoisition, 2f);
            }
            else
            {
                Back();
            }
        }

        public void Put()
        {
            onPut.OnNext(Unit.Default);
            onPut.OnCompleted();
        }

        public void Back()
        {
            onBack.OnNext(Unit.Default);
        }
    }
}