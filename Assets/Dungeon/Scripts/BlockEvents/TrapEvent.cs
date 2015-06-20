using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrapEvent : BlockEvent
{
    public TrapEvent(Animator[] eventAnimators, GameObject messageBox, Text messageBoxText)
        : base(eventAnimators, messageBox, messageBoxText)
    {        
    }

    public override IEnumerator GetEventCoroutine(DungeonParameter paramater)
    {
        // TODO : イベントの内容を決定
        eventAnimators[0].SetBool("visible", true);
        eventAnimators[0].SetTrigger("icon2");
        yield return new WaitForSeconds(1);

        eventAnimators[0].SetBool("visible", false);        
        messageBoxText.text = "ＨＰ減トラップ";
        messageBox.SetActive(true);
        yield return new WaitForSeconds(1);
        
        messageBox.SetActive(false);
        eventAnimators[0].SetBool("visible", true);        
        eventAnimators[0].SetTrigger("logo3");
        yield return new WaitForSeconds(1);

        paramater.hp -= 1;
        eventAnimators[0].SetBool("visible", false);
    }
}