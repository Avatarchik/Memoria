using UnityEngine;
using System.Collections;
using System.Linq;
using Memoria.Battle.Managers;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.States
{
    public class StatePrepare : BattleState
    {
        private int _orderIndex;
        private int _timeBeforeStart;
        override public void Initialize()
        {
            _timeBeforeStart = 120;

            BattleMgr.actorList = BattleMgr.actorList.OrderByDescending (x => x.GetComponent<Entity> ().parameter.speed).ToList ();
            GenerateOrderIndex ();
            uiMgr.SpawnAttackOrder();
            //uiMgr.CreateHpBar();
        }

        override public void Update()
        {
            _timeBeforeStart--;
            if((_timeBeforeStart) <= 0) {
                _timeBeforeStart = 120;
                battleMgr.SetState(State.RUNNING);
            }
        }

        public void GenerateOrderIndex()
        {
            foreach (GameObject go in BattleMgr.actorList) {
                Entity actor = go.GetComponent<Entity>();
                actor.orderIndex = _orderIndex;
                if(!battleMgr.AttackTracker.attackOrder.ContainsKey(actor))
                {
                    battleMgr.AttackTracker.attackOrder.Add(actor, _orderIndex);
                }
                _orderIndex++;
            }
        }
    }
}