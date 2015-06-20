using UnityEngine;
using System.Collections;
using System.Linq;

public struct Parameter {
    
    public int hp;
    public int attack;
    public int defense;
    public int mattack;
    public int mdefense;
    public int speed;
    
}

public class Entity : MonoBehaviour {

    public string battleID;
    public IDamageable target;
    public AttackType attackType;
    public int attackTimer = 0;
//    public float attackDir;
    public string entityType;
    public Profile profile;
    
    public int phaseTimer;
    public float orderIndex;
    public bool attackReady;
    public bool chargeReady;
    public bool charge;
    
    public Parameter parameter;
    
    //public Animator _animator;
    public HealthSystem health;
    public DeathSystem death;
    public AttackTracker tracker;
    
    // Use this for initialization
    void Awake () {
        tracker = GameObject.FindObjectOfType(typeof(AttackTracker)) as AttackTracker;
        EventListner.Instance.SubscribeTurnEnd(UpdateOrder);
        attackReady = false;
        chargeReady = true;
    }

    public virtual bool Attack (AttackType attack)
    {
        if(charge) {
            this.orderIndex = attack.phaseCost;
            tracker.QueueAction(this, orderIndex);
            BattleMgr.currentState = BattleMgr.BattleState.BattleRunning;
            return true;
        }
        this.attackTimer++;
        if(!attack.attacked)
        {
            attack.Execute(target);
            attack.attacked = true;
            
        }
        if (this.attackTimer > attack.AttackTime) {
            attackTimer = 0;
            attackReady = true;
            return true;   
        }
        return false;
    }

    public virtual void StartTurn()
    {
        
    }
    
    public virtual void EndTurn()
    {
        this.attackReady = false;
    }

    private void UpdateOrder()
    {
        if(!this.charge) {
            this.orderIndex--;
            if(this.orderIndex < 0) {
                this.orderIndex = BattleMgr.actorList.Count - 1;
            }
            tracker.MoveTo(this, orderIndex);
        } else {
            this.charge = false;
            this.chargeReady = true;
        }
        
    }
}
