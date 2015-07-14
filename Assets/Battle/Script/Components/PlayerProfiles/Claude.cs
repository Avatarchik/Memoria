namespace Memoria.Battle.GameActors
{
    public class Iska : Profile {
        //現イスカ
        // Use this for initialization
        void Awake () {
            nameplate = "Namebar_Iska";

            parameter.attack = 5425;
            parameter.defense = 1691;
//          parameter.mattack = 100;
//          parameter.mdefense = 100;
            parameter.speed = 2584;
            parameter.hp = 1603;

            parameter.elementAff = new ElementFire(Element.FIRE); 

            attackList.Add("Player_Strike", gameObject.AddComponent<PlayerStrike>());
            attackList.Add("Fire_Attack", gameObject.AddComponent<FireAttack>());
        }
    }
}