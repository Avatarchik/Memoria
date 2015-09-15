
namespace Memoria.Battle.GameActors
{
    public class WaterSlime : EnemyAI
    {
        //スライム青
        void Awake ()
        {
            nameplate = "Namebar_WaterSlime";
            nameplateId = 8;
            parameter.attack = 870;
            parameter.defense = 235;
            parameter.speed = 163;
            parameter.hp = 1000;

            parameter.elementAff = new ElementWater(Element.WATER);

            attackList.Add ("Enemy_Normal", gameObject.AddComponent<EnemyNormal>());
            attackList.Add ("Enemy_Special", gameObject.AddComponent<EnemyWater>());
            attackType = attackList["Enemy_Normal"];
            attackType = attackList["Enemy_Special"];
        }
    }
}