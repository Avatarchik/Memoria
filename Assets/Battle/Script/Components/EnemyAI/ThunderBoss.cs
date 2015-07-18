
namespace Memoria.Battle.GameActors
{
    public class ThunderBoss : EnemyAI
    {
        //雷ボス
        void Awake ()
        {
            nameplate = "Namebar_Golem";

            parameter.attack = 9081;
            parameter.defense = 1861;
//          parameter.mattack = 10;
//          parameter.mdefense = 10;
            parameter.speed = 2468;
            parameter.hp = 49500;

            parameter.elementAff = new ElementThunder(Element.THUNDER);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}