
namespace Memoria.Battle.GameActors
{
    public class ThunderSlime : EnemyAI
    {
        //スライム黄
        void Awake ()
        {
            nameplate = "Namebar_Golem";

            parameter.attack = 8850;
            parameter.defense = 1614;
//          parameter.mattack = 10;
//          parameter.mdefense = 10;
            parameter.speed = 1977;
            parameter.hp = 10000;

            parameter.elementAff = new ElementThunder(Element.THUNDER);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}