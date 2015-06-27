using UnityEngine;
using Memoria.Battle.GameActors;
using Memoria.Battle.Managers;
using System.Linq;

namespace Memoria.Battle.States
{
    public class StateAnimation : BattleState
    {
        override public void Initialize()
        {
            foreach(var actor in BattleMgr.actorList)
            {
                uiMgr.SetCursor(actor.GetComponent<Entity>().battleID, actor, false);
            }
            uiMgr.RemoveDescBar();
            
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