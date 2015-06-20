using UnityEngine;
using System.Collections.Generic;
using System.Linq;

abstract public class BattleState
{
    public BattleMgr battleMgr;
    public UIMgr uiMgr;
    public Entity nowActor;
    public bool Initialized { get; set; }

    public void PreInitialize(BattleMgr bMgr, UIMgr uMgr, Entity nActor)
    {
        uiMgr = uMgr;
        battleMgr = bMgr;
        nowActor = nActor;
    }

    public new void Finalize()
    {
        Initialized = false;
    }
    
    abstract public void Initialize();
    abstract public void Update();
}

public class StatePrepare : BattleState
{
    private int _orderIndex;
    private int _timeBeforeStart;
    override public void Initialize()
    {
        _timeBeforeStart = 120;

        if(EnemySpawner.Init && HeroSpawner.Init) {
            BattleMgr.actorList = BattleMgr.actorList.OrderByDescending (x => x.GetComponent<Entity> ().parameter.speed).ToList ();
            GenerateOrderIndex ();
            uiMgr.SpawnAttackList();
            uiMgr.CreateHpBar();
            EnemySpawner.Init = false; //temporary
            HeroSpawner.Init = false; //temporary
        }
    }

    override public void Update()
    {
        _timeBeforeStart--;
        if((_timeBeforeStart) <= 0) {
            _timeBeforeStart = 120;
            battleMgr.SetState("BattleRunning");
        }
    }
        
    public void GenerateOrderIndex() 
    {
        foreach (GameObject go in BattleMgr.actorList) {
            Entity actor = go.GetComponent<Entity>();            
            actor.orderIndex = _orderIndex;
            if(!battleMgr.AttackTracker.attackOrder.ContainsKey(actor)) {
                 battleMgr.AttackTracker.attackOrder.Add(actor, _orderIndex);
            }
            _orderIndex++;
        }
    }
}

public class StateBattleRunning  : BattleState
{
    override public void Initialize()
    {
        battleMgr.SetCurrentActor();
        nowActor = battleMgr.NowActor;
    }
    override public void Update()
    {
        Debug.Log(nowActor +" is attacking");
        if(nowActor.Attack (nowActor.attackType)){
            nowActor.EndTurn();
            EventMgr.Instance.OnTurnEnd();
            Initialized = false;
        }
    }    
}

public class StatePlayerAction : BattleState
{
    override public void Initialize()
    {
        FadeAttackScreen.DeFlash();
    }
    override public void Update()
    {
        var hero = (Hero)nowActor;
        uiMgr.ShowSkill(hero);
        if(hero.attackSelected) {
            uiMgr.DestroyButton();
            battleMgr.SetState("SelectEnemy");
        }
    }    
}

public class StateSelectEnemy : BattleState
{
    override public void Initialize()
    {}
    override public void Update()
    {
        var hero = (Hero)nowActor;
        if(hero.EnemySelected()) {
            hero.SetTarget((IDamageable)hero.GetComponent<TargetSelector>().target);
            battleMgr.SetState("AnimationSequence");
        }
    }
}

public class StateAnimationSequence : BattleState
{
    override public void Initialize()
    {
        battleMgr._attackAnimation = (float)(nowActor.attackType.AttackTime / 60);
    }
    override public void Update()
    {
        if(nowActor.chargeReady) {
            nowActor.attackType.PlayEffect((Entity)nowActor.target);
        }
        battleMgr.SetState("BattleRunning");
    }
}


public class StatePlayerWon : BattleState
{
    override public void Initialize()
    {
        Debug.Log("Player won"); 
        FadeAttackScreen.DeFlash();
        EnemySpawner.Init = false;
        HeroSpawner.Init = false;//TODO: temporary
    }
    override public void Update()
    {}
}    


public class BattleMgr : MonoBehaviour {

    public static Entity MainPlayer;
    public static List<GameObject> actorList = new List<GameObject>();

    private static BattleMgr _instance;
    private Dictionary<string, BattleState> _battleStates;
    
    public Entity NowActor { get; private set; }
    public UIMgr UiMgr { get; private set; }
    public AttackTracker AttackTracker { get; private set; }
    public BattleState CurrentState { get; private set; }

    public float _attackAnimation;
    
    public static BattleMgr Instance
    {
        get {
            if(_instance == null) {
                _instance = GameObject.FindObjectOfType<BattleMgr>() as BattleMgr;
            }
            return _instance;
        }
    }
     
    // Use this for initialization
    void Awake () {
        InitBattleStates();
        MainPlayer = GameObject.FindObjectOfType<MainPlayer>().GetComponent<Entity> ();
        AttackTracker = GetComponent<AttackTracker>();
        UiMgr = GetComponent<UIMgr> ();
    }

    void Start()
    {
        CurrentState = _battleStates["Prepare"];
        Debug.Log("Refactor branch");
    }
    
    // Update is called once per frame
    void Update () {
        UiMgr.SetAttackList();
        CheckWinLoss();
        if(CurrentState.Initialized) {
            CurrentState.Update();
        } else {
            CurrentState.PreInitialize(this, UiMgr, NowActor);
            CurrentState.Initialize();
            CurrentState.Initialized = true;
        }
    }

    public void InitBattleStates()
    {
        _battleStates = new Dictionary<string, BattleState>();
        _battleStates.Add("Prepare"            ,new StatePrepare());
        _battleStates.Add("BattleRunning"      ,new StateBattleRunning());
        _battleStates.Add("PlayerAction"       ,new StatePlayerAction());
        _battleStates.Add("SelectEnemy"        ,new StateSelectEnemy());
        _battleStates.Add("AnimationSequence"  ,new StateAnimationSequence());
        _battleStates.Add("PlayerWon"          ,new StatePlayerWon());
    }
    
    public void SetState(string state)
    {
        CurrentState.Finalize();
        if(_battleStates.ContainsKey(state)) {
            CurrentState = _battleStates[state];
        } else {
            Debug.Log("[E] Battle state missing.");
        }
    }

    public void SetCurrentActor()
    {
        NowActor = AttackTracker.currentActor;
    }
    
    //TODO: Temporary
    private void CheckWinLoss() 
    {
        if (actorList.Count == 4) {
            SetState("PlayerWon");
            Invoke("LoadLevelTitle", _attackAnimation + 0.5f);
        }
    }

    private void LoadLevelTitle()
    {
        BattleMgr.actorList.Clear();
        DeathSystem.deadEnemy.Clear();
        EnemySpawner.enemyObjs.Clear();
        Application.LoadLevel("dungeon");
    }

    public void RemoveFromBattle(Entity e)
    {
        AttackTracker.DestroyActor(e);
        UiMgr.DestroyNameplate(e.battleID);
        actorList.RemoveAll(x => x.GetComponent<Entity>().Equals(e));
    }
}