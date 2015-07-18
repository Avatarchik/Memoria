using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.BlockEvents;
using Memoria.Dungeon.Items;
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
            Block block = mapManager.GetBlock(player.location);
            if (block.blockType == BlockType.None)
            {
                return;
            }

			// アイテムの取得
            if (mapManager.ExistsItem(player.location))
            {
                Item item = mapManager.GetItem(player.location);
				
                switch (item.itemData.type)
                {
                    case ItemType.Key:
						OnTakeKey(item);
						
						// ボス戦
						if (OnTriggerOnBossBattleEvent())
						{
							OnBossBattleEvent();
							return;
						}
                        break;

                    case ItemType.Jewel:
                        OnTakeJewel(item);
                        break;

                    case ItemType.Soul:
						OnTakeSoul(item);
                        break;

                    case ItemType.MagicPlate:
						OnTakeMagicPlate(item);
                        break;
                }
            }
			// 戦闘
            else if (OnTriggerOnBattleEvent(block))
            {
				OnBattleEvent(block);
                return;
            }
			
			// ストック取得
			OnTakeStock(block);			
			block.OnBlockEventExit();
			
			// SPチェック
			if (!RemainsSp())
			{
				Debug.Log("Leave Dungeon!");	
			}

            //  StartCoroutine(CoroutineBlockEvent(block, parameterManager.parameter));
        }
		
		private void OnTakeKey(Item key)
		{
			
		}
		
		private bool OnTriggerOnBossBattleEvent()
		{
			return parameterManager.parameter.getKeyNum == parameterManager.parameter.allKeyNum;
		}
		
		private void OnBossBattleEvent()
		{
			
		}
		
		private void OnTakeJewel(Item jewel)
		{
		}
		
		private void OnTakeSoul(Item soul)
		{
		}
		
		private void OnTakeMagicPlate(Item magicPlate)
		{			
		}

        private bool OnTriggerOnBattleEvent(Block block)
        {
            if (block.blockType == BlockType.None ||
                block.blockType == BlockType.Recovery)
            {
				return false;
            }

            return Random.value < 0.2f;
        }

        private void OnBattleEvent(Block block)
        {
            StartCoroutine(CoroutineBlockEvent(block, parameterManager.parameter));
        }
		
		private void OnTakeStock(Block block)
		{
			
		}
		
		private bool RemainsSp()
		{
			return parameterManager.parameter.sp > 0;
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
            Block block = mapManager.GetBlock(player.location);
			
			OnTakeStock(block);
            block.OnBlockEventExit();

            if (!RemainsSp())
            {
                Debug.Log("leave dungeon!!");
            }
        }
    }
}
