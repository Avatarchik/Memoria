using UnityEngine;
using System.Collections;
using Memoria.Battle.Utility;
using Memoria.Battle.Managers;
using Memoria.Battle.States;
using Memoria.Managers;
     
namespace Memoria.Battle.GameActors
{
    public class Enemy : Entity, IDamageable
    {
        private bool isAlive = true;
        private bool _boss;
        private int _counter;
        private int _killSound;
        private bool _ultimate;

        private readonly string [] _attackList =
            {
                "Enemy_Normal",
                "Enemy_Skill",
                "Enemy_Ultimate"
            };

        void Start()
        {
            entityType = "enemy";

            health = GetComponent<HealthSystem> ();
            death = GetComponent<DeathSystem> ();
            profile = GetComponent<EnemyAI>();

            parameter = profile.parameter;
            death.isAlive = true;
            health.maxHp = parameter.hp;
            health.hp = health.maxHp;

            attackType = profile.attackList[GetPattern(_counter, _boss)];
            target  = GameObject.FindObjectOfType<MainPlayer>().GetComponent<Entity>() as IDamageable;

            transform.SetParent(GameObject.Find("Enemies").gameObject.transform, false);
            parameter.blockBonus = (BattleMgr.Instance.elementalAffinity == parameter.elementAff.Type);

            _counter = 0;
            _killSound = 2;

            if(profile.attackList.ContainsKey("Enemy_Ultimate"))
            {
                _boss = true;
                _killSound = 3;
            }
        }

        void Update()
        {
        }

        override public void Init()
        {
            components.Add(typeof(HealthSystem));
            components.Add(typeof(DeathSystem));
            base.Init();
        }

        override public bool Attack (AttackType attackType)
        {
            if(!isAlive) {
                return true;
            }
            phaseTimer = attackType.phaseCost;

            if(phaseTimer > 0 && !_ultimate)
            {
                charge = true;
                chargeReady = false;
                _ultimate = true;
            }
            if (!attackReady && isAlive)
            {
                attackReady = true;
                BattleMgr.Instance.SetState(State.ANIMATOIN);
            }
            return base.Attack (attackType);
        }

        override public void EndTurn()
        {
            if(!charge) {
                _counter++;
                if(_counter > 2) {
                    _counter = 0;
                }
                _ultimate = false;
                attackType = profile.attackList[GetPattern(_counter, _boss)];
                this.attackType.attacked = false;
            }
            base.EndTurn();
        }

        public void TakeDamage(Damage d)
        {
            StartCoroutine(DamageFlick(1, 0.03f, 0.03f));
            d.TargetParameters = parameter;
            this.health.hp -= d.Calculate();
            d.Appear(this.transform.position);
            CheckDeath();
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

        private void CheckDeath()
        {
            if (isAlive && health.hp <= 0)
            {
                isAlive = false;
                BattleMgr.Instance.RemoveFromBattle(this);
                SoundManager.instance.PlaySound(_killSound);
                StartCoroutine(death.DeadEffect());
            }
        }

        private string GetPattern(int turnCount, bool boss)
        {
            if(boss && (turnCount >= 2)) {
                return _attackList[2];
            }
            return _attackList[(turnCount % 2)];
        }

    }
}