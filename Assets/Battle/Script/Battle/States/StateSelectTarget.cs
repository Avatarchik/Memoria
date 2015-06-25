using UnityEngine;
using System.Collections;
using Memoria.Battle.GameActors;
using Memoria.Battle.Managers;

namespace Memoria.Battle.States
{
    public class StateSelectTarget : BattleState
    {
        Hero hero;
        override public void Initialize()
        {
            hero = (Hero)nowActor;
            if(!hero.passtToStock)
            {
                SetSelectable(nowActor.attackType.targetType, true);
            }
        }
        override public void Update()
        {
            if(hero.passtToStock)
            {
                battleMgr.SetState(State.RUNNING);
            }
            if(hero.EnemySelected()) {
                hero.SetTarget((IDamageable)hero.GetComponent<TargetSelector>().target);
                SetSelectable(nowActor.attackType.targetType, false);
                battleMgr.SetState(State.ANIMATOIN);
            }
        }

        private void SetSelectable(char c, bool state)
        {
            if(hero.passtToStock)
                return;
            foreach(var actor in BattleMgr.actorList)
            {
                var e = actor.GetComponent<Entity>();
                if (e.battleID.ToLowerInvariant().IndexOf(c) != -1)
                {
                    e.GetComponent<BoxCollider2D>().enabled = state;
                }
            }
        }
    }
}