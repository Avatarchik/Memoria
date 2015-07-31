using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;
using Memoria.Dungeon.BlockComponent;

namespace Memoria.Dungeon.BlockEvents
{
    public class PowerTakeEvent
    {
        private static DungeonManager dungeonManager
        {
            get { return DungeonManager.instance; }
        }

        private MonoBehaviour coroutineAppended;
        private Animator eventAnimator;

        public PowerTakeEvent(MonoBehaviour coroutineAppended, Animator eventAnimator)
        {
            this.coroutineAppended = coroutineAppended;
            this.eventAnimator = eventAnimator;
        }

        public Coroutine StartTakePowerCoroutine(Block block)
        {
            return coroutineAppended.StartCoroutine(CoroutineTakePower(block));
        }

        private IEnumerator CoroutineTakePower(Block block)
        {
            dungeonManager.EnterState(DungeonState.StockTaking);

            switch (block.blockType)
            {
                case BlockType.Thunder:
                case BlockType.Water:
                case BlockType.Fire:
                case BlockType.Wind:
                    yield return coroutineAppended.StartCoroutine(CoroutineTakePowerTypeOfElements(block.blockType));
                    break;

                case BlockType.Recovery:
                    yield return coroutineAppended.StartCoroutine(CoroutineTakePowerTypeOfRecovery());
                    break;
            }

            block.TakeStock();
            dungeonManager.ExitState();
            yield break;
        }

        private IEnumerator CoroutineTakePowerTypeOfElements(BlockType attribute)
        {
            yield break;
        }

        private IEnumerator CoroutineTakePowerTypeOfRecovery()
        {
            // TODO : 体力回復
            eventAnimator.SetFloat("eventType", 1);
            EventManager.instance.message = "ＨＰ回復！！";
            eventAnimator.SetTrigger("getPower");
            yield return new WaitForSeconds(1);
        }
    }
}