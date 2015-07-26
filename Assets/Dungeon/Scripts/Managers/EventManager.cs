using UnityEngine;
using UnityEngine.UI;
//  using System;
//  using System.Collections;
//  using System.Collections.Generic;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.BlockEvents;
//  using Memoria.Dungeon.Items;
using UniRx;
//  using UniRx.Triggers;

namespace Memoria.Dungeon.Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager instance { get { return DungeonManager.instance.eventManager; } }

        private MapManager mapManager;
        private Player player;

        [SerializeField]
        private Animator eventAnimator;

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
        private PowerTakeEvent powerTakeEvent;
        private SpRemainCheckEvent spRemainCheckEvent;

        void Awake()
        {
            mapManager = MapManager.instance;
            player = DungeonManager.instance.player;

            player.OnWalkEndAsObservable()
                .Subscribe(_ => OnArrivePlayer());

            messageBoxText = messageBox.GetComponentInChildren<Text>();
            messageBox.SetActive(false);

            itemTakeEvent = new ItemTakeEvent(this, eventAnimator);
            battleEvent = new BattleEvent(this, eventAnimator);
            powerTakeEvent = new PowerTakeEvent(this, eventAnimator);
            spRemainCheckEvent = new SpRemainCheckEvent(this, eventAnimator);
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

            var takeItem = itemTakeEvent.CreateTakeItemAsObservable(player.location);
            var battle = battleEvent.CreateBattleEventAsObservable(block);
            var takePower = powerTakeEvent.CreateTakePowerAsObservable(block);
            //  var checkRemain = Observable.FromCoroutine(CoroutineCheckRemain);
            var checkSpRemain = spRemainCheckEvent.CreateCheckSpRemainAsObservable();

            takeItem.Last()
                .SelectMany(battle).Last()
                .Where(onTriggerBattle => !onTriggerBattle)
                .SelectMany(takePower)
                .SelectMany(checkSpRemain)
                .Subscribe();
        }

        #region Remain

        //  private IEnumerator CoroutineCheckRemain()
        //  {
        //      if (!RemainsSp())
        //      {
        //          Debug.Log("Leave Dungeon!!");
        //      }

        //      yield return null;
        //  }

        //  private bool RemainsSp()
        //  {
        //      return ParameterManager.instance.parameter.sp > 0;
        //  }

        #endregion

        public void ReturnFromBattle()
        {
            Block block = mapManager.GetBlock(player.location);

            var takePower = powerTakeEvent.CreateTakePowerAsObservable(block);
            //  var checkRemain = Observable.FromCoroutine(CoroutineCheckRemain);
            var checkSpRemain = spRemainCheckEvent.CreateCheckSpRemainAsObservable();

            takePower
                .SelectMany(checkSpRemain)
                .Subscribe();
        }

        public void ShowMessageBox(bool visible)
        {
            messageBox.SetActive(visible);
        }
    }
}