using UnityEngine;
using Memoria.Battle.GameActors;
using Memoria.Battle.Managers;

namespace Memoria.Battle.States
{
    public class StateAnimation : BattleState
    {
        bool _isPlaying;
        override public void Initialize()
        {
            _isPlaying = false;
            battleMgr.AttackAnimation = (float)(nowActor.attackType.AttackTime / 60);
        }
        override public void Update()
        {
            if(nowActor.chargeReady && !_isPlaying) {
                nowActor.attackType.PlayEffect((Entity)nowActor.target);
                _isPlaying = true;
            }
            battleMgr.SetState(State.RUNNING);
        }
    }
}