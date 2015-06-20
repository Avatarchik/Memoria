using UnityEngine;
using System.Collections.Generic;

public class Hero : Entity {

    public bool attackSelected;
    public string nameplate;
    // Use this for initialization
    void Start () {
        parameter.speed = 20;
        entityType = "hero";
        profile = GetComponent<Profile>(); 
        nameplate = profile.nameplate;
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

    public override bool Attack (AttackType attack)
    {
        if(!attackReady) {
            StartTurn();
            BattleMgr.Instance.SetState("PlayerAction");
            return false;
        }
        return base.Attack (attack);
    }

    public override void StartTurn()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, 1);   
    }
    
    public override void EndTurn()
    {
        if(!charge) {
            BattleMgr.MainPlayer.EndTurn();
            attackSelected = false;
            attackType.attacked = false;
            attackType = null;
            target = null;
        }
        transform.position = new Vector3(transform.position.x,transform.position.y - 0.4f, 1);   
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
            target = e;
        }
        attackReady = true;

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
