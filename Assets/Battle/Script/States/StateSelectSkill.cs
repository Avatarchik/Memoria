using UnityEngine;
using System.Collections;
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
        }
        override public void Update()
        {
            uiMgr.ShowSkill(hero);
            if(hero.attackSelected || hero.passToStock)
            {
                uiMgr.DestroyButton();
                battleMgr.SetState(State.SELECT_TARGET);
            }
        }
    }
}