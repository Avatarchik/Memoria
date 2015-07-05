﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

using Memoria.Battle.Managers;
using Memoria.Battle.States;

namespace Memoria.Battle.GameActors
{
    public class Hero : Entity {

        public bool attackSelected;
        public bool passToStock;
        public string nameplae;
        private Button _iconButton;
        private bool _enemyTarget;
        private bool _initializedTurn;

        public ElementalPowerStock power;

        void Start () {
            entityType = "hero";

            profile = GetComponent<Profile>();
            power = GetComponent<ElementalPowerStock>();
            parameter = profile.parameter;
            nameplate = profile.nameplate;
            _iconButton = GetComponent<Button>();
            power.elementType = parameter.elementAff.ToEnum<Element, ElementType>();
            power.objType = ObjectType.UI_OBJECT;
        }

        void Update()
        {

//            for (int i = 0; i < _power.stock; i++)
  //          {
 //           }
        }
        override public void Init()
        {
            components.Add(typeof(TargetSelector));
            components.Add(typeof(Namebar));
            components.Add(typeof(ElementalPowerStock));
            base.Init();
        }


        override public bool Attack (AttackType attack)
        {
            if(passToStock)
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
            SetIconSkill();
            if(BattleMgr.Instance.elementalAffinity == parameter.elementAff && attackType == null) {
                power.AddStock();
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, 1);
        }

        override public void EndTurn()
        {
            _iconButton.onClick.RemoveAllListeners();
            if(!charge && !passToStock) {
                attackSelected = false;
                attackType.attacked = false;
                attackType = null;
                target = null;
            }
            if(passToStock) {
                passToStock = false;
                target = null;
            }
            if(charge)
            {
                power.UseStock(attackType.stockCost);
            }

            transform.position = new Vector3(transform.position.x,transform.position.y - 0.4f, 1);
            base.EndTurn();
        }

        public void StockUp()
        {
            power.AddStock();
            passToStock = true;
            target  = GameObject.FindObjectOfType<MainPlayer>().GetComponent<Entity>() as IDamageable;
        }

        public void SetAttack(string attack)
        {
            if(!charge) {
                attackType = profile.attackList[attack];

                if(attackType.stockCost > power.stock)
                    return;

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
            _iconButton.onClick.RemoveAllListeners();

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

        private void SetIconSkill()
        {
            if(!power.Full)
            {
                _iconButton.onClick.AddListener(() => StockUp());
                return;
            }
            _iconButton.onClick.AddListener(() => SetAttack(profile.ultimateAttack));
        }
    }
}