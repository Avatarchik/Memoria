using UnityEngine;
using Memoria.Battle.Utility;
using Memoria.Battle.Managers;
using Memoria.Battle.States;

namespace Memoria.Battle.GameActors
{
    public class Enemy : Entity, IDamageable
    {
        private bool isAlive = true;

        void Start()
        {
            target  = GameObject.FindObjectOfType<MainPlayer>().GetComponent<Entity>() as IDamageable;
            parameter.speed = 100;
            entityType = "enemy";
            health = GetComponent<HealthSystem> ();
            death = GetComponent<DeathSystem> ();
            profile = GetComponent<EnemyAI>();
            attackType = profile.attackType;

            death.isAlive = true;
            health.maxHp = 150;
            health.hp = health.maxHp;
        }

        void Update()
        {
            if (isAlive && health.hp <= 0) {
                isAlive = false;
                BattleMgr.Instance.RemoveFromBattle(this);
                StartCoroutine(death.DeadEffect());
            }
        }

        override public void Init()
        {
            components.Add(typeof(HealthSystem));
            components.Add(typeof(DeathSystem));
            base.Init();
        }

        override public bool Attack (AttackType attackType)
        {
            if(!isAlive) { return false; }
            phaseTimer = attackType.phaseCost;
            if (!attackReady && isAlive) {
                FadeAttackScreen.Flash(); //TODO: temporary
                BattleMgr.Instance.SetState(State.RUNNING);
            }
            return base.Attack (attackType);
        }

        override public void EndTurn()
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

        public bool IsAlive()
        {
            return isAlive;
        }
    }
}