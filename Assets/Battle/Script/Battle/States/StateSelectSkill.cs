using UnityEngine;
using System.Collections;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.States
{
    public class StateSelectSkill: BattleState
    {
        override public void Initialize()
        {
            FadeAttackScreen.DeFlash();
        }
        override public void Update()
        {
            var hero = (Hero)nowActor;
            uiMgr.ShowSkill(hero);
            if(hero.attackSelected || hero.passtToStock) {
                uiMgr.DestroyButton();
                battleMgr.SetState(State.SELECT_TARGET);
            }
        }
    }
}