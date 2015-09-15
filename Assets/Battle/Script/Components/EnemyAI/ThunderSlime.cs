
namespace Memoria.Battle.GameActors
{
    public class ThunderSlime : EnemyAI
    {
        //スライム黄
        void Awake ()
        {
            nameplate = "Namebar_ThunderSlime";
            nameplateId = 7;
            parameter.attack = 945;
            parameter.defense = 151;
            parameter.speed = 230;
            parameter.hp = 920;

            parameter.elementAff = new ElementThunder(Element.THUNDER);

            attackList.Add ("Enemy_Normal", gameObject.AddComponent<EnemyNormal>());
            attackList.Add ("Enemy_Special", gameObject.AddComponent<EnemyThunder>());
            attackType = attackList["Enemy_Normal"];
            attackType = attackList["Enemy_Special"];
        }
    }
}