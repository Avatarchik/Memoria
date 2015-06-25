using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public enum TargetType
    {
        SINGLE,
        ALL
    }

    abstract public class AttackType : MonoBehaviour {

        public TargetType attackType;
        public int phaseCost;
        public int stockCost;
        public ElementType elementalAff;

        protected int animationDur;
        public char targetType; // e = enemy, h = se;f
        public bool attacked { get; set; }
        public bool useStock { get; private set; }
        public GameObject particleEffect;
        public GameObject effectObj;
        public GameObject normalEffect;

        public int AttackTime
        {
            get { return animationDur; }
        }
        abstract public void Execute(Damage dmg, IDamageable target);
        abstract public void Execute(IDamageable target);
        abstract public void PlayEffect(Entity target);
    }
}