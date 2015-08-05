using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Amelia_SP : AttackType
	{
		//CT無しで発動可能な便利技
		void Start ()
		{
			phaseCost = 0;
			stockCost = 3;
			animationDur = 210;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementThunder(Element.THUNDER);
			effectObj = (GameObject)Resources.Load("Skills/Amelia_SP");
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
