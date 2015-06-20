using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AcquisitionEvent : BlockEvent
{
    public AcquisitionEvent(Animator[] eventAniamtors, GameObject messageBox, Text messageBoxText)
        : base(eventAniamtors, messageBox, messageBoxText)
    {
    }

    public override IEnumerator GetEventCoroutine(DungeonParameter paramater)
    {
        // TODO : イベントの内容を決定
        eventAnimators[0].SetBool("visible", true);
        eventAnimators[0].SetTrigger("icon1");
        yield return new WaitForSeconds(1);
        
        eventAnimators[0].SetBool("visible", false);        
        messageBoxText.text = "シリングを獲得した！！";
        messageBox.SetActive(true);
        yield return new WaitForSeconds(1);
        
        messageBox.SetActive(false);
        eventAnimators[0].SetBool("visible", true);
        eventAnimators[0].SetTrigger("logo1");
        eventAnimators[1].SetBool("visible", true);        
        eventAnimators[1].SetTrigger("sering");
        yield return new WaitForSeconds(1);

        eventAnimators[0].SetBool("visible", false);
        eventAnimators[1].SetBool("visible", false);
    }
}