using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BlockEvent
{
    protected Animator[] eventAnimators { get; private set; }
    protected GameObject messageBox { get; private set; }
    protected Text messageBoxText { get; private set; }

    public BlockEvent(Animator[] eventAnimators, GameObject messageBox, Text messageBoxText)
    {
        this.eventAnimators = eventAnimators;
        this.messageBox = messageBox;
        this.messageBoxText = messageBoxText;
    }

    abstract public IEnumerator GetEventCoroutine(DungeonParameter paramater);
}