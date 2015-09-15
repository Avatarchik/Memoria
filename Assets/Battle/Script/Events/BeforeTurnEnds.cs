using Memoria.Battle.Managers;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Events
{
    public class BeforeTurnEnds : GameEvent
    {
        public Entity actor;

        public BeforeTurnEnds(Entity a)
        {
            this.actor = a;
        }
    }
}