using UnityEngine;
using System.Collections;
using Memoria.Managers;
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

            return UnityEngine.Random.value < dungeonManager.dungeonData.stageData.probabilityOfEncounter;
        }

        private IEnumerator CoroutineBattleToBoss(Block block)
        {
            dungeonManager.dungeonData.SetIsBossBattle(true);            
            
            if (block.blockType == BlockType.Recovery)
            {
                dungeonManager.dungeonData.SetBattleType(block.blockType);
            }
            else
            {
                dungeonManager.dungeonData.SetBattleType(BlockType.None);
            }

            int idMin = dungeonManager.dungeonData.stageData.bossPatternIdMin;
            int idMax = dungeonManager.dungeonData.stageData.bossPatternIdMax;
            int id = Random.Range(idMin, idMax + 1);
            dungeonManager.dungeonData.SetEnemyPattern(id);

            dungeonManager.dungeonData.Save();
            eventAnimator.SetTrigger("onBossBattleEvent");
            SoundManager.instance.PlayBGM(3);
            yield return new WaitForSeconds(3f);
            Application.LoadLevel("Battle");
        }

        private IEnumerator CoroutineBattleToEnemy(Block block)
        {
            dungeonManager.dungeonData.SetIsBossBattle(false);
            dungeonManager.dungeonData.SetBattleType(block.blockType);
            
            int idMin = dungeonManager.dungeonData.stageData.enemyPatternIdMin;
            int idMax = dungeonManager.dungeonData.stageData.enemyPatternIdMax;
            int id = Random.Range(idMin, idMax + 1);
            dungeonManager.dungeonData.SetEnemyPattern(id);
            
            dungeonManager.dungeonData.Save();
            eventAnimator.SetTrigger("onBattleEvent");
            SoundManager.instance.PlayBGM(2);
            yield return new WaitForSeconds(1f);
            Application.LoadLevel("Battle");
        }
    }
}