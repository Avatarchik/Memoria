using UnityEngine;
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
            uiMgr.SpawnNamebars(attackTracker.attackOrder);
        }

        override public void Update()
        {
            _timeBeforeStart--;
            if((_timeBeforeStart) <= 0)
            {
                _timeBeforeStart = 120;
                battleMgr.SetState(State.RUNNING);
            }
        }

        public void GenerateOrderIndex()
        {
            foreach (GameObject go in BattleMgr.actorList) {
                Entity actor = go.GetComponent<Entity>();
                actor.orderIndex = _orderIndex;
                if(!attackTracker.attackOrder.ContainsKey(actor)) {
                    attackTracker.attackOrder.Add(actor, _orderIndex);
                }
                _orderIndex++;
            }
        }
    }
}