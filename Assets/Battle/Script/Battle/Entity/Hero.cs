using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

using Memoria.Battle.Managers;
using Memoria.Battle.States;

namespace Memoria.Battle.GameActors
{
    public class Hero : Entity {

        const char ENEMY = 'e';
        const char PARTY = 'h';

        public bool attackSelected;
        public bool passtToStock;
        public string nameplae;
        public int stock;
        private Button _iconButton;
        private bool _enemyTarget;

        void Start () {
            entityType = "hero";
            profile = GetComponent<Profile>();
            parameter = profile.parameter;
            nameplate = profile.nameplate;
            _iconButton = GetComponent<Button>();
        }

        void Update()
        {
            for (int i = 0; i < stock; i++)
            {
                //Update stock icons
            }
        }
        override public void Init()
        {
            components.Add(typeof(TargetSelector));
            components.Add(typeof(Namebar));
            base.Init();
        }

        override public bool Attack (AttackType attack)
        {
            SetIconSkill(stock);
            if(passtToStock)
            {
                return true;
            }
            if(!attackReady) {
                StartTurn();
                BattleMgr.Instance.SetState(State.SELECT_SKILL);
                return false;
            }
            return base.Attack (attack);
        }

        override public void StartTurn()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, 1);
        }

        override public void EndTurn()
        {
            _iconButton.onClick.RemoveAllListeners();
            if(!charge && !passtToStock) {
                attackSelected = false;
                attackType.attacked = false;
                attackType = null;
                target = null;
            }
            passtToStock = false;
            transform.position = new Vector3(transform.position.x,transform.position.y - 0.4f, 1);
            base.EndTurn();
        }

        public void StockUp()
        {
            if(BattleMgr.Instance.elementalAffinity == parameter.elementAff) {
                stock += 1;
            }
            stock += 1;
            if(stock > 3)
            {
                stock = 3;
            }
            passtToStock = true;
            Debug.Log(this +"Elemetal Stock"+ stock);
        }

        public void SetAttack(string attack)
        {
            _iconButton.onClick.RemoveAllListeners();
            if(!charge) {
                attackType = profile.attackList[attack];
                attackSelected = true;
                if(attackType.phaseCost > 1) {
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
            } else
                  return true;
        }

        public void SetTarget(IDamageable e)
        {
            if (target == null){
                target = e;
            }
            attackReady = true;
        }

        public string[] GetSkills()
        {
            var list = new List<string>();

            foreach (var skill in profile.attackList.Where(x => x.Value.stockCost < 3))
            {
                list.Add(skill.Key);
            }
            return list.ToArray();
        }

        private void SetIconSkill(int i)
        {
            if(i < 3)
            {
                _iconButton.onClick.AddListener(() => StockUp());
                return;
            }
            _iconButton.onClick.AddListener(() => SetAttack(profile.ultimateAttack));
        }
    }
}