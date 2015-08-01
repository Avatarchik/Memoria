using UnityEngine;

namespace Memoria.Battle.GameActors
{
    abstract public class AttackType : MonoBehaviour {

        protected int animationDur;

        public ElementType elementalAff;
        public TargetType selectType;
        public DmgParameters parameters;

        public GameObject particleEffect;
        public GameObject effectObj;
        public GameObject normalEffect;

        public int phaseCost;
        public int stockCost;
        public char targetType; // used to decide who gets selectable. e = enemy, h = self

        public bool attacked { get; set; }
        public bool useStock { get; private set; } //removed
        public int AttackTime {
            get {
                return animationDur;
            }
        }

        abstract public void Execute(Damage damage, IDamageable target);
        abstract public void PlayEffect(Entity target);
    }
}