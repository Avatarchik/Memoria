
namespace Memoria.Battle.GameActors
{
    public class WindBoss : EnemyAI
    {
        //風ボス
        void Awake ()
        {
            nameplate = "Namebar_Golem";

            parameter.attack = 9137;
            parameter.defense = 1793;
//          parameter.mattack = 10;
//          parameter.mdefense = 10;
            parameter.speed = 2468;
            parameter.hp = 48000;

            parameter.elementAff = new ElementWind(Element.WIND); 

            attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
            attackType = attackList["Quick_attack"];
        }
    }
}