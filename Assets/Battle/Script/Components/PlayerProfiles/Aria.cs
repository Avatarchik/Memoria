namespace Memoria.Battle.GameActors

{
    public class Aria : Profile
    {
        void Awake ()
        {
            nameplate = "Namebar_Aria";
            nameplateId = 3;
            ultimateAttack = "Aria_SP";
            parameter.attack = 472;
            parameter.defense = 123;
            parameter.speed = 195;
            parameter.hp = 1426;
            parameter.criticalHit = 0.05f;
            parameter.elementAff = new ElementWater(Element.WATER);

            attackList.Add("Attack_Normal", gameObject.AddComponent<Aria_S1>());
            attackList.Add("Attack_Special", gameObject.AddComponent<Aria_S2>());
            attackList.Add("Aria_SP", gameObject.AddComponent<Aria_SP>());
        }
    }
}