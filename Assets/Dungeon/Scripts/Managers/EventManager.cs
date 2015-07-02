using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.BlockUtility;
using Memoria.Dungeon.BlockEvents;
using UniRx;

namespace Memoria.Dungeon.Managers
{
	public class EventManager : MonoBehaviour
	{
		private DungeonManager dungeonManager;
		private MapManager mapManager;
		private ParameterManager paramaterManager;
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
			paramaterManager = dungeonManager.parameterManager;
			player = dungeonManager.player;
//			dungeonManager.changedDungeonState += HandleChangedDungeonState;
			dungeonManager.ActiveStateAsObservable()
			.Buffer(2, 1)
			.Select(states => new 
			{
				current = states.ElementAt(0),
				next = states.ElementAt(1)
			})
			.Where(states => states.current == DungeonState.PlayerMoving && states.next == DungeonState.None)
			.Subscribe(_ =>
			{
				if (!mapManager.map.ContainsKey(player.location))
				{
					return;
				}
							
				Block block = mapManager.map[player.location];
				if (block.type == BlockType.None)
				{
					return;
				}
							
				StartCoroutine(CoroutineBlockEvent(block));
			});
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

		//        private void HandleChangedDungeonState(object sender, ChangeDungeonStateEventArgs e)
		//        {
		//            if (e.nowState == DungeonState.PlayerMoving && e.nextState == DungeonState.None)
		//            {
		//                if (!mapManager.map.ContainsKey(player.location))
		//                {
		//                    return;
		//                }
		//
		//                Block block = mapManager.map [player.location];
		//                if (block.type == BlockType.None)
		//                {
		//                    return;
		//                }
		//
		//                StartCoroutine(CoroutineBlockEvent(block));
		//            }
		//        }

		public void OnTapBlock(Block tappedBlock)
		{
			Debug.Log("tappedBlock : " + tappedBlock);
			onTapBlock.OnNext(tappedBlock);
		}

		private Subject<Block> onTapBlock = new Subject<Block>();
		public IObservable<Block> OnTapBlockAsObservable()
		{
			return onTapBlock.AsObservable();
		}

		private IEnumerator CoroutineBlockEvent(Block block)
		{
			DungeonParameter parameter = paramaterManager.parameter;
			parameter.sp -= 1;
//            parameter.tp += 1;

			if (!block.hasEvent)
			{
				yield break;
			}

			dungeonManager.EnterState(DungeonState.BlockEvent);
			block.OnEnterBlockEvent();

			if (eventCoroutineTable.ContainsKey(block.type))
			{
				yield return StartCoroutine(eventCoroutineTable[block.type].GetEventCoroutine(parameter));
			}

			block.OnExitBlockEvent();
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
			DungeonParameter parameter = paramaterManager.parameter;

			block.OnExitBlockEvent();

			if (parameter.sp <= 0)
			{
				Debug.Log("leave dungeon!!");
			}
		}
	}
}