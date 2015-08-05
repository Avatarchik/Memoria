﻿using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Iska_SP : AttackType
	{
		//CT最長、威力最大の単体必殺技
		void Start ()
		{
			phaseCost = 3;
			stockCost = 3;
			animationDur = 210;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementFire(Element.FIRE);
			effectObj = (GameObject)Resources.Load("Skills/Iska_SP");
			parameters.attackPower = -1;
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
			damage.DamageParameters = parameters;
			target.TakeDamage(damage);
		}
		
		override public void PlayEffect (Entity target)
		{
			particleEffect = Instantiate (effectObj);
			particleEffect.transform.position = new Vector3 (0, -2, -9);
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}