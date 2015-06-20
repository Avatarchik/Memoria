using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainPlayer : Entity, IDamageable {
    private const int HEALTH_BAR_FULL = 40;
    public int AP;
    
    // Use this for initialization
    void Awake () {
        entityType = "Player";
        health = GetComponent<HealthSystem> ();
        health.maxHp = 250;
        health.hp = 250;
        AP = 0;
    }
    
    // Update is called once per frame

    public void TakeDamage(int i)
    {
        health.hp -= i;
    }

    public void TakeDamage(Damage d)
    {
        
    }

    public override void EndTurn()
    {
        AP += 1;
    }

    public bool IsDead()
    {
        if(health.hp <= 0)
            return true;
        return false;
    }
}
