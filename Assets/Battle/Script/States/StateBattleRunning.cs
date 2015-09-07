using UnityEngine;
using System.Collections;
using Memoria.Battle.Managers;
using Memoria.Battle.Events;

namespace Memoria.Battle.States
{
    public class StateBattleRunning  : BattleState
    {
        float pauseBetween;

        override public void Initialize()
        {
            battleMgr.SetCurrentActor();
            nowActor = attackTracker.nowActor;
        }
        override public void Update()
        {
            pauseBetween--;
            if(!battleMgr.BattleOver() && !battleMgr.StateResult() && pauseBetween <= 0)
            {
                if(nowActor.Attack (nowActor.attackType))
                {
                    pauseBetween = 60;
                    nowActor.EndTurn();
                    EventMgr.Instance.Raise(new TurnEnds(false));
                    Initialized = false;
                }
            }
        }
    }
}