using UnityEngine;

namespace Memoria.Battle.GameActors
{
	public class Diel_S2 : AttackType
	{
		//攻撃順がすぐに回ってくる強化技
		void Start ()
		{
			phaseCost = 0;
			stockCost = 1;
			animationDur = 210;
			targetType = 'h';
			selectType = TargetType.SELF;
			elementalAff = new ElementWind(Element.WIND);
			effectObj = (GameObject)Resources.Load("Skills/Dhiel_S2");
			parameters.attackPower = -1;
            spriteData = new SpriteData("31");
		}
		
		override public void Execute(Damage damage, IDamageable target)
		{
		}
		
		override public void PlayEffect (Entity target)
		{
            var user = GetComponent<Entity>();
			particleEffect = Instantiate (effectObj);
			particleEffect.transform.position = user.transform.position;
            particleEffect.GetComponent<HasteSkill>().user = user;
			particleEffect.GetComponent<ParticleSystem>().Play();
		}
	}
}
