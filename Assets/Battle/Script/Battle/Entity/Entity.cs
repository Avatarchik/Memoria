using UnityEngine;
using System.Collections.Generic;
using Memoria.Battle.States;
using Memoria.Battle.Managers;
using Memoria.Battle.Utility;

namespace Memoria.Battle.GameActors
{
    public struct Parameter {
        public int hp;
        public int attack;
        public int defense;
        public int mattack;
        public int mdefense;
        public int speed;
        public ElementType elementAff;
    }

    public class Entity : MonoBehaviour {

        public List<System.Type> components = new List<System.Type>();

        public string nameplate;
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

        void Awake () {
            Init();
        }

        public virtual void Init()
        {
            tracker = GameObject.FindObjectOfType<AttackTracker>() as AttackTracker;
            EventListner.Instance.SubscribeTurnEnd(UpdateOrder);
            attackReady = false;
            chargeReady = true;
        }

        public virtual bool Attack (AttackType attack)
        {
            if(!target.IsAlive())
                return false;
            if(charge) {
                orderIndex = attack.phaseCost;
                tracker.QueueAction(this, orderIndex);
                BattleMgr.Instance.SetState(State.RUNNING);
                return true;
            }
            attackTimer++;
            if(!attack.attacked)
            {
                attack.Execute(target);
                attack.attacked = true;
            }
            if (attackTimer > attack.AttackTime) {
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
            attackReady = false;
        }

        protected virtual void UpdateOrder()
        {
            if(!charge) {
                orderIndex--;
                if(orderIndex < 0) {
                    orderIndex = BattleMgr.actorList.Count - 1;
                }
                tracker.MoveTo(this, orderIndex);
            } else {
                charge = false;
                chargeReady = true;
            }

        }
    }
}