using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : Entity {

    public bool attackSelected;
    // Use this for initialization
    void Start () {
        parameter.speed = 20;
        entityType = "hero";
        profile = GetComponent<Profile>();
    }

    public void SetAttack(string attack)
    {
        if(!charge) {
            attackType = profile.attackList[attack];
            attackSelected = true;
            if(attackType.phaseCost > 1) {
                charge = true;
                chargeReady = false;
            }
        }
    }

    public override bool Attack (AttackType attackType)
    {
        if(!attackReady) {
            StartTurn();
            BattleMgr.currentState = BattleMgr.BattleState.PlayerAction;
            return false;
        }        
        return base.Attack (this.attackType);
    }

    public override void StartTurn()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.4f, 1);   
    }
    
    public override void EndTurn()
    {
        if(!charge) {
            attackSelected = false;
            attackType.attacked = false;
            attackType = null;
            target = null;
        }
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.4f, 1);   
        base.EndTurn();
    }

    public bool EnemySelected()
    {
        if (target == null) {
            return GetComponent<TargetSelector> ().TargetSelected ();
        } else
              return true;
    }

    public void SetTarget(IDamageable e)
    {
        if (target == null){
            this.target = e;
        }
        this.attackReady = true;

    }

    public string[] GetSkills()
    {
        var list = new List<string>();
        
        foreach (var skill in profile.attackList) {
            list.Add(skill.Key);
        }
        return list.ToArray();       
    }
}
