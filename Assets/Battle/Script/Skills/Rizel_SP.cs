using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Rizel_SP : AttackType
	{
        bool destroyed;
		//唯一の水属性で威力も高い
		void Start ()
		{
			phaseCost = 1;
			stockCost = 3;
            cutIn = 1;
			animationDur = 210;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementWater(Element.WATER);
			effectObj = (GameObject)Resources.Load("Skills/Rizelo_SP");
            ultimate = true;
            parameters.attackPower = 2.0f;
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
			damage.DamageParameters = parameters;
            foreach(var t in BattleMgr.Instance.enemyList)
            {
                t.GetComponent<IDamageable>().TakeDamage(damage);
            }
            destroyed = false;
		}
		
		override public void PlayEffect (Entity target)
		{
            if(!particleEffect && !destroyed)
            {                
                particleEffect = Instantiate (effectObj);
                particleEffect.transform.position = new Vector3 (0, 4, 2);
                particleEffect.GetComponentInChildren<ParticleRenderer>().sortingLayerName = "Foreground";
                DestroyObject(particleEffect, 2.1f);
                destroyed = true;
            }
		}
	}
}
