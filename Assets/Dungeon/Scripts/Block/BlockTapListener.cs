using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon.BlockComponent.Utility
{
    public class BlockTapListener
    {
        private Subject<Unit> onTap;

        public IObservable<Unit> OnTapAsObservable()
        {
            return onTap ?? (onTap = new Subject<Unit>());
        }

        public void Bind(Block block)
        {
            float? raiseTime = null;
            block.UpdateAsObservable()
                .Where(_ => raiseTime != null && Time.realtimeSinceStartup > raiseTime)
                .Subscribe(_ => raiseTime = null);

            block.OnMouseDownAsObservable()
                .Where(_ => block.putted)
                .Subscribe(_ => raiseTime = Time.realtimeSinceStartup + 0.5f);

            block.OnMouseExitAsObservable()
                .Subscribe(_ => raiseTime = null);

            block.OnMouseUpAsObservable()
                .Do(_ => { if (Time.realtimeSinceStartup <= raiseTime) OnTap(); })
                .Subscribe(_ => raiseTime = null);
        }

        private void OnTap()
        {
            if (onTap != null)
            {
                onTap.OnNext(Unit.Default);
            }
        }
    }
}