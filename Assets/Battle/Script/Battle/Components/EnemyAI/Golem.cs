using UnityEngine;
using System.Collections;

public class Golem : EnemyAI {

    // Use this for initialization
    void Start ()
    {
        Debug.Log("fdsafdsf");
        attackList.Add ("Quick_attack", gameObject.AddComponent<QuickAttack>());
        attackType = attackList["Quick_attack"];
        nameplate = "GOJBNA6000";
    }

    void Update() 
    {
    }
}
