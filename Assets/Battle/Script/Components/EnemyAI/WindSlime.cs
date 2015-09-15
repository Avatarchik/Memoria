
namespace Memoria.Battle.GameActors
{
    public class WindSlime : EnemyAI
    {
        //スライム緑
        void Awake ()
        {
            nameplate = "Namebar_WindSlime";
            nameplateId = 10;
            parameter.attack = 903;
            parameter.defense = 133;
            parameter.speed = 275;
            parameter.hp = 810;

            parameter.elementAff = new ElementWind(Element.WIND); 

            attackList.Add ("Enemy_Normal", gameObject.AddComponent<EnemyNormal>());
            attackList.Add ("Enemy_Special", gameObject.AddComponent<EnemyWind>());
            attackType = attackList["Enemy_Normal"];
            attackType = attackList["Enemy_Special"];
        }
    }
}