
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
            parameter.defense = 141;
            parameter.speed = 230;
            parameter.hp = 3500;

			parameter.elementAff = new ElementFire(Element.FIRE);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}