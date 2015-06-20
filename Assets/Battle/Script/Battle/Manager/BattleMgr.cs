/* TODO:
 * Put in return button
 */
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class BattleMgr : MonoBehaviour {

    public static Entity MainPlayer;
    public static List<GameObject> actorList = new List<GameObject>();
    private static BattleMgr _instance;

    private int _orderIndex;
    private Entity _entity;
    private int _timeBeforeStart;
    private UIMgr _uiMgr;
    private AttackTracker _attackTracker;

    public enum BattleState{
        Prepare,
        BattleRunning,
        PlayerAction,
        SelectEnemy,
        EnemyAction,
        AnimationSequence,
        PlayerWon,
        PlayerLost
    }

    public static BattleMgr Instance
    {
        get {
            if(_instance == null) {
                _instance = GameObject.FindObjectOfType(typeof(BattleMgr)) as BattleMgr;
            }
            return _instance;
        }
    }
    
    
    public static BattleState currentState { get; set; }

    // Use this for initialization
    void Start () {

        MainPlayer = GameObject.FindWithTag ("MainPlayer").GetComponent<Entity> ();
        _attackTracker = GetComponent<AttackTracker>();
        _timeBeforeStart = 120;
        _uiMgr = GetComponent<UIMgr> ();
        currentState = BattleState.Prepare;

    }

    // Update is called once per frame
    void Update () {
//      Debug.Log (_orderIndex +":"+ _entity +":"+ currentState +":"+ actorList.Count);
        _uiMgr.SetAttackList();
        CheckWinLoss();
        switch (currentState) {
            case BattleState.Prepare:
                PrepareBattle();
                break;
            case BattleState.BattleRunning:
                _entity = _attackTracker.currentActor;
                UpdateBattle (_entity);
                break;
            case BattleState.PlayerAction:
                FadeAttackScreen.DeFlash(); //TODO: temporary
                HeroAction(_entity);
                break;
            case BattleState.SelectEnemy:
                HeroSelectTarget(_entity);
                break;
            case BattleState.AnimationSequence:
                PlayAnimation(_entity);
                currentState = BattleState.BattleRunning;
                break;
            case BattleState.PlayerWon:
                Debug.Log("Player won"); 
                FadeAttackScreen.DeFlash(); //TODO: temporary
                Invoke("LoadLevelTitle", 1.0f);
                break;
        }
    }
    
    private void UpdateBattle(Entity nowActor) 
    {
        if(nowActor.Attack (nowActor.attackType)) {
            nowActor.EndTurn();
            EventMgr.Instance.OnTurnEnd();
        }
    }

    private void HeroAction(Entity e)
    {
        Hero hero = (Hero)e;
        _uiMgr.ShowSkill(hero);
        if(hero.attackSelected) {
            _uiMgr.DestroyButton();
            currentState = BattleState.SelectEnemy;
        }
    }

    private void HeroSelectTarget(Entity e)
    {
        Hero hero = (Hero)e;
        if(hero.EnemySelected()) {
            hero.SetTarget((IDamageable)_entity.GetComponent<TargetSelector>().target);
            BattleMgr.currentState = BattleMgr.BattleState.AnimationSequence;
        }
    }
    //TODO: Temporary
    private void CheckWinLoss() 
    {
        if (actorList.Count == 4) {
            currentState = BattleState.PlayerWon;
        }
    }

    private void PlayAnimation(Entity e)
    {
        if(e.chargeReady) {
            e.attackType.PlayEffect((Entity)e.target);
        }
    }
    
    // Preparation functionsunctions
    
    private void PrepareBattle()
    {
        if(EnemySpawner.Init && HeroSpawner.Init) {
            actorList = actorList.OrderByDescending (x => x.GetComponent<Entity> ().parameter.speed).ToList ();
            GenerateOrderIndex ();
            _uiMgr.SpawnAttackList ();
            _uiMgr.CreateHpBar();
            EnemySpawner.Init = false; //temporary
            HeroSpawner.Init = false; //temporary
        }
        _timeBeforeStart--;
        if((_timeBeforeStart) <= 0) {
            _timeBeforeStart = 120;
            currentState = BattleState.BattleRunning;
        }
    }
    
    public void GenerateOrderIndex() 
    {
        foreach (GameObject go in actorList) {
            Entity actor = go.GetComponent<Entity>();            
            actor.orderIndex = _orderIndex;
            if(!_attackTracker.attackOrder.ContainsKey(actor)) {
                _attackTracker.attackOrder.Add(actor, _orderIndex);
            }
            _orderIndex++;
        }
    }

    public void RemoveFromBattle(Entity e)
    {
        _attackTracker.DestroyActor(e);
        actorList.RemoveAll(x => x.GetComponent<Entity>().Equals(e));
    }
    
    private void LoadLevelTitle()
    {
        actorList.Clear();
        Application.LoadLevel("dungeon");
    }   
}