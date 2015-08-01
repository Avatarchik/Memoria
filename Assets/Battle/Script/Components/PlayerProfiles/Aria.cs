namespace Memoria.Battle.GameActors

{
    public class Aria : Profile
    {
        void Awake ()
        {
            nameplate = "Namebar_Aria";

            ultimateAttack = "WindAll";

            parameter.attack = 2766;
            parameter.defense = 3027;
//          parameter.mattack = 100;
//          parameter.mdefense = 100;
            parameter.speed = 1953;
            parameter.hp = 3884;
            parameter.elementAff = new ElementWater(Element.WATER);

            attackList.Add("Player_Strike", gameObject.AddComponent<PlayerStrike>());
            attackList.Add("Fire_Attack", gameObject.AddComponent<FireAttack>());
            attackList.Add("WindAll", gameObject.AddComponent<DhielUltimate>());
        }
    }
}