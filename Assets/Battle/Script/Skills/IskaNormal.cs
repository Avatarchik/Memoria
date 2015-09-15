using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class IskaNormal : AttackType
	{
		//イスカ通常攻撃
		void Start ()
		{
			phaseCost = 0;
			stockCost = 0;
			animationDur = 130;
			targetType = 'e';
			selectType = TargetType.SINGLE;
			elementalAff = new ElementFire(Element.FIRE);
			effectObj = (GameObject)Resources.Load("Skills/Iska_S1");
			parameters.attackPower = 1;
            spriteData = new SpriteData("20");
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
			damage.DamageParameters = parameters;
			target.TakeDamage(damage);
		}
		
		override public void PlayEffect (Entity target)
		{
			particleEffect = Instantiate (effectObj);
			particleEffect.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, 0);
//            particleEffect.GetComponentInChildren<ParticleRenderer>().sortingLayerName = "Foreground";
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}
