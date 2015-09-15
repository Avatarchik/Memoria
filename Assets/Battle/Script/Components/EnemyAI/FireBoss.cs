
namespace Memoria.Battle.GameActors
{
	public class FireBoss : EnemyAI
    {
        //火ボス
        void Awake ()
        {
            nameplate = "Namebar_FireBoss";
            nameplateId = 5;
            parameter.attack = 1500;
            parameter.defense = 151;
            parameter.speed = 246;
            parameter.hp = 4000;

			parameter.elementAff = new ElementFire(Element.FIRE);

            attackList.Add ("Enemy_Normal", gameObject.AddComponent<EnemyNormal>());
            attackList.Add ("Enemy_Skill", gameObject.AddComponent<BossSkill>());
            attackList.Add ("Enemy_Ultimate", gameObject.AddComponent<BossFire>());

        }
    }
}