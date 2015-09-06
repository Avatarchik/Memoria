namespace Memoria.Battle.GameActors

{
    public class Aria : Profile
    {
        void Awake ()
        {
            nameplate = "Namebar_Aria";
            nameplateId = 3;
            ultimateAttack = "Aria_SP";
            parameter.attack = 2766;
            parameter.defense = 3027;
//          parameter.mattack = 100;
//          parameter.mdefense = 100;
            parameter.speed = 1953;
            parameter.hp = 3884;
            parameter.elementAff = new ElementWater(Element.WATER);

            attackList.Add("Attack_Normal", gameObject.AddComponent<Aria_S1>());
            attackList.Add("Attack_Special", gameObject.AddComponent<Aria_S2>());
            attackList.Add("Aria_SP", gameObject.AddComponent<Aria_SP>());
        }
    }
}