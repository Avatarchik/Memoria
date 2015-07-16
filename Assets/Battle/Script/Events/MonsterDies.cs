using Memoria.Battle.Managers;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Events
{
    public class MonsterDies : GameEvent
    {
        public Entity killedEntity;

        public MonsterDies(Entity e)
        {
            this.killedEntity = e;
        }
    }
}