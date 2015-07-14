using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class Amelia : Profile {
        // Use this for initialization
        void Awake () {
            nameplate = "Namebar_Amelia";

            parameter.attack = 3197;
            parameter.defense = 2268;
      //    parameter.mattack = 100;
      //    parameter.mdefense = 100;
            parameter.speed = 3972;
            parameter.hp = 2014;
            parameter.elementAff = new ElementThunder(Element.THUNDER);

            attackList.Add("Player_Strike", gameObject.AddComponent<PlayerStrike>());
            attackList.Add("Fire_Attack", gameObject.AddComponent<FireAttack>());
            //attackType = gameObject.AddComponent<PlayerStrike>();

        }


        // Update is called once per frame
        void Update () {

        }
    }
}