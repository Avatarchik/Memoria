
namespace Memoria.Battle.GameActors
{
    public class WaterBoss : EnemyAI
    {
        //水ボス
        void Awake ()
        {
            nameplate = "Namebar_WaterBoss";
            nameplateId = 4;
            parameter.attack = 8955;
            parameter.defense = 2169;
//          parameter.mattack = 10;
//          parameter.mdefense = 10;
            parameter.speed = 2468;
            parameter.hp = 52000;

            parameter.elementAff = new ElementWater(Element.WATER);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}