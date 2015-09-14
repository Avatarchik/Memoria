
namespace Memoria.Battle.GameActors
{
    public class WindBoss : EnemyAI
    {
        //風ボス
        void Awake ()
        {
            nameplate = "Namebar_WindBoss";
            nameplateId = 6;
            parameter.attack = 1044;
            parameter.defense = 169;
            parameter.speed = 298;
            parameter.hp = 5000;

            parameter.elementAff = new ElementWind(Element.WIND); 

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}