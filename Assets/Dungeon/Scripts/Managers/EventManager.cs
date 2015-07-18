using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.BlockEvents;
using UniRx;

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

        private Dictionary<BlockType, BlockEvent> eventCoroutineTable = null;

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

            eventCoroutineTable = new Dictionary<BlockType, BlockEvent>()
            {
                {
                    BlockType.Fire,
                    new BattleEvent(BlockType.Fire, eventAnimators, messageBox, messageBoxText)
                },
                {
                    BlockType.Wind,
                    new BattleEvent(BlockType.Wind, eventAnimators, messageBox, messageBoxText)
                },
                {
                    BlockType.Thunder,
                    new BattleEvent(BlockType.Thunder, eventAnimators, messageBox, messageBoxText)
                },
                {
                    BlockType.Water,
                    new BattleEvent(BlockType.Water, eventAnimators, messageBox, messageBoxText)
                },
                {
                    BlockType.Recovery,
                    new RecoveryEvent(eventAnimators, messageBox, messageBoxText)
                },
            };
        }

        // プレイヤーが歩き終わったときに呼び出される
        public void OnArrivePlayer()
        {
            // プレイヤーがいるブロックを取得
            Block block = mapManager.map[player.location];
            if (block.blockType == BlockType.None)
            {
                return;
            }

            /*
            // @TODO 
			1 キーの確認 
			2 ボス戦の発生
			3 アイテム獲得処理（宝石、精霊の魂、魔石版）
			4 戦闘
			5 ストック
			6 SPチェック
			*/

            /*
			// 1 キーの確認
			if (ExistsKey())
			{
				GetKey();
				if (keyCount == fillKeyCount)
				{
					// 2 ボス戦の発生
					// LoadLevelCalled
					OnBossBattle();
				}
			}
			// 3 アイテムの確認
			else if (ExistsItem())
			{
				GetItem(); 
				// => {
					//  switch(item)
					//  {
					//  case Jewel:
					//  	GetJewel();
					//  case Soul:
					//  	GetSoul();
					//  case Plane:
					//  	GetPlane();
					//  }
				// };
			}
			// 4 戦闘
			else if (OnTriggerOnBattleEvent())
			{
				// LoadLevelCalled
				OnBattleEvent();
				return;
			}
			ReturnFromBattle();
			=> {
				// 5 ストック
				GetStock();
				
				// 6 SPチェック
				CheckSp();
			};
            */

            StartCoroutine(CoroutineBlockEvent(block, parameterManager.parameter));
        }

        private IEnumerator CoroutineBlockEvent(Block block, DungeonParameter parameter)
        {
            dungeonManager.EnterState(DungeonState.BlockEvent);
            block.OnBlockEventEnter();

            if (eventCoroutineTable.ContainsKey(block.blockType))
            {
                yield return StartCoroutine(eventCoroutineTable[block.blockType].GetEventCoroutine(parameter));
            }

            block.OnBlockEventExit();
            dungeonManager.ExitState();

            if (parameter.sp <= 0)
            {
                Debug.Log("leave dungeon!!");
            }

            yield break;
        }

        public void ReturnFromBattle()
        {
            Block block = mapManager.map[player.location];
            var parameter = parameterManager.parameter;

            block.OnBlockEventExit();

            if (parameter.sp <= 0)
            {
                Debug.Log("leave dungeon!!");
            }
        }
    }
}
