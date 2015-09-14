using Memoria.Battle.Managers;

namespace Memoria.Battle.Events
{
    public class TurnEnds : GameEvent
    {
        public bool monsterDied;
        public int monstersDead;
        public TurnEnds(bool monsterDie, int deadMonster = 0)
        {
            this.monsterDied = monsterDie;
            this.monstersDead = deadMonster;
        }
    }
}