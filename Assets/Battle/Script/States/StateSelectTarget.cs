using UnityEngine;
using System.Linq;
using Memoria.Battle.GameActors;
using Memoria.Battle.Managers;

namespace Memoria.Battle.States
{
    public class StateSelectTarget : BattleState
    {
        Hero hero;
        float _timer;
        override public void Initialize()
        {
            hero = (Hero)nowActor;

            if(!hero.passToStock)
            {
                SetSelectable(nowActor.attackType.targetType, true);
            }

            if(hero.target == null) {
                uiMgr.ShowDescBar("description_frame");
                foreach(var actor in BattleMgr.actorList.Where(x => x.GetComponent<BoxCollider2D>().enabled))
                {
                    uiMgr.SetCursor(actor.GetComponent<Entity>().battleID, actor);
                }
            }
        }

        override public void Update()
        {

            if(hero.passToStock)
            {
                battleMgr.SetState(State.RUNNING);
                return;
            }

            if(hero.TargetSelected())
            {
                hero.SetTarget((IDamageable)hero.GetComponent<TargetSelector>().target);
                uiMgr.SetCurorAnimation(hero.attackType.selectType, hero.target.ToString());
                uiMgr.DestroyElement("frame");
                SetSelectable(nowActor.attackType.targetType, false);

                _timer++;
                if(_timer > 20)
                {
                    foreach(var actor in BattleMgr.actorList)
                    {
                        uiMgr.DestroyElement("cursor_" + actor.GetComponent<Entity>().battleID);
                    }
                    if(_timer > 40)
                    {
                        _timer = 0;
                        battleMgr.SetState(State.ANIMATOIN);
                    }
                }
            }
        }

        private void SetSelectable(char c, bool state)
        {
            if(hero.passToStock)
                return;
            foreach(var actor in BattleMgr.actorList)
            {
                var e = actor.GetComponent<Entity>();
                if (e.battleID.ToLowerInvariant().IndexOf(c) != -1) {
                    e.GetComponent<BoxCollider2D>().enabled = state;
                }
            }
        }
    }
}