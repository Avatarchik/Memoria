using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Iska_S2 : AttackType
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
			parameters.attackPower = -1;
            spriteData = new SpriteData("21");
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
