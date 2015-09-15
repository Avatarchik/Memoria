using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class EnemyNormal : AttackType {
        bool destroyed;
        void Start () {
            phaseCost = 0;
            animationDur = 20;
            effectObj = (GameObject)Resources.Load("Skills/Enemy_Normal");
            parameters.attackPower = 1;
        }

        override public void Execute(Damage damage, IDamageable target)
        {
            damage.DamageParameters = parameters;
            target.TakeDamage(damage);
            destroyed = false;
        }

        override public void PlayEffect (Entity target)
        {
            if(!particleEffect && !destroyed)
            {                
                particleEffect = Instantiate (effectObj);
                particleEffect.transform.position = new Vector3 (0,0,0);
                particleEffect.GetComponentInChildren<EllipsoidParticleEmitter>().Emit(3);
                DestroyObject(particleEffect, 0.5f);
                destroyed = true;
            }
        }
    }
}