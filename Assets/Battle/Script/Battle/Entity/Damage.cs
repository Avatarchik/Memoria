using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public interface IDamageable
    {
        void TakeDamage(int i);
        void TakeDamage(Damage d);
        bool IsAlive();
    }

    public class Damage : ITriggerable {

        public int totalDamage { get; set; }
        public int percentMod { get; set; }
        public int attackPower { get; set; }
        public int defencePower { get; set; }
        public bool criticalHit { get; set; }
        public int hitChance { get; set; }
        public int offElementalPower { get; set; }
        public int defElementalPower { get; set; }


        public void Calculate()
        {

        }

        public void TakeDamageEvent()
        {

        }
    }
}