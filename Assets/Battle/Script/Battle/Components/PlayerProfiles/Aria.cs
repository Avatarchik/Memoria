using UnityEngine;
using System.Collections;

public class Aria : Profile {

    // Use this for initialization
    void Start () {
        parameter.attack = 317;
        parameter.defense = 239;
        parameter.mattack = 224;
        parameter.defense = 260;
        parameter.speed = 342;
        parameter.hp = 484;
        
        attackList.Add("Player_Strike", gameObject.AddComponent<PlayerStrike>());
        attackList.Add("Fire_Attack", gameObject.AddComponent<FireAttack>());
        //attackType = gameObject.AddComponent<PlayerStrike>();

        nameplate = "GOJBNA4001";
    }
    
    // Update is called once per frame
    void Update () {

    }
}
