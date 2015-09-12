
namespace Memoria.Battle.GameActors
{
    public class WaterBoss : EnemyAI
    {
        //水ボス
        void Awake ()
        {
            nameplate = "Namebar_WaterBoss";
            nameplateId = 4;
            parameter.attack = 936;
            parameter.defense = 287;
            parameter.speed = 189;
            parameter.hp = 5000;

            parameter.elementAff = new ElementWater(Element.WATER);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}