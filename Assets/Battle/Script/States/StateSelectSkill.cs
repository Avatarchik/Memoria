using UnityEngine;
using UnityEngine.UI;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.States
{
    public class StateSelectSkill: BattleState
    {
        Hero hero;
        override public void Initialize()
        {
            FadeAttackScreen.DeFlash();
            hero = (Hero)nowActor;
            hero.GetComponent<BoxCollider2D>().enabled = true;
            if(!hero.attackSelected || hero.passToStock)
            {
                uiMgr.SpawnSkills(hero);
                //Show cancel button
            }

            Color fadeOutColor;
            fadeOutColor = new Color(1, 1, 1, 0.5f);

            foreach (var entity in attackTracker.attackOrder)
            {
                if(entity.Key != nowActor && !entity.Key.entityType.Equals("enemy"))
                {
                    entity.Key.GetComponent<SpriteRenderer>().color = fadeOutColor;
                }
            }
        }
        override public void Update()
        {
            hero.CheckIfhit();
            if(hero.attackSelected || hero.passToStock)
            {
                foreach (var entity in attackTracker.attackOrder)
                {
                    if(entity.Key != nowActor && !entity.Key.entityType.Equals("enemy"))
                    {
                        entity.Key.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    }
                }
                hero.GetComponent<BoxCollider2D>().enabled = false;
                uiMgr.DestroyElement("skill");
                battleMgr.SetState(State.SELECT_TARGET);
            }
        }
    }
}