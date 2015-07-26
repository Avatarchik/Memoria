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

        public Animator eventAnimator { get { return eventAnimators[0]; } }

        [SerializeField]
        private GameObject messageBox;

        private Text messageBoxText;

        public string message
        {
            get { return messageBoxText.text; }
            set { messageBoxText.text = value; }
        }
        
        private ItemTakeEvent itemTakeEvent;
        private BattleEvent battleEvent;

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
            
            itemTakeEvent = new ItemTakeEvent(this, eventAnimator);
            battleEvent = new BattleEvent(this, eventAnimator);
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

            //  var takeItem = Observable.FromCoroutine<bool>(observer => CoroutineTakeItem(observer, player.location));
            //  var takeItem = ItemTakeEvent.CreateTakeItemAsObservable();
            //  Func<bool, IObservable<bool>> battle =
            //       (bool taked) => Observable.FromCoroutine<bool>(observer => CoroutineBattle(observer, block, taked));
            var takeItem = itemTakeEvent.CreateTakeItemAsObservable(player.location);
            var battle = battleEvent.CreateBattleEventAsObservable(block);
            var takePower = Observable.FromCoroutine(() => CoroutineTakePower(block));
            var checkRemain = Observable.FromCoroutine(CoroutineCheckRemain);

            takeItem.Last()
                .SelectMany(battle).Last()
                .Where(onTriggerBattle => !onTriggerBattle)
                .SelectMany(takePower)
                .SelectMany(checkRemain)
                .Subscribe();
        }

        #region Item

        //  private IEnumerator CoroutineTakeItem(IObserver<bool> observer, Vector2Int location)
        //  {
        //      bool exists = mapManager.ExistsItem(location);

        //      if (exists)
        //      {
        //          Item item = mapManager.GetItem(location);

        //          switch (item.itemData.type)
        //          {
        //              case ItemType.Key:
        //                  yield return StartCoroutine(ItemTakeEvent.CoroutineTakeKey(eventAnimator));
        //                  break;

        //              case ItemType.Jewel:
        //                  yield return StartCoroutine(ItemTakeEvent.CoroutineTakeJewel(eventAnimator, item.itemData.attribute));
        //                  break;
        //          }

        //          mapManager.TakeItem(item);
        //          yield return null;
        //      }

        //      observer.OnNext(exists);
        //      observer.OnCompleted();
        //  }

        //  private IEnumerator CoroutineTakeKey(Animator eventAnimator)
        //  {
        //      eventAnimator.SetFloat("itemType", 0);

        //      messageBoxText.text = "鍵を入手した！！";
        //      messageBox.SetActive(true);
        //      yield return new WaitForSeconds(1);
        //      messageBox.SetActive(false);
        //  }

        //  private IEnumerator CoroutineTakeJewel(Animator eventAnimator, BlockType attribute)
        //  {
        //      eventAnimator.SetFloat("itemType", 1);

        //      switch (attribute)
        //      {
        //          case BlockType.Thunder:
        //              eventAnimator.SetFloat("attribute", 0);
        //              break;

        //          case BlockType.Water:
        //              eventAnimator.SetFloat("attribute", 1);
        //              break;

        //          case BlockType.Fire:
        //              eventAnimator.SetFloat("attribute", 2);
        //              break;

        //          case BlockType.Wind:
        //              eventAnimator.SetFloat("attribute", 3);
        //              break;
        //      }

        //      messageBoxText.text = "宝石を入手した！！";
        //      eventAnimators[0].SetTrigger("getJewel");
        //      yield return new WaitForSeconds(1.5f);
        //  }

        #endregion
        #region Battle

        //  private IEnumerator CoroutineBattle(IObserver<bool> observer, Block block, bool takedItem)
        //  {
        //      bool onTriggerBattle = false;

        //      // ボス戦
        //      if (takedItem && OnTriggerOnBossBattleEvent())
        //      {
        //          onTriggerBattle = true;
        //          Debug.Log("ボス戦");
        //          yield return StartCoroutine(CoroutineBattleToBoss(block));
        //      }
        //      // 通常
        //      else if (OnTriggerOnBattleEvent(block))
        //      {
        //          onTriggerBattle = true;
        //          yield return StartCoroutine(CoroutineBattleToEnemy(block));
        //      }

        //      observer.OnNext(onTriggerBattle);
        //      observer.OnCompleted();
        //      yield break;
        //  }

        //  private bool OnTriggerOnBossBattleEvent()
        //  {
        //      return parameterManager.parameter.getKeyNum == parameterManager.parameter.allKeyNum;
        //  }

        //  private bool OnTriggerOnBattleEvent(Block block)
        //  {
        //      switch (block.blockType)
        //      {
        //          case BlockType.None:
        //          case BlockType.Recovery:
        //              return false;
        //      }

        //      return UnityEngine.Random.value < 0.3f;
        //  }

        //  private IEnumerator CoroutineBattleToBoss(Block block)
        //  {
        //      // TODO : blockType == BlockType.Recoveryの時の対処
        //      dungeonManager.dungeonData.SetBattleType(block.blockType);
        //      dungeonManager.dungeonData.Save();
        //      yield return new WaitForSeconds(0.5f);
        //      Application.LoadLevel("Battle");
        //  }

        //  private IEnumerator CoroutineBattleToEnemy(Block block)
        //  {
        //      dungeonManager.dungeonData.SetBattleType(block.blockType);
        //      dungeonManager.dungeonData.Save();
        //      yield return new WaitForSeconds(0.5f);
        //      Application.LoadLevel("Battle");
        //  }

        #endregion
        #region Power
        private IEnumerator CoroutineTakePower(Block block)
        {
            dungeonManager.EnterState(DungeonState.StockTaking);

            switch (block.blockType)
            {
                case BlockType.Thunder:
                case BlockType.Water:
                case BlockType.Fire:
                case BlockType.Wind:
                    yield return StartCoroutine(CoroutineTakePowerTypeOfElements(block.blockType));
                    break;

                case BlockType.Recovery:
                    yield return StartCoroutine(CoroutineTakePowerTypeOfRecovery());
                    break;
            }

            block.TakeStock();
            dungeonManager.ExitState();
        }

        private IEnumerator CoroutineTakePowerTypeOfElements(BlockType attribute)
        {
            yield break;
        }

        private IEnumerator CoroutineTakePowerTypeOfRecovery()
        {
            // TODO : 体力回復
            messageBoxText.text = "ＨＰ回復！！";
            eventAnimators[0].SetTrigger("getPower");
            yield return new WaitForSeconds(1);
        }

        #endregion
        #region Remain
        private IEnumerator CoroutineCheckRemain()
        {
            if (!RemainsSp())
            {
                Debug.Log("Leave Dungeon!!");
            }

            yield return null;
        }


        private bool RemainsSp()
        {
            return parameterManager.parameter.sp > 0;
        }

        #endregion

        public void ReturnFromBattle()
        {
            Block block = mapManager.GetBlock(player.location);

            var takePower = Observable.FromCoroutine(() => CoroutineTakePower(block));
            var checkRemain = Observable.FromCoroutine(CoroutineCheckRemain);

            takePower
                .SelectMany(checkRemain)
                .Subscribe();
        }

        public void ShowMessageBox(bool visible)
        {
            messageBox.SetActive(visible);
        }
    }
}