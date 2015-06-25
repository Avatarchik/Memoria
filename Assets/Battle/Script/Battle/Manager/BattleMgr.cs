using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Memoria.Battle.States;
using Memoria.Battle.GameActors;
using Memoria.Battle.Utility;

namespace Memoria.Battle.Managers
{
    public class BattleMgr : MonoBehaviour {

        public static List<GameObject> actorList = new List<GameObject>();

        private static BattleMgr _instance;

        private Dictionary<BattleState.State, BattleState> _battleStates;
        private Dictionary<System.Type, string>  _profiles = new Dictionary<System.Type, string>(); //Temporary hero list
        private System.Type[] _party;
        private ActorSpawner _spawner;
    
        public List<GameObject> enemyList = new List<GameObject> ();
    
        public Entity NowActor { get; private set; }
        public UIMgr UiMgr { get; private set; }
        public AttackTracker AttackTracker { get; private set; }
        public BattleState CurrentState { get; private set; }

        public ElementType elementalAffinity { get; private set; }
        public float _attackAnimation;

        public enum ElementType
        {
            FIRE = 0,
            WATER = 1,
            WIND = 2,
            THUNDER = 3 
        }

        
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
            _party = new System.Type[] {
                typeof(Amelia),
                typeof(Claude),
                typeof(Tracy),
                typeof(Aria)
            };

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
            System.Type[] enemies = _spawner.GetRandomEnemies();
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
            _battleStates.Add(BattleState.State.SELECT_SKILL  ,new StateSelectSkill());
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
}