using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class AmeliaUltimate : AttackType
	{
		//CT無しで発動可能な便利技
		void Start ()
		{
			phaseCost = 0;
			cutIn = 0;
			stockCost = 3;
			animationDur = 210;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementThunder(Element.THUNDER);
			effectObj = (GameObject)Resources.Load("Skills/Amelia_SP");
			parameters.attackPower = 1.7f;
            ultimate = true;
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
			particleEffect.transform.position = new Vector3 (0, 0, 0);
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}
