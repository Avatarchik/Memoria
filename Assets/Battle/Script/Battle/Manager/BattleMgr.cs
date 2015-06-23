using UnityEngine;
using System.Collections.Generic;
using System.Linq;

abstract public class BattleState
{
    public enum State
    {
        PREPARE,
        RUNNING,
        SELECT_TARGET,
        SELECT_SKILL,
        ANIMATOIN,
        PLAYER_WON
    }

    protected BattleMgr battleMgr;
    protected  UIMgr uiMgr;
    protected Entity nowActor;
    public bool Initialized { get; set; }

    public void PreInitialize(BattleMgr bMgr, UIMgr uMgr, Entity nActor)
    {
        uiMgr = uMgr;
        battleMgr = bMgr;
        nowActor = nActor;
    }

    public void EndState()
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

        BattleMgr.actorList = BattleMgr.actorList.OrderByDescending (x => x.GetComponent<Entity> ().parameter.speed).ToList ();
        GenerateOrderIndex ();
        uiMgr.SpawnAttackOrder();
        uiMgr.CreateHpBar();
    }

    override public void Update()
    {
        _timeBeforeStart--;
        if((_timeBeforeStart) <= 0) {
            _timeBeforeStart = 120;
            battleMgr.SetState(State.RUNNING);
        }
    }
        
    public void GenerateOrderIndex() 
    {
        foreach (GameObject go in BattleMgr.actorList) {
            Entity actor = go.GetComponent<Entity>();
            actor.orderIndex = _orderIndex;
            if(!battleMgr.AttackTracker.attackOrder.ContainsKey(actor))
            {
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
        if(hero.attackSelected || hero.passtToStock) {
            uiMgr.DestroyButton();
            battleMgr.SetState(State.SELECT_TARGET);
        }
    }    
}

public class StateSelectTarget : BattleState
{
    Hero hero;
    override public void Initialize()
    {
        hero = (Hero)nowActor;
        if(!hero.passtToStock)
        {
            SetSelectable(nowActor.attackType.targetType, true);            
        }
    }
    override public void Update()
    {
        if(hero.passtToStock)
        {
            battleMgr.SetState(State.RUNNING);
        }
        if(hero.EnemySelected()) {
            hero.SetTarget((IDamageable)hero.GetComponent<TargetSelector>().target);
            SetSelectable(nowActor.attackType.targetType, false);
            battleMgr.SetState(State.ANIMATOIN);
        }
    }

    private void SetSelectable(char c, bool state)
    {
        if(hero.passtToStock)
            return;
        foreach(var actor in BattleMgr.actorList)
        {
            var e = actor.GetComponent<Entity>();
            if (e.battleID.ToLowerInvariant().IndexOf(c) != -1)
            {
                e.GetComponent<BoxCollider2D>().enabled = state;
            }     
        }
    }
}
    
public class StateAnimation : BattleState
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
        battleMgr.SetState(State.RUNNING);
    }
}


public class StatePlayerWon : BattleState
{
    override public void Initialize()
    {
        Debug.Log("Player won"); 
        FadeAttackScreen.DeFlash();
    }
    override public void Update()
    {
        
    }
}    


public class BattleMgr : MonoBehaviour {

    public static List<GameObject> actorList = new List<GameObject>();

    private static BattleMgr _instance;

    private Dictionary<BattleState.State, BattleState> _battleStates;
    private Dictionary<string, string>  _profiles = new Dictionary<string, string>(); //Temporary hero list
    private string[] _party;
    private ActorSpawner _spawner;
    
    public List<GameObject> enemyList = new List<GameObject> ();
    
    public Entity NowActor { get; private set; }
    public UIMgr UiMgr { get; private set; }
    public AttackTracker AttackTracker { get; private set; }
    public BattleState CurrentState { get; private set; }
    public ElementType elementalAffinity { get; private set; }

    public enum ElementType
    {
        FIRE = 0,
        WATER = 1,
        WIND = 2,
        THUNDER = 3 
    }

    public float _attackAnimation;
    
