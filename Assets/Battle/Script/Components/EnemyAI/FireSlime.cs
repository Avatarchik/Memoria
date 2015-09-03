
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
            parameter.defense = 90;
            parameter.speed = 209;
            parameter.hp = 470;

            parameter.elementAff = new ElementFire(Element.FIRE);

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}