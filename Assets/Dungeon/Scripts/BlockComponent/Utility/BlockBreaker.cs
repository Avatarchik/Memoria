using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockComponent.Utility
{
    public class BlockBreaker
    {
        private Subject<Unit> onBreak;

        public Subject<Unit> OnBreakAsObservable()
        {
            return onBreak ?? (onBreak = new Subject<Unit>());
        }

        public void Bind(Block block)
        {
            float raiseTime = 0;
            block.UpdateAsObservable()
                .Where(_ => block.putted)
                .Where(_ => DungeonManager.instance.activeState == DungeonState.None)
                .Where(_ => block.location != DungeonManager.instance.player.location)
                .SkipUntil(block.OnMouseDownAsObservable()
                    .Do(_ => raiseTime = Time.realtimeSinceStartup + 0.5f))
                .TakeUntil(block.OnMouseUpAsObservable()
                    .Merge(block.OnMouseExitAsObservable()))
                .Repeat()
                .Where(_ => Time.realtimeSinceStartup >= raiseTime)
                .Do(OnBreak)
                .Subscribe(_ => GameObject.Destroy(block.gameObject));
        }

        private void OnBreak(Unit unit)
        {
            if (onBreak != null)
            {
                onBreak.OnNext(Unit.Default);
                onBreak.OnCompleted();
            }
        }
    }
}