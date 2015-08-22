using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Amelia_S1 : AttackType
	{
		//アリア通常攻撃
		void Start ()
		{
			phaseCost = 0;
			stockCost = 0;
			animationDur = 70;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementThunder(Element.THUNDER);
			effectObj = (GameObject)Resources.Load("Skills/Amelia_S1");
			parameters.attackPower = -1;
            descriptionSprite = "skill_info_00";
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
			damage.DamageParameters = parameters;
			target.TakeDamage(damage);
		}
		
		override public void PlayEffect (Entity target)
		{
			particleEffect = Instantiate (effectObj);
			particleEffect.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, -9);
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}
