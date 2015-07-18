
namespace Memoria.Battle.GameActors
{
    public class FireSlime : EnemyAI
    {
        //スライム赤
        void Awake ()
        {
            nameplate = "Namebar_Golem";

            parameter.attack = 9000;
            parameter.defense = 1104;
//          parameter.mattack = 10;
//          parameter.mdefense = 10;
            parameter.speed = 2099;
            parameter.hp = 10000;

            parameter.elementAff = new ElementFire(Element.FIRE);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}