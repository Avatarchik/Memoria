﻿using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class IskaElement : AttackType
	{
		//CTがかかるが全体攻撃
		void Start ()
		{
			phaseCost = 1;
			stockCost = 1;
			animationDur = 210;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementFire(Element.FIRE);
			effectObj = (GameObject)Resources.Load("Skills/Iska_S2");
			parameters.attackPower = 1.2f;
            spriteData = new SpriteData("21");
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
			damage.DamageParameters = parameters;
            foreach(var t in BattleMgr.Instance.enemyList)
            {
                t.GetComponent<IDamageable>().TakeDamage(damage);
            }
		}
		
		override public void PlayEffect (Entity target)
		{
			particleEffect = Instantiate (effectObj);
			particleEffect.transform.position = new Vector3 (0, 0.3f, 2);
//            particleEffect.GetComponentInChildren<ParticleRenderer>().sortingLayerName = "Foreground";
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}