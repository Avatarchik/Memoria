
namespace Memoria.Battle.GameActors
{
    public class WindSlime : EnemyAI
    {
        //スライム緑
        void Awake ()
        {
            nameplate = "Namebar_WindSlime";
            nameplateId = 10;
            parameter.attack = 8900;
            parameter.defense = 1418;
//          parameter.mattack = 10;
//          parameter.mdefense = 10;
            parameter.speed = 2144;
            parameter.hp = 10000;

            parameter.elementAff = new ElementWind(Element.WIND); 

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}