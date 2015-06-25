using UnityEngine;
using System.Collections;

public class Claude : Profile {

    // Use this for initialization
    void Awake () {
        nameplate = "GOJBNA4006";

        parameter.attack = 317;
        parameter.defense = 239;
        parameter.mattack = 224;
        parameter.defense = 260;
        parameter.speed = 342;
        parameter.hp = 484;
        parameter.elementAff = BattleMgr.ElementType.THUNDER;
        
        attackList.Add("Player_Strike", gameObject.AddComponent<PlayerStrike>());
        attackList.Add("Fire_Attack", gameObject.AddComponent<FireAttack>());
        //attackType = gameObject.AddComponent<PlayerStrike>();

    }
    // Update is called once per frame
    void Update () {

    }
}
