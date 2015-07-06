using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class QuickAttack : AttackType, ITriggerable {

        void Start () {
            animationDur = 7;
            effectObj = (GameObject)Resources.Load("dmg");
            elementalAff = new NoElement(Element.NONE);
        }

        override public void Execute(Damage damage, IDamageable target)
        {
            damage.DamageParameters = parameters;
            target.TakeDamage(damage);
        }

        override public void PlayEffect (Entity target)
        {
            if (!normalEffect)
            {
                normalEffect = Instantiate (effectObj) as GameObject;
                normalEffect.transform.position = new Vector3 (0, 0, 0);
                Destroy (normalEffect, 0.20f);
            }
        }
    }
}