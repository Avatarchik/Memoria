using UnityEngine;
using System.Collections;
using Memoria.Battle.Managers;


namespace Memoria.Battle.GameActors
{
    public class Tracy : Profile {

        // Use this for initialization
        void Awake () {
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
            //attackType = gameObject.AddComponent<PlayerStrike>();

        }

        // Update is called once per frame
        void Update () {
        }
    }
}