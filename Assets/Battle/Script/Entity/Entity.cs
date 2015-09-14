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
        public bool wait;
        public bool curve;

        public float orderIndex;
        public string battleID { get ; set; }

        void Awake ()
        {
            Init();
        }
        void Update()
        {
            if(charge)
            {
                Debug.Log(this +"***"+ charge);
            }
        }

        public virtual void Init()
        {
            tracker = GameObject.FindObjectOfType<AttackTracker>() as AttackTracker;
            EventMgr.Instance.AddListener<TurnEnds>(UpdateOrder);
            EventMgr.Instance.AddListener<MonsterDies>(Die);
            attackReady = false;
            chargeReady = true;
            charge = false;

        }

        public virtual bool Attack (AttackType attack)
        {
            if(target != null && !target.IsAlive()) {
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

        public virtual void StartTurn()
        {

        }
        public virtual void EndTurn()
        {
            attackReady = false;
            EventMgr.Instance.Raise(new BeforeTurnEnds(this));
            EventMgr.Instance.Raise(new TurnEnds(false));
        }

        protected virtual void UpdateOrder(TurnEnds gameEvent)
        {

            if(gameEvent.monsterDied)
            {
                EventMgr.Instance.Raise(new NewTurn(this));
                return;
            }
            if(!charge && !wait)
            {                 
                orderIndex--;
                if(orderIndex < 0) {
                    orderIndex =  BattleMgr.Instance.actorList.Count - 1;
                }
                tracker.MoveTo(this, orderIndex);
            }
            else {            
                chargeReady = true;
            }
            EventMgr.Instance.Raise(new NewTurn(this));
            charge = false;
            wait = false;
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
                EventMgr.Instance.Raise(new TurnEnds(true));
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