using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubRecoveryEvent : BlockEvent
{
    public SubRecoveryEvent(Animator[] eventAnimators, GameObject messageBox, Text messageBoxText)
        : base(eventAnimators, messageBox, messageBoxText)
    {        
    }

    public override IEnumerator GetEventCoroutine(DungeonParameter paramater)
    {        
        // TODO : イベントの内容を決定
        eventAnimators[0].SetBool("visible", true);
        eventAnimators[0].SetTrigger("logo2");
        yield return new WaitForSeconds(1);

        eventAnimators[0].SetBool("visible", false);        
        messageBoxText.text = "ＳＰ回復！！／探索ｐｔアップ！！";
        messageBox.SetActive(true);
        yield return new WaitForSeconds(1);
        
        paramater.sp += 1;
        messageBox.SetActive(false);
    }
}