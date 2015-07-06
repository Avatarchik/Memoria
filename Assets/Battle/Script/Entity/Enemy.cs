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
            entityType = "enemy";

            health = GetComponent<HealthSystem> ();
            death = GetComponent<DeathSystem> ();
            profile = GetComponent<EnemyAI>();

            parameter = profile.parameter;
            attackType = profile.attackType;

            death.isAlive = true;
            health.maxHp = parameter.hp;
            health.hp = health.maxHp;

            target  = GameObject.FindObjectOfType<MainPlayer>().GetComponent<Entity>() as IDamageable;

            transform.SetParent(GameObject.Find("Enemies").gameObject.transform, false);

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

        public void TakeDamage(Damage d)
        {
            d.TargetParameters = parameter;
            this.health.hp -= d.Calculate();
        }

        public bool IsAlive()
        {
            return isAlive;
        }
    }
}