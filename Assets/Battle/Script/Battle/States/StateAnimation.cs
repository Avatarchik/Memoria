using Memoria.Battle.GameActors;

namespace Memoria.Battle.States
{
    public class StateAnimation : BattleState
    {
        override public void Initialize()
        {
            battleMgr._attackAnimation = (float)(nowActor.attackType.AttackTime / 60);
        }
        override public void Update()
        {
            if(nowActor.chargeReady) {
                nowActor.attackType.PlayEffect((Entity)nowActor.target);
            }
            battleMgr.SetState(State.RUNNING);
        }
    }
}