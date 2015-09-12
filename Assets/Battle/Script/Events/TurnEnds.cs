using Memoria.Battle.Managers;

namespace Memoria.Battle.Events
{
    public class TurnEnds : GameEvent
    {
        public bool monsterDied;
        public TurnEnds(bool monsterDie)
        {
            this.monsterDied = monsterDie;
        }
    }
}