namespace Memoria.Battle.GameActors
{
    public class Iska : Profile {
        //現イスカ
        // Use this for initialization
        void Awake () {
            nameplate = "Namebar_Iska";
            nameplateId = 2;
            ultimateAttack = "Iska_SP";
            parameter.attack = 849;
            parameter.defense = 77;
            parameter.speed = 258;
            parameter.hp = 745;
            parameter.criticalHit = 0.1f;
            parameter.elementAff = new ElementFire(Element.FIRE); 

            attackList.Add("Attack_Normal", gameObject.AddComponent<Iska_S1>());
            attackList.Add("Attack_Special", gameObject.AddComponent<Iska_S2>());
            attackList.Add("Iska_SP", gameObject.AddComponent<Iska_SP>());
        }
    }
}