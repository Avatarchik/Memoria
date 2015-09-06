using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Aria_S2 : AttackType
	{
        bool destroyed;
		//唯一の回復スキル
		void Start ()
		{
			phaseCost = 2;
			stockCost = 1;
			animationDur = 250;
			targetType = 'h';
			selectType = TargetType.ALL;
			elementalAff = new ElementWater(Element.WATER);
			effectObj = (GameObject)Resources.Load("Skills/Aria_S2");
			parameters.attackPower = -1;
            descriptionSprite = "skill_info_11";
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
			damage.DamageParameters = parameters;
			target.TakeDamage(damage);
            destroyed = false;
		}
		
		override public void PlayEffect (Entity target)
		{
            if(!particleEffect && !destroyed)
            {                
                particleEffect = Instantiate (effectObj);
                particleEffect.transform.position = new Vector3 (0, -2, 2);
                particleEffect.GetComponent<ParticleRenderer>().sortingLayerName = "Foreground";
                particleEffect.GetComponentInChildren<ParticleRenderer>().sortingLayerName = "Foreground";
                DestroyObject(particleEffect, 4.0f);
                destroyed = true;
            }
        }
	}
}
