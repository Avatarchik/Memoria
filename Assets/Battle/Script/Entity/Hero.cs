using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using Memoria.Battle.Managers;
using Memoria.Battle.States;
using Memoria.Battle;
using Memoria.Battle.Events;

namespace Memoria.Battle.GameActors
{
    public class Hero : Entity
    {
        private GameObject _stockEffect;

        private bool _enemyTarget;

        // Used when tapping for stock to store ultimate
        // attack when stock is full.
        public delegate void OnHit();
        private OnHit stockUp;

        public ElementalPowerStock power;
        public bool attackSelected;
        public bool passToStock;
        public bool tappable;
        public string nameplae;


        void Start ()
        {
            entityType = "hero";
            profile = GetComponent<Profile>();
            power = GetComponent<ElementalPowerStock>();
            parameter = profile.parameter;

            power.SortingLayer = "Middle";
            power.elementType = parameter.elementAff.Type.ToEnum<StockType, Element>();
            power.objType = ObjectType.NORMAL;
            transform.SetParent(GameObject.Find("Player").gameObject.transform, false);
            _stockEffect = (GameObject)Resources.Load("effects/Effect_UI_201");            
            parameter.blockBonus = (BattleMgr.Instance.elementalAffinity == parameter.elementAff.Type);

        }

        void Update()
        {
        }

        override public void Init()
        {
            components.Add(typeof(TargetSelector));
            components.Add(typeof(ElementalPowerStock));
            base.Init();
        }


        override public bool Attack (AttackType attack)
        {
            if(passToStock) {
                return true;
            }
            if(!attackReady)
            {
                StartTurn();
                BattleMgr.Instance.SetState(State.SELECT_SKILL);
                return false;
            }
            return base.Attack (attack);
        }

        override public void StartTurn()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, -10);
            if((parameter.blockBonus) && attackType == null) {
                StartCoroutine(StockRoutine(0.0f));
            } else {
                SetIconSkill();
            }
        }

        override public void EndTurn()
        {
            if(attackType != null) {
                power.UseStock(attackType.stockCost);
            }
            
            if(!charge && !passToStock)
            {
                attackSelected = false;
                attackType.attacked = false;
                attackType = null;
                target = null;
            }
            if(passToStock)
            {
                passToStock = false;
                target = null;
            }

            transform.position = new Vector3(transform.position.x,transform.position.y - 0.4f, -10);
            base.EndTurn();
        }

        override protected void UpdateOrder(TurnEnds gameEvent)
        {
            if(profile.GetType() == typeof(Dhiel))
            {
//                Debug.Log(this + ": updates");
            }
            base.UpdateOrder(gameEvent);
        }

        public void StockUp()
        {
            StartCoroutine(StockRoutine());
            passToStock = true;
            target  = GameObject.FindObjectOfType<MainPlayer>().GetComponent<Entity>() as IDamageable;
        }

        public void Cancel ()
        {
            attackSelected = false;
            charge = false;
            BattleMgr.Instance.SetState(State.SELECT_SKILL);
        }

        public void CheckIfhit()
        {
            if(GetComponent<TargetSelector>().hitBoxCollider && tappable)
            {
                stockUp.Invoke();
                tappable = false;
            }
        }

        public void SetAttack(string attack)
        {
            if(!charge) {
                attackType = profile.attackList[attack];

                if(attackType.stockCost > power.stock)
                    return;

                attackSelected = true;
                if(attackType.phaseCost > 0) {
                    charge = true;
                    chargeReady = false;
                }
            }

            if (attackType.targetType == ENEMY)
            {
                _enemyTarget = true;
            }
            if (attackType.targetType == PARTY)
            {
                _enemyTarget = false;
            }
        }
        public bool TargetSelected()
        {
            if (target == null) {
                return GetComponent<TargetSelector> ().TargetSelected(_enemyTarget);
            } else {
                return true;
            }
        }

        public void SetTarget(IDamageable e)
        {
            if (target == null) {
                target = e;
            }
            attackReady = true;
        }

        public string[] GetSkills()
        {
            var list = new List<string>();

            foreach (var skill in profile.attackList.Where(x => x.Value.stockCost < 3)) {
                list.Add(skill.Key);
            }
            return list.ToArray();
        }

        private void SetIconSkill()
        {
            tappable = true;
            if(!power.Full)
            {
                stockUp = StockUp;
                return;
            }
            stockUp = SetUltimate;
        }

        private void SetUltimate()
        {
            SetAttack(profile.ultimateAttack);
        }
        private void SetFinalStock()
        {
        }

        private IEnumerator StockRoutine(float extraStock = 0.4f)
        {
            if(!power.Full)
            {
                var pos = this.transform.position;
                _stockEffect.transform.position = new Vector3(pos.x + power.OffsetX + (power.stock * power.SpaceOffset),
                                                              pos.y + power.OffsetY - extraStock, pos.z + power.LayerOffset);

                Destroy(Instantiate(_stockEffect), 2.0f);
            }

            yield return new WaitForSeconds(0.5f);
            power.AddStock();
            if(extraStock == 0.0f)
            {
                power.UpdateStatus();
                SetIconSkill();
            }
            yield return null;
        }
    }
}
