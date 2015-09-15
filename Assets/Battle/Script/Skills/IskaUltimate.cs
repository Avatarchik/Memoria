using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class IskaUltimate : AttackType
	{
		//CT最長、威力最大の単体必殺技
		void Start ()
		{
			phaseCost = 3;
            cutIn = 2;
			stockCost = 3;
			animationDur = 550; //550;
			targetType = 'e';
			selectType = TargetType.ALL;
			elementalAff = new ElementFire(Element.FIRE);
			effectObj = (GameObject)Resources.Load("Skills/Iska_SP");
            ultimate = true;
			parameters.attackPower = 2.3f;
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
			particleEffect.transform.position = new Vector3 (0, -2, 0);
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}
