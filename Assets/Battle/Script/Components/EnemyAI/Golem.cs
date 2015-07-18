
namespace Memoria.Battle.GameActors
{
    public class Golem : EnemyAI
    {
        void Awake ()
        {
            int[][] i;

            nameplate = "Namebar_Golem";

            parameter.attack = 10;
            parameter.defense = 10;
            parameter.mattack = 10;
            parameter.mdefense = 10;
            parameter.speed = 10;
            parameter.hp = 300;

            parameter.elementAff = new ElementFire(Element.FIRE);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}