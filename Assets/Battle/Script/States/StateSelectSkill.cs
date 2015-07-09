using UnityEngine;
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
        }
        override public void Update()
        {
            hero.CheckIfhit();
            if(hero.attackSelected || hero.passToStock)
            {
                hero.GetComponent<BoxCollider2D>().enabled = false;
                uiMgr.DestroyElement("skill");
                battleMgr.SetState(State.SELECT_TARGET);
            }
        }
    }
}