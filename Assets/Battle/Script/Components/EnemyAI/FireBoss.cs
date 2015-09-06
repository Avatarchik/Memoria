
namespace Memoria.Battle.GameActors
{
	public class FireBoss : EnemyAI
    {
        //火ボス
        void Awake ()
        {
            nameplate = "Namebar_FireBoss";
            nameplateId = 5;
            parameter.attack = 9349;
            parameter.defense = 1614;
//          parameter.mattack = 10;
//          parameter.mdefense = 10;
            parameter.speed = 2468;
            parameter.hp = 50000;

			parameter.elementAff = new ElementFire(Element.FIRE);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}