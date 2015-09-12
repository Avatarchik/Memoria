using UnityEngine;
using System;
using System.Collections;
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
            parameter.blockBonus = (BattleMgr.Instance.elementalAffinity == parameter.elementAff.Type);

        }

        void Update()
        {
            if (isAlive && health.hp <= 0)
            {
                isAlive = false;
                BattleMgr.Instance.RemoveFromBattle(this);
                StartCoroutine(death.DeadEffect());
            }
        }

        override public void Init()
        {
            components.Add(typeof(HealthSystem));
            components.Add(typeof(DeathSystem));
//            components.Add(typeof(Namebar));
            base.Init();
        }

        override public bool Attack (AttackType attackType)
        {
            if(!isAlive) {
                return true;
            }

            phaseTimer = attackType.phaseCost;

            if (!attackReady && isAlive)
            {
                attackReady = true;
                BattleMgr.Instance.SetState(State.ANIMATOIN);
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
            StartCoroutine(DamageFlick(1, 0.03f, 0.03f));
            d.TargetParameters = parameter;
            this.health.hp -= d.Calculate();
            d.Appear(this.transform.position);
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        private IEnumerator DamageFlick(int nTimes, float timeOn, float timeOff)
        {
             var position = transform.position;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            transform.position = new Vector3(position.x, position.y + 0.2f, position.z);

            while (nTimes > 0) {
                sr.enabled = true;
                yield return new WaitForSeconds(timeOn);
                sr.enabled = false;
                yield return new WaitForSeconds(timeOff);
                nTimes--;
            }
            sr.enabled = true;   
            transform.position = position;
        }
    }
}