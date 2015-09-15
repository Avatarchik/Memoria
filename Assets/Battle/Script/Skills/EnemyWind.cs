using UnityEngine;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
	public class EnemyWind : AttackType {
		bool destroyed;
		void Start () {
			animationDur = 20;
			effectObj = (GameObject)Resources.Load("Skills/Enemy_WIND");
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
            particleEffect.transform.position = new Vector3 (0, 0, 0);
            particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}