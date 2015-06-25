using Memoria.Battle.Managers;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.States
{

    public enum State
    {
        PREPARE = 0,
        RUNNING = 1,
        SELECT_TARGET = 2,
        SELECT_SKILL = 3,
        ANIMATOIN = 4,
        PLAYER_WON = 5
    }

    abstract public class BattleState
    {
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