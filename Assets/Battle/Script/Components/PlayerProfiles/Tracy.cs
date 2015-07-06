namespace Memoria.Battle.GameActors
{
    public class Tracy : Profile
    {
        void Awake ()
        {
            nameplate = "Namebar_Tracy";

            parameter.attack = 100;
            parameter.defense = 100;
            parameter.mattack = 100;
            parameter.mdefense = 100;
            parameter.speed = 100;
            parameter.hp = 100;
            parameter.elementAff = new ElementWater(Element.WATER);

            attackList.Add("Player_Strike", gameObject.AddComponent<PlayerStrike>());
            attackList.Add("Fire_Attack", gameObject.AddComponent<FireAttack>());
        }
    }
}