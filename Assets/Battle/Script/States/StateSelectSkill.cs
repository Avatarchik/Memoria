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
        }
        override public void Update()
        {
            uiMgr.ShowSkill(hero);
            hero.CheckIfhit();
            if(hero.attackSelected || hero.passToStock)
            {
                hero.GetComponent<BoxCollider2D>().enabled = false;
                uiMgr.DestroyButton();
                battleMgr.SetState(State.SELECT_TARGET);
            }
        }
    }
}