using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class Amelia : Profile {
        // Use this for initialization
        void Awake () {
            nameplate = "Namebar_Amelia";
            nameplateId = 0;
			ultimateAttack = "Amelia_SP";
            parameter.attack = 538;
            parameter.defense = 161;
            parameter.speed = 397;
            parameter.hp = 1271;
            parameter.criticalHit = 0.15f;
            parameter.elementAff = new ElementThunder(Element.THUNDER);

            attackList.Add("Attack_Normal", gameObject.AddComponent<AmeliaNormal>());
            attackList.Add("Attack_Special", gameObject.AddComponent<AmeliaElement>());
			attackList.Add("Amelia_SP", gameObject.AddComponent<AmeliaUltimate>());
            //attackType = gameObject.AddComponent<PlayerStrike>();

        }


        // Update is called once per frame
        void Update () {

        }
    }
}