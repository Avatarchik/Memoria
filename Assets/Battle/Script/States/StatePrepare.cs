using System.Linq;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.States
{
    public class StatePrepare : BattleState
    {
        private int _timeBeforeStart;
        override public void Initialize()
        {
            _timeBeforeStart = 120;
            battleMgr.actorList = battleMgr.actorList.OrderByDescending (x => x.GetComponent<Entity> ().parameter.speed).ToList ();
            attackTracker.GenerateQueue<Entity>(battleMgr.actorList);
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
    }
}