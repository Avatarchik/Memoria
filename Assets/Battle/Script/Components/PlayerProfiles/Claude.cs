namespace Memoria.Battle.GameActors
{
    public class Claude : Profile {

        // Use this for initialization
        void Awake () {
            nameplate = "Namebar_Claude";

            parameter.attack = 100;
            parameter.defense = 100;
            parameter.mattack = 100;
            parameter.mdefense = 100;
            parameter.speed = 100;
            parameter.hp = 100;
            parameter.elementAff = new ElementThunder(Element.THUNDER);

            attackList.Add("Player_Strike", gameObject.AddComponent<PlayerStrike>());
            attackList.Add("Fire_Attack", gameObject.AddComponent<FireAttack>());
        }
    }
}