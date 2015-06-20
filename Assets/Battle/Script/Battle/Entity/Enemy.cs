using UnityEngine;
using System;
using System.Collections;


public class Enemy : Entity, IDamageable
{
    private bool isAlive = true;
    public Enemy ()
    {        
    }
    void Start()
    {
        target = (IDamageable)BattleMgr.MainPlayer;
        parameter.speed = 100;
        entityType = "enemy";
        health = GetComponent<HealthSystem> ();
        death = GetComponent<DeathSystem> ();
        profile = GetComponent<EnemyAI>();
        attackType = profile.attackType;
    }

    void Update()
    {
        if (isAlive && health.hp <= 0) {
            isAlive = false;
            BattleMgr.Instance.RemoveFromBattle(this);
            StartCoroutine(death.DeadEffect());
        }
    }

    public override bool Attack (AttackType attackType)
    {        
        if(!isAlive) { return false; }
        phaseTimer = attackType.phaseCost;
        if (!attackReady && isAlive) {
            FadeAttackScreen.Flash(); //TODO: temporary
            BattleMgr.Instance.SetState("AnimationSequence");
        }
        return base.Attack (attackType);
    }

    public override void EndTurn()
    {
        this.attackType.attacked = false;
        base.EndTurn();
    }

    public void TakeDamage(int i)
    {
        this.health.hp -= i;
    }

    public void TakeDamage(Damage d)
    {
        
    }

    public bool IsDead()
    {
        if(!isAlive)
            return true;
        return false;
    }
}