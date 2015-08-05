using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Aria_SP : AttackType
	{
		//唯一の水属性で威力も高い
		void Start ()
		{
			phaseCost = 1;
			stockCost = 3;
			animationDur = 210;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementWater(Element.WATER);
			effectObj = (GameObject)Resources.Load("Skills/Aria_SP");
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
