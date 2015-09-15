using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class EnemyWater : AttackType {
		bool destroyed;
		void Start () {
			animationDur = 20;
			effectObj = (GameObject)Resources.Load("Skills/Enemy_MIZU");
			parameters.attackPower = 1.5f;
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
            particleEffect.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, 0);
            particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}