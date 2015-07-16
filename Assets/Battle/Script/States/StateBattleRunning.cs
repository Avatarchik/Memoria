using Memoria.Battle.Managers;
using Memoria.Battle.Events;

namespace Memoria.Battle.States
{
    public class StateBattleRunning  : BattleState
    {
        override public void Initialize()
        {
            battleMgr.SetCurrentActor();
            nowActor = attackTracker.nowActor;
        }
        override public void Update()
        {
            if(!battleMgr.BattleOver() && !battleMgr.StateResult())
            {
                if(nowActor.Attack (nowActor.attackType))
                {
                    nowActor.EndTurn();
//                    EventMgr.Instance.OnTurnEnd();
                    EventMgr.Instance.Raise(new TurnEnds());
                    Initialized = false;
                }
            }
        }
    }
}