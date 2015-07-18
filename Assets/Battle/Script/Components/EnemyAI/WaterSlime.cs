
namespace Memoria.Battle.GameActors
{
    public class WaterSlime : EnemyAI
    {
        //スライム青
        void Awake ()
        {
            nameplate = "Namebar_Golem";

            parameter.attack = 8800;
            parameter.defense = 1935;
//          parameter.mattack = 10;
//          parameter.mdefense = 10;
            parameter.speed = 1638;
            parameter.hp = 10000;

            parameter.elementAff = new ElementWater(Element.WATER);

            attackList.Add ("Enemy_Normal", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}