    public static BattleMgr Instance
    {
        get {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<BattleMgr>() as BattleMgr;
            }
            return _instance;
        }
    }

    void Awake ()
    {
        //set variables sent from dungeon scene
        elementalAffinity = ElementType.FIRE;
        _party = new string[] { "Amelia", "Claude", "Tracy", "Aria" };

        //set entitys
        InitBattleStates();
        _spawner = FindObjectOfType<ActorSpawner>();
        AttackTracker = GetComponent<AttackTracker>();
        UiMgr = GetComponent<UIMgr> ();
    }
    void Start()
    {
        SpawnHeroes();
        SpawnEnemies();
        CurrentState = _battleStates[BattleState.State.PREPARE];
    }

    void Update () {
        UiMgr.SetAttackOrder();
        CheckWinLoss();
        if(CurrentState.Initialized) {
            CurrentState.Update();
        } else {
            CurrentState.PreInitialize(this, UiMgr, NowActor);
            CurrentState.Initialize();
            CurrentState.Initialized = true;
        }
    }

    private void SpawnHeroes()
    {
        _profiles = _spawner.GetProfiles(_party);
        Transform ui =  GameObject.FindObjectOfType<Canvas>().gameObject.transform;
        for(int i = 0; i < _profiles.Count; i++)
        {
            var pos = new Vector3((3.8f / 1.5f - 4f + i) * 3.5f, -3, 1);
            var hero = _spawner.Spawn<Hero>(_profiles.ElementAt(i).Key , _profiles.ElementAt(i).Value);
            _spawner.InitObj(hero, hero.GetComponent<Hero>().components, ui);
            hero.GetComponent<Hero>().battleID = "h0" + i;
            hero.transform.position = pos;
            actorList.Add(hero);
        }
    }

    private void SpawnEnemies()
    {
        string[] enemies = _spawner.GetRandomEnemies();
        for(int i = 0; i < enemies.Length; i++)
        {
            var randomEnemy = _spawner.Spawn<Enemy>(enemies[i], "enemy" + i);
            _spawner.InitObj(randomEnemy, randomEnemy.GetComponent<Enemy>().components, _spawner.parentObject);
            randomEnemy.GetComponent<Enemy>().battleID = "e0" + i;
            randomEnemy.transform.position = new Vector3((enemies.Length / 1.5f - enemies.Length + i) * 2, 1.5f, -9);
            enemyList.Add(randomEnemy);
            actorList.Add(randomEnemy);
        }
    }
    
    public void InitBattleStates()
    {
        _battleStates = new Dictionary<BattleState.State, BattleState>();
        _battleStates.Add(BattleState.State.PREPARE       ,new StatePrepare());
        _battleStates.Add(BattleState.State.RUNNING       ,new StateBattleRunning());
        _battleStates.Add(BattleState.State.SELECT_SKILL  ,new StatePlayerAction());
        _battleStates.Add(BattleState.State.SELECT_TARGET ,new StateSelectTarget());
        _battleStates.Add(BattleState.State.ANIMATOIN     ,new StateAnimation());
        _battleStates.Add(BattleState.State.PLAYER_WON    ,new StatePlayerWon());
    }

    public void SetState(BattleState.State state)
    {
        CurrentState.EndState();
        if(_battleStates.ContainsKey(state))
        {
            CurrentState = _battleStates[state];
        }
        else
        {
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
        if (actorList.Count == 4)
        {
            SetState(BattleState.State.PLAYER_WON);
            Invoke("LoadLevelTitle", _attackAnimation + 0.5f);
        }
    }

    private void LoadLevelTitle()
    {
        BattleMgr.actorList.Clear();
        DeathSystem.deadEnemy.Clear();
        enemyList.Clear();
        Application.LoadLevel("dungeon");
    }

    public void RemoveFromBattle(Entity e)
    {
        AttackTracker.DestroyActor(e);
        UiMgr.DestroyNameplate(e.battleID);
        actorList.RemoveAll(x => x.GetComponent<Entity>().Equals(e));
    }
}