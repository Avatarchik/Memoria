using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.BlockEvents;
using Memoria.Dungeon.Items;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon.Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager instance { get { return DungeonManager.instance.eventManager; } }

        private DungeonManager dungeonManager;
        private MapManager mapManager;
        private ParameterManager parameterManager;
        private Player player;

        // 0 : root/Rizell/reaction
        [SerializeField]
        private Animator[] eventAnimators;

        [SerializeField]
        private GameObject messageBox;

        public Text messageBoxText { get; set; }

        void Awake()
        {
            dungeonManager = DungeonManager.instance;
            mapManager = dungeonManager.mapManager;
            parameterManager = dungeonManager.parameterManager;
            player = dungeonManager.player;

            player.OnWalkEndAsObservable()
                .Subscribe(_ => OnArrivePlayer());

            messageBoxText = messageBox.GetComponentInChildren<Text>();
            messageBox.SetActive(false);
        }

        // プレイヤーが歩き終わったときに呼び出される
        public void OnArrivePlayer()
        {
            // プレイヤーがいるブロックを取得			
            Block block = mapManager.GetBlock(player.location);
            if (block.blockType == BlockType.None)
            {

                return;
            }

            var takeItem = Observable.FromCoroutine<bool>(observer => CoroutineTakeItem(observer, player.location));
            Func<bool, IObservable<bool>> battle =
                 (bool taked) => Observable.FromCoroutine<bool>(observer => CoroutineBattle(observer, block, taked));
            var takePower = Observable.FromCoroutine(() => CoroutineTakePower(block));
            var checkRemain = Observable.FromCoroutine(CoroutineCheckRemain);

            takeItem.Last()
                .SelectMany(battle).Last()
                .Where(onTriggerBattle => !onTriggerBattle)
                .SelectMany(takePower)
                .SelectMany(checkRemain)
                .Subscribe();
        }
        
        private IEnumerator CoroutineTakeItem(IObserver<bool> observer, Vector2Int location)
        {
            bool exists = mapManager.ExistsItem(location);

            if (exists)
            {
                Item item = mapManager.GetItem(location);

                switch (item.itemData.type)
                {
                    case ItemType.Key:
                        messageBoxText.text = "鍵を入手した！！";
                        messageBox.SetActive(true);
                        yield return new WaitForSeconds(1);
                        messageBox.SetActive(false);
                        break;

                    case ItemType.Jewel:
                        messageBoxText.text = "宝石を入手した！！";
                        messageBox.SetActive(true);
                        yield return new WaitForSeconds(1);
                        messageBox.SetActive(false);
                        break;
                }

                mapManager.TakeItem(item);
                yield return null;
            }

            observer.OnNext(exists);
            observer.OnCompleted();
        }

        private IEnumerator CoroutineBattle(IObserver<bool> observer, Block block, bool takedItem)
        {
            bool onTriggerBattle = false;

            // ボス戦
            if (takedItem && OnTriggerOnBossBattleEvent())
            {
                onTriggerBattle = true;
                dungeonManager.dungeonData.SetBattleType(block.blockType);
                dungeonManager.dungeonData.Save();
                yield return new WaitForSeconds(0.5f);
                Application.LoadLevel("Battle");
            }
            // 通常
            else if (OnTriggerOnBattleEvent(block))
            {
                onTriggerBattle = true;
                dungeonManager.dungeonData.SetBattleType(block.blockType);
                dungeonManager.dungeonData.Save();
                yield return new WaitForSeconds(0.5f);
                Application.LoadLevel("Battle");
            }

            observer.OnNext(onTriggerBattle);
            observer.OnCompleted();
            yield break;
        }

        private IEnumerator CoroutineTakePower(Block block)
        {
            dungeonManager.EnterState(DungeonState.StockTaking);

            if (block.blockType == BlockType.Recovery)
            {
                eventAnimators[0].SetBool("visible", true);
                eventAnimators[0].SetTrigger("logo2");
                yield return new WaitForSeconds(1);

                eventAnimators[0].SetBool("visible", false);
                messageBoxText.text = "ＨＰ回復！！";
                messageBox.SetActive(true);
                yield return new WaitForSeconds(1);

                //  paramater.hp += 1;
                messageBox.SetActive(false);
            }

            block.TakeStock();
            dungeonManager.ExitState();
        }

        private IEnumerator CoroutineCheckRemain()
        {
            if (!RemainsSp())
            {
                Debug.Log("Leave Dungeon!!");
            }

            yield return null;
        }

        private bool OnTriggerOnBossBattleEvent()
        {
            return parameterManager.parameter.getKeyNum == parameterManager.parameter.allKeyNum;
        }

        private bool OnTriggerOnBattleEvent(Block block)
        {
            if (block.blockType == BlockType.None ||
                block.blockType == BlockType.Recovery)
            {
                return false;
            }

            return UnityEngine.Random.value < 1f;
        }

        private bool RemainsSp()
        {
            return parameterManager.parameter.sp > 0;
        }

        public void ReturnFromBattle()
        {
            Block block = mapManager.GetBlock(player.location);

            var takePower = Observable.FromCoroutine(() => CoroutineTakePower(block));
            var checkRemain = Observable.FromCoroutine(CoroutineCheckRemain);

            takePower
                .SelectMany(checkRemain)
                .Subscribe();
        }
    }
}