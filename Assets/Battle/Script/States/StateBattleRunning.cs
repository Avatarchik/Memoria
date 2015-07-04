using Memoria.Battle.Managers;
using UnityEngine;

namespace Memoria.Battle.States
{
    public class StateBattleRunning  : BattleState
    {
        override public void Initialize()
        {
            battleMgr.SetCurrentActor();
            nowActor = battleMgr.NowActor;
        }
        override public void Update()
        {
            if(nowActor.Attack (nowActor.attackType)){
                nowActor.EndTurn();
                EventMgr.Instance.OnTurnEnd();
                Initialized = false;
            }
        }
    }
}