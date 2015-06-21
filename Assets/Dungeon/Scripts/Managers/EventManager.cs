using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
		dungeonManager.changedDungeonState += HandleChangedDungeonState;
		messageBoxText = messageBox.GetComponentInChildren<Text>();
		messageBox.SetActive(false);

		eventCoroutineTable = new Dictionary<BlockType, BlockEvent>()
		{            
			{ BlockType.Recovery,   new RecoveryEvent(eventAnimators, messageBox, messageBoxText) },
		};
	}

	private void HandleChangedDungeonState(object sender, ChangeDungeonStateEventArgs e)
	{
		if (e.nowState == DungeonState.PlayerMoving && e.nextState == DungeonState.None)
		{
			if (!mapManager.map.ContainsKey(player.location))
			{
				return;
			}

			Block block = mapManager.map[player.location];

			if (block.type == BlockType.None)
			{
			}

			StartCoroutine(CoroutineBlockEvent(block));
		}
	}

	private IEnumerator CoroutineBlockEvent(Block block)
	{
		DungeonParameter parameter = paramaterManager.parameter;
		parameter.sp -= 1;
		parameter.tp += 1;

		if (!block.hasEvent)
		{
			yield break;
		}

		dungeonManager.EnterState(DungeonState.BlockEvent);
		block.OnEnterBlockEvent();

		//if (eventCoroutineTable.ContainsKey(block.onEventType))
		//{
		//	yield return StartCoroutine(eventCoroutineTable[block.onEventType].GetEventCoroutine(parameter));
		//}

		block.OnExitBlockEvent();
		dungeonManager.ExitState();

		if (parameter.sp <= 0)
		{
			Debug.Log("leave dungeon!!");
		}

		yield break;
	}

	//private BlockType GetRandomBlockType()
	//{
	//	KeyValuePair<BlockType, float>[] typeAndProbabilityTable = new[]
	//	{
	//		new KeyValuePair<BlockType,float>(BlockType.None,		0.2f),
	//		new KeyValuePair<BlockType,float>(BlockType.Recovery,	0.1f),
	//	};

	//	float random = Random.value;
	//	float sum = 0;
	//	BlockType result = BlockType.None;

	//	foreach (var typeAndPropability in typeAndProbabilityTable)
	//	{
	//		sum += typeAndPropability.Value;

	//		if (random < sum)
	//		{
	//			result = typeAndPropability.Key;
	//			break;
	//		}
	//	}

	//	return result;
	//}

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