using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class Amelia : Profile {
        // Use this for initialization
        void Awake () {
            nameplate = "Namebar_Amelia";
			ultimateAttack = "Amelia_SP";
            parameter.attack = 3197;
            parameter.defense = 2268;
      //    parameter.mattack = 100;
      //    parameter.mdefense = 100;
            parameter.speed = 3972;
            parameter.hp = 2014;
            parameter.elementAff = new ElementThunder(Element.THUNDER);

            attackList.Add("Player_Strike", gameObject.AddComponent<Amelia_S1>());
            attackList.Add("Fire_Attack", gameObject.AddComponent<Amelia_S2>());
			attackList.Add("Amelia_SP", gameObject.AddComponent<Amelia_SP>());
            //attackType = gameObject.AddComponent<PlayerStrike>();

        }


        // Update is called once per frame
        void Update () {

        }
    }
}