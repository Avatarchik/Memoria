namespace Memoria.Battle.GameActors

{
    public class Aria : Profile
    {
        void Awake ()
        {
            nameplate = "Namebar_Aria";

            parameter.attack = 100;
            parameter.defense = 100;
            parameter.mattack = 100;
            parameter.defense = 100;
            parameter.speed = 100;
            parameter.hp = 100;
            parameter.elementAff = new ElementWind(Element.WIND);

            attackList.Add("Player_Strike", gameObject.AddComponent<PlayerStrike>());
            attackList.Add("Fire_Attack", gameObject.AddComponent<FireAttack>());
        }
    }
}