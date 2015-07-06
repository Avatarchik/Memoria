using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class PlayerStrike : AttackType, ITriggerable
    {

        void Start ()
        {
            phaseCost = 1;
            stockCost = 0;
            animationDur = 210;
            targetType = 'h';
            selectType = TargetType.ALL;
            elementalAff = new ElementThunder(Element.THUNDER);
            effectObj = (GameObject)Resources.Load("explode2");
        }

        void Update () {

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

