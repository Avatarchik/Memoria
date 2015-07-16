using UnityEngine;
using System.Collections.Generic;
using Memoria.Battle.States;
using Memoria.Battle.Managers;
using Memoria.Battle.Utility;
using Memoria.Battle.Events;

namespace Memoria.Battle.GameActors
{
    public class Entity : MonoBehaviour {

        protected const char ENEMY = 'e';
        protected const char PARTY = 'h';

        public List<System.Type> components = new List<System.Type>();

        public string battleID { get ; set; }
        public IDamageable target;
        public AttackType attackType;

        public int attackTimer = 0;

        public string entityType;
        public Profile profile;

        public int phaseTimer;
        public float orderIndex;

        public bool attackReady;
        public bool chargeReady;
        public bool charge;

        public Parameter parameter;

        public HealthSystem health;
        public DeathSystem death;
        public AttackTracker tracker;

        void Awake ()
        {
            Init();
        }

        public virtual void Init()
        {
            tracker = GameObject.FindObjectOfType<AttackTracker>() as AttackTracker;
            EventMgr.Instance.AddListener<TurnEnds>(UpdateOrder);
            EventMgr.Instance.AddListener<MonsterDies>(Die);
            attackReady = false;
            chargeReady = true;
        }

        public virtual bool Attack (AttackType attack)
        {
            if(!target.IsAlive()) {
                return true;
            }

            if(charge)
            {
                orderIndex = attack.phaseCost;
                tracker.QueueAction(this, orderIndex);
                BattleMgr.Instance.SetState(State.RUNNING);
                return true;
            }
            attackTimer++;

            if (attackTimer > attack.AttackTime)
            {
                attackTimer = 0;
                attackReady = true;
                DealDamage(attack);
                return true;
            }
            return false;
        }

        public void DealDamage(AttackType attack)
        {
                Damage damage = ScriptableObject.CreateInstance<Damage>();
                damage.AttackerParameters = parameter;
                attack.Execute(damage, target);
                attack.attacked = true;
        }

        public virtual void StartTurn()
        {
        }

        public virtual void EndTurn()
        {
            attackReady = false;
        }

        protected void UpdateOrder(TurnEnds gameEvent)
        {
           if(!charge)
            {
                orderIndex--;
                if(orderIndex < 0) {
                    orderIndex = BattleMgr.actorList.Count - 1;
                }
                tracker.MoveTo(this, orderIndex);
            }
            else
            {
                charge = false;
                chargeReady = true;
            }
        }

        protected void Die(MonsterDies gameEvent)
        {
            if(this.Equals(gameEvent.killedEntity))
            {
                EventMgr.Instance.RemoveListener<TurnEnds>(UpdateOrder);
            }
            if((this.orderIndex) > gameEvent.killedEntity.orderIndex)
            {
                tracker.MoveTo(this, orderIndex--);
            }
        }
    }
}