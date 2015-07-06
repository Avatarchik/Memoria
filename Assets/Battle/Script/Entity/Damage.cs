using UnityEngine;

namespace Memoria.Battle.GameActors
{
    public interface IDamageable
    {
        void TakeDamage(Damage d);
        bool IsAlive();
    }

    public struct DmgParameters
    {
        public int totalDamage;
        public int percentMod;
        public int attackPower;
        public int defencePower;
        public int offElementalPower;
        public int defElementalPower;

    }

    public class Damage : ScriptableObject {

        public Parameter PlayerParameters { get; set;}
        public Parameter TargetParameters { get; set;}
        public DmgParameters DamageParameters { get; set; }


        public int Calculate()
        {
            return 10;
        }
    }
}