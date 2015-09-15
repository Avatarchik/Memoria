
namespace Memoria.Battle.GameActors
{
    public class FireSlime : EnemyAI
    {
        //スライム赤
        void Awake ()
        {
            nameplate = "Namebar_FireSlime";
            nameplateId = 9;
            parameter.attack = 1010;
            parameter.defense = 100;
            parameter.speed = 209;
            parameter.hp = 770;

            parameter.elementAff = new ElementFire(Element.FIRE);

            attackList.Add ("Enemy_Normal", gameObject.AddComponent<EnemyNormal>());
            attackList.Add ("Enemy_Special", gameObject.AddComponent<EnemyFire>());
            attackType = attackList["Enemy_Normal"];
            attackType = attackList["Enemy_Special"];
        }
    }
}