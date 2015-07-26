using Memoria.Battle.Managers;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Events
{
    public class CancelSkill : GameEvent
    {
        public Hero actingHero;

        public CancelSkill(Hero h)
        {
            this.actingHero = h;
        }
    }
}