using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleEvent : BlockEvent
{
	public BlockType battleType { get; private set; }

	public BattleEvent(BlockType battleType, Animator[] eventAniamtors, GameObject messageBox, Text messageBoxText)
		: base(eventAniamtors, messageBox, messageBoxText)
	{

		this.battleType = battleType;
	}

	public override IEnumerator GetEventCoroutine(DungeonParameter paramater)
	{
		DungeonManager dungeonManager = DungeonManager.instance;
		dungeonManager.dungeonData.SetBattleType(battleType);
		dungeonManager.dungeonData.Save();
		yield return new WaitForSeconds(0.5f);
		Application.LoadLevel("Battle");
		yield return null;
	}
}