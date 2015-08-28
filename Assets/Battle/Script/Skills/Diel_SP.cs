using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class Diel_SP : AttackType
	{
		//威力は低いが全体攻撃
		void Start ()
		{
			phaseCost = 0;
            cutIn = 3;
			stockCost = 3;
			animationDur = 210;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementWind(Element.WIND);
			effectObj = (GameObject)Resources.Load("Skills/Dhiel_SP");
            ultimate = true;
			parameters.attackPower = -1;
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
			damage.DamageParameters = parameters;
            foreach(var t in BattleMgr.Instance.enemyList)
            {
                t.GetComponent<IDamageable>().TakeDamage(damage);
            }
		}
		
		override public void PlayEffect (Entity target)
		{
			particleEffect = Instantiate (effectObj);
			particleEffect.transform.position = new Vector3 (0, 0, -9);
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}
