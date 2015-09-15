using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class BossFire : AttackType {
		bool destroyed;
		void Start () {
            phaseCost = 2;
			animationDur = 20;
			effectObj = (GameObject)Resources.Load("Skills/Enemy_HI");
			parameters.attackPower = 2.0f;
            ultimate = true;
            cutIn = 5;
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
			damage.DamageParameters = parameters;
			target.TakeDamage(damage);
			destroyed = false;
		}
		
		override public void PlayEffect (Entity target)
		{
            particleEffect = Instantiate (effectObj);
            particleEffect.transform.position = new Vector3 (0, 0, 0);
            particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}