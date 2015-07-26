using UnityEngine;
using System;
using System.Collections;
using UniRx;
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

        public BattleEvent(MonoBehaviour coroutineAppended, Animator eventAnimator)
        {
            this.coroutineAppended = coroutineAppended;
            this.eventAnimator = eventAnimator;
        }

        public Func<bool, IObservable<bool>> CreateBattleEventAsObservable(Block block)
        {
            return (bool taked) =>
                Observable.FromCoroutine<bool>(observer => CoroutineBattle(observer, block, taked));
        }

        private IEnumerator CoroutineBattle(IObserver<bool> observer, Block block, bool takedItem)
        {
            bool onTriggerBattle = false;

            // ボス戦
            if (takedItem && OnTriggerOnBossBattleEvent())
            {
                onTriggerBattle = true;
                yield return coroutineAppended.StartCoroutine(CoroutineBattleToBoss(block));
            }
            // 通常
            else if (OnTriggerOnBattleEvent(block))
            {
                onTriggerBattle = true;
                yield return coroutineAppended.StartCoroutine(CoroutineBattleToEnemy(block));
            }
            
            observer.OnNext(onTriggerBattle);
            observer.OnCompleted();
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