using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Aria_S2 : AttackType
	{
		//唯一の回復スキル
		void Start ()
		{
			phaseCost = 2;
			stockCost = 1;
			animationDur = 210;
			targetType = 'h';
			selectType = TargetType.ALL;
			elementalAff = new ElementWater(Element.WATER);
			effectObj = (GameObject)Resources.Load("Skills/Aria_S2");
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
			particleEffect.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y -0.3f, -9);
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}
