﻿using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class FireAttack : AttackType, ITriggerable  {

        void Start ()
        {
            phaseCost = 2;
            stockCost = 2;
            animationDur = 310;
            targetType = 'e';
            elementalAff = BattleMgr.ElementType.FIRE;
            effectObj = (GameObject)Resources.Load("explode");
        }

        override public void Execute(IDamageable target) 
        {
            target.TakeDamage(120);
        }

        override public void Execute(Damage damage, IDamageable target)
        {
            //       target.TakeDamage(damage);
        }

    
        override public void PlayEffect (Entity target)
        {
            particleEffect = Instantiate (effectObj);
            particleEffect.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y -0.3f, -3);
            particleEffect.GetComponent<ParticleSystem>().Play();	
        }
    }
}