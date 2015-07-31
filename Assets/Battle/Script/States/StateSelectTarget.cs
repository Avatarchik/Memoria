using UnityEngine;
using System.Linq;
using Memoria.Battle.GameActors;
using Memoria.Battle.Managers;
using Memoria.Battle.Events;

namespace Memoria.Battle.States
{
    public class StateSelectTarget : BattleState
    {
        Hero hero;
        float _timer;
        CancelButton _cancelButton;

        override public void Initialize()
        {
            hero = (Hero)nowActor;
            _cancelButton = GameObject.FindObjectOfType<CancelButton>();
            if(!hero.passToStock)
            {
                _cancelButton.Visible = true;
                SetSelectable(nowActor.attackType.targetType, true);
            }

            if(hero.target == null)
            {
                uiMgr.SpawnDescription("description_frame");
                foreach(var actor in battleMgr.actorList.Where(x => x.GetComponent<BoxCollider2D>().enabled))
                {
                    uiMgr.SpawnCursor(actor.GetComponent<Entity>().battleID, actor);
                }
            }

            EventMgr.Instance.AddListener<CancelSkill>(Cancel);
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
                EventMgr.Instance.RemoveListener<CancelSkill>(Cancel);
                hero.SetTarget((IDamageable)hero.GetComponent<TargetSelector>().target);
                uiMgr.SetCurorAnimation(hero.attackType.selectType, hero.target.ToString());
                uiMgr.DestroyElement("frame");
                _cancelButton.Visible = false;
                SetSelectable(nowActor.attackType.targetType, false);

                _timer++;
                if(_timer > 20)
                {
                    foreach(var actor in battleMgr.actorList)
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
            foreach(var actor in battleMgr.actorList)
            {
                var e = actor.GetComponent<Entity>();
                if (e.battleID.ToLowerInvariant().IndexOf(c) != -1) {
                    e.GetComponent<BoxCollider2D>().enabled = state;
                }
            }
        }

        private void Cancel(CancelSkill gameEvent)
        {
            SetSelectable(nowActor.attackType.targetType, false);
            uiMgr.DestroyElement("frame");

            foreach(var actor in battleMgr.actorList)
            {
                uiMgr.DestroyElement("cursor_" + actor.GetComponent<Entity>().battleID);
            }

            gameEvent.actingHero.attackSelected = false;
            gameEvent.actingHero.attackReady = false;
            gameEvent.actingHero.attackType = null;
            gameEvent.actingHero.charge = false;

            EventMgr.Instance.RemoveListener<CancelSkill>(Cancel);
            battleMgr.SetState(State.SELECT_SKILL);
        }
    }
}