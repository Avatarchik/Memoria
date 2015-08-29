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
        protected bool curve = false;

        public List<System.Type> components = new List<System.Type>();

        public IDamageable target;
        public AttackType attackType;
        public Profile profile;
        public Parameter parameter;
        public HealthSystem health;
        public DeathSystem death;
        public AttackTracker tracker;

        public int phaseTimer;
        public int attackTimer = 0;
        public string entityType;
        public bool attackReady;
        public bool chargeReady;
        public bool charge;

        public float orderIndex { get; set; }
        public string battleID { get ; set; }

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
            if(target != null && !target.IsAlive()) {
                return true;
            }

            if(charge)
            {
                curve = true;
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

        public virtual void StartTurn()
        {

        }
        public virtual void EndTurn()
        {
            attackReady = false;
        }

        protected virtual void UpdateOrder(TurnEnds gameEvent)
        {
            bool moves = true;
            if(!charge)
            {
                orderIndex--;
                if(orderIndex < 0) {
                    orderIndex =  BattleMgr.Instance.actorList.Count - 1;
                    curve = true;
                }
                tracker.MoveTo(this, orderIndex);
            }
            else
            {
                moves = false;
                chargeReady = true;
            }
            EventMgr.Instance.Raise(new NewTurn(this, moves, curve, charge));
            charge = false;
            curve = false;
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

        public void DealDamage(AttackType attack)
        {
                Damage damage = ScriptableObject.CreateInstance<Damage>();
                damage.AttackerParameters = parameter;
                attack.Execute(damage, target);
                attack.attacked = true;
        }

    }
}