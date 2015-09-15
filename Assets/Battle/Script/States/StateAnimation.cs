using UnityEngine;
using Memoria.Battle.GameActors;
using Memoria.Battle.Managers;

namespace Memoria.Battle.States
{
    public class StateAnimation : BattleState
    {
        bool _isPlaying;
        CutIn cutIn;
        override public void Initialize()
        {
            cutIn = GameObject.Find("cutIn").GetComponent<CutIn>();

            if(nowActor.attackType.ultimate && nowActor.chargeReady)
            {
                cutIn.id = nowActor.attackType.cutIn;
                cutIn.Init();
                cutIn.StartSequence();
            }
            _isPlaying = false;
            battleMgr.AttackAnimation = (float)(nowActor.attackType.AttackTime / 60);
        }
        override public void Update()
        {
            if(cutIn.played)
            {
                if(nowActor.chargeReady && !_isPlaying) {
                    nowActor.attackType.PlayEffect((Entity)nowActor.target);
                    _isPlaying = true;
                }
                battleMgr.SetState(State.RUNNING);
            }
        }
    }
}