using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Diel_S1 : AttackType
	{
		//ディエル通常攻撃
		void Start ()
		{
			phaseCost = 0;
			stockCost = 0;
			animationDur = 70;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementWind(Element.WIND);
			effectObj = (GameObject)Resources.Load("Skills/Dhiel_S1");
			parameters.attackPower = -1;
            descriptionSprite = "skill_info_30";
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
		}
	}
}
