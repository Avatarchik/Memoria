
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
            parameter.defense = 141;
            parameter.speed = 230;
            parameter.hp = 620;

            parameter.elementAff = new ElementThunder(Element.THUNDER);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}