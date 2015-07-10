using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class FireAttack : AttackType, ITriggerable  {

        void Awake()
        {
            parameters.attackPower = 100;
        }

        void Start ()
        {
            phaseCost = 2;
            stockCost = 1;
            animationDur = 310;
            targetType = 'e';
            selectType = TargetType.ALL;
            elementalAff = new ElementFire(Element.FIRE);
            effectObj = (GameObject)Resources.Load("Skills/explode");
        }

        override public void Execute(Damage damage, IDamageable target)
        {
            damage.DamageParameters = parameters;
            target.TakeDamage(damage);
        }

        override public void PlayEffect (Entity target)
        {
            particleEffect = Instantiate (effectObj);
            particleEffect.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y -0.3f, -3);
            particleEffect.GetComponent<ParticleSystem>().Play();
        }
    }
}