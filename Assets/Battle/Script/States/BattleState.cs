using Memoria.Battle.Managers;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.States
{
    abstract public class BattleState
    {
        protected BattleMgr battleMgr;
        protected  UIMgr uiMgr;
        protected Entity nowActor;
        protected AttackTracker attackTracker;
        public bool Initialized { get; set; }
        public bool Run { get; set; }

        public void PreInitialize(BattleMgr bMgr, UIMgr uMgr, Entity nActor, AttackTracker aTracker)
        {
            uiMgr = uMgr;
            battleMgr = bMgr;
            nowActor = nActor;
            attackTracker = aTracker;
        }

        public void EndState()
        {
            Initialized = false;
        }

        abstract public void Initialize();
        abstract public void Update();
    }
}