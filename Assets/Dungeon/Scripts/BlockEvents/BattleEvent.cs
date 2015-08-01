using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;
using Memoria.Dungeon.BlockComponent;

namespace Memoria.Dungeon.BlockEvents
{
    public class BattleEvent
    {
        private static DungeonManager dungeonManager
        {
            get { return DungeonManager.instance; }
        }

        private static DungeonParameter parameter
        {
            get { return ParameterManager.instance.parameter; }
        }

        private MonoBehaviour coroutineAppended;
        private Animator eventAnimator;

        public bool onBattleEvent { get; private set; }

        public BattleEvent(MonoBehaviour coroutineAppended, Animator eventAnimator)
        {
            this.coroutineAppended = coroutineAppended;
            this.eventAnimator = eventAnimator;
        }

        public Coroutine StartBattleEventCoroutine(Block block, bool itemTaked)
        {
            return coroutineAppended.StartCoroutine(CoroutineBattle(block, itemTaked));
        }

        private IEnumerator CoroutineBattle(Block block, bool itemTaked)
        {
            onBattleEvent = false;

            if (itemTaked)
            {
                if (OnTriggerOnBossBattleEvent())
                {
                    onBattleEvent = true;
                    yield return coroutineAppended.StartCoroutine(CoroutineBattleToBoss(block));
                }
            }
            else
            {
                if (OnTriggerOnBattleEvent(block))
                {
                    onBattleEvent = true;
                    yield return coroutineAppended.StartCoroutine(CoroutineBattleToEnemy(block));
                }
            }

            yield break;
        }

        private bool OnTriggerOnBossBattleEvent()
        {
            return parameter.getKeyNum == parameter.allKeyNum;
        }

        private bool OnTriggerOnBattleEvent(Block block)
        {
            switch (block.blockType)
            {
                case BlockType.None:
                case BlockType.Recovery:
                    return false;
            }

            return UnityEngine.Random.value < 0.3f;
        }

        private IEnumerator CoroutineBattleToBoss(Block block)
        {
            // TODO : blockType == BlockType.Recoveryの時の対処
            dungeonManager.dungeonData.SetBattleType(block.blockType);
            dungeonManager.dungeonData.Save();
            yield return new WaitForSeconds(0.5f);
            Application.LoadLevel("Battle");
        }

        private IEnumerator CoroutineBattleToEnemy(Block block)
        {
            dungeonManager.dungeonData.SetBattleType(block.blockType);
            dungeonManager.dungeonData.Save();
            yield return new WaitForSeconds(0.5f);
            Application.LoadLevel("Battle");
        }
    }
}