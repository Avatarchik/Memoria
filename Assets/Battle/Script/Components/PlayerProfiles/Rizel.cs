namespace Memoria.Battle.GameActors

{
    public class Rizel : Profile
    {
        void Awake ()
        {
            nameplate = "Namebar_Rizel";
            nameplateId = 3;
            ultimateAttack = "Rizel_SP";
            parameter.attack = 472;
            parameter.defense = 123;
            parameter.speed = 195;
            parameter.hp = 1426;
            parameter.criticalHit = 0.05f;
            parameter.elementAff = new ElementWater(Element.WATER);

            attackList.Add("Attack_Normal", gameObject.AddComponent<Rizel_S1>());
            attackList.Add("Attack_Special", gameObject.AddComponent<Rizel_S2>());
            attackList.Add("Rizel_SP", gameObject.AddComponent<Rizel_SP>());
        }
    }
}