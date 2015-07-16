using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Memoria.Dungeon.BlockEvents
{
    public class RecoveryEvent : BlockEvent
    {
        public RecoveryEvent(Animator[] eventAnimators, GameObject messageBox, Text messageBoxText)
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
            messageBoxText.text = "ＨＰ回復！！";
            messageBox.SetActive(true);
            yield return new WaitForSeconds(1);

            paramater.hp += 1;
            messageBox.SetActive(false);
        }
    }
}