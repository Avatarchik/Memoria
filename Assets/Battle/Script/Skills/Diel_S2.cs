using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Diel_S2 : AttackType
	{
		//攻撃順がすぐに回ってくる強化技
		void Start ()
		{
			phaseCost = 0;
			stockCost = 1;
			animationDur = 210;
			targetType = 'h';
			selectType = TargetType.ALL;
			elementalAff = new ElementWind(Element.WIND);
			effectObj = (GameObject)Resources.Load("Skills/Dhiel_S2");
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
			particleEffect.transform.position = new Vector3 (0, 0, -9);
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}
