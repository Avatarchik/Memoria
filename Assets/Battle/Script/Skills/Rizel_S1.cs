using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Rizel_S1 : AttackType
	{
		//アリア通常攻撃
		void Start ()
		{
			phaseCost = 0;
			stockCost = 0;
			animationDur = 210;
			targetType = 'e';
			selectType = TargetType.SINGLE;
			elementalAff = new ElementWater(Element.WATER);
			effectObj = (GameObject)Resources.Load("Skills/Rizel_S1");
			parameters.attackPower = 1;
            spriteData = new SpriteData("10");
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
			damage.DamageParameters = parameters;
			target.TakeDamage(damage);
		}
		
		override public void PlayEffect (Entity target)
		{
			particleEffect = Instantiate (effectObj);
			particleEffect.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y -0.3f, 2);
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}
