
namespace Memoria.Battle.GameActors
{
    public class Golem : EnemyAI {

        // Use this for initialization
        void Awake ()
        {
            parameter.attack = 10;
            parameter.defense = 10;
            parameter.mattack = 10;
            parameter.mdefense = 10;
            parameter.speed = 10;
            parameter.hp = 300;

            parameter.elementAff = new ElementFire(Element.FIRE);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
            nameplate = "Namebar_Golem";
        }

        void Update()
        {
        }
    }
}