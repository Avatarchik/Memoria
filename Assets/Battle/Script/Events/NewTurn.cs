using Memoria.Battle.Managers;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Events
{
    public class NewTurn : GameEvent
    {
        public Entity entity;

        public NewTurn(Entity e)
        {
            this.entity = e;
        }
    }
}