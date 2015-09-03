namespace Memoria.Battle.GameActors
{
    public class Iska : Profile {
        //現イスカ
        // Use this for initialization
        void Awake () {
            nameplate = "Namebar_Iska";
            nameplateId = 2;
            ultimateAttack = "Iska_SP";
            parameter.attack = 5425;
            parameter.defense = 1691;
//          parameter.mattack = 100;
//          parameter.mdefense = 100;
            parameter.speed = 2584;
            parameter.hp = 1603;

            parameter.elementAff = new ElementFire(Element.FIRE); 

            attackList.Add("Attack_Normal", gameObject.AddComponent<Iska_S1>());
            attackList.Add("Attack_Special", gameObject.AddComponent<Iska_S2>());
            attackList.Add("Iska_SP", gameObject.AddComponent<Iska_SP>());
        }
    }
}