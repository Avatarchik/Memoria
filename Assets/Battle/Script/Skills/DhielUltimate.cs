using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class DhielUltimate : AttackType  {

        void Start ()
        {
            phaseCost = 3;
            stockCost = 3;
            animationDur = 310;
            targetType = 'e';
            selectType = TargetType.ALL;
            elementalAff = new ElementFire(Element.WIND);
            effectObj = (GameObject)Resources.Load("Skills/DhielUltimate");
            parameters.attackPower = 100;
        }

        override public void Execute(Damage damage, IDamageable target)
        {
            damage.DamageParameters = parameters;
            target.TakeDamage(damage);
        }

        override public void PlayEffect (Entity target)
        {
            //TODO: Add cut in

            particleEffect = Instantiate (effectObj);
            particleEffect.transform.position = new Vector3 (0, 0, -9);
            particleEffect.GetComponent<ParticleSystem>().Play();
        }
    }
}