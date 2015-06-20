using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleEvent : BlockEvent
{
	public BattleEvent(Animator[] eventAniamtors, GameObject messageBox, Text messageBoxText)
		: base(eventAniamtors, messageBox, messageBoxText)
	{
	}

	public override IEnumerator GetEventCoroutine(DungeonParameter paramater)
	{
		// TODO : データのバックアップ
		DungeonManager.instance.dungeonData.Save();
		yield return new WaitForSeconds(0.5f);
		Application.LoadLevel("Battle");
		yield return null;
	}
}