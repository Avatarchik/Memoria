namespace Memoria.Battle.GameActors
{
    public class Dhiel : Profile
    {
        //現ディエル
        void Awake ()
        {
            nameplate = "Namebar_Dhiel";
            ultimateAttack = "Diel_SP";
            parameter.attack = 3612;
            parameter.defense = 1714;
//          parameter.mattack = 100;
//          parameter.mdefense = 100;
            parameter.speed = 3038;
            parameter.hp = 2499;
            parameter.elementAff = new ElementWind(Element.WIND); 

            attackList.Add("Player_Strike", gameObject.AddComponent<Diel_S1>());
            attackList.Add("Fire_Attack", gameObject.AddComponent<Diel_S2>());
            attackList.Add("Diel_SP", gameObject.AddComponent<Diel_SP>());
        }
    }
}