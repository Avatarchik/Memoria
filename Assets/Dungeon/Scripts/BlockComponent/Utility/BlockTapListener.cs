using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockComponent.Utility
{
    public class BlockTapListener
    {
        private Block block;
        private float? raiseTime = null;
        private Subject<Unit> onTap;

        public IObservable<Unit> OnTapAsObservable()
        {
            return onTap ?? (onTap = new Subject<Unit>());
        }

        public void Bind(Block block)
        {
            this.block = block;

            // 時間計測終了
            block.UpdateAsObservable()
                .Where(_ => raiseTime != null && Time.realtimeSinceStartup > raiseTime)
                .Subscribe(_ => raiseTime = null);

            // ブロックがタップされたとき
            // 時間計測を開始する
            block.OnMouseDownAsObservable()
                .Where(_ => block.putted)
                .Subscribe(_ => raiseTime = Time.realtimeSinceStartup + 0.5f);

            // 時間計測終了
            block.OnMouseExitAsObservable()
                .Subscribe(_ => raiseTime = null);

            // 時間計測終了
            block.OnMouseUpAsObservable()
                .Do(_ => OnTap())
                .Subscribe(_ => raiseTime = null);
        }

        private void OnTap()
        {
            if (raiseTime == null || Time.realtimeSinceStartup > raiseTime)
            {
                return;
            }

            if (!MapManager.instance.canPutBlockArea.Contains(block.transform.position))
            {
                return;
            }

            if (onTap != null)
            {
                onTap.OnNext(Unit.Default);
            }
        }
    }
}