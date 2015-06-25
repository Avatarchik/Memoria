using Memoria.Battle.Managers;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.States
{
    abstract public class BattleState
    {
        public enum State
        {
            PREPARE,
            RUNNING,
            SELECT_TARGET,
            SELECT_SKILL,
            ANIMATOIN,
            PLAYER_WON
        }

        protected BattleMgr battleMgr;
        protected  UIMgr uiMgr;
        protected Entity nowActor;
        public bool Initialized { get; set; }

        public void PreInitialize(BattleMgr bMgr, UIMgr uMgr, Entity nActor)
        {
            uiMgr = uMgr;
            battleMgr = bMgr;
            nowActor = nActor;
        }

        public void EndState()
        {
            Initialized = false;
        }

        abstract public void Initialize();
        abstract public void Update();
    }
}