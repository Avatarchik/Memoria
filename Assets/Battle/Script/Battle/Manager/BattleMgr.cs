using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Memoria.Battle.States;
using Memoria.Battle.GameActors;
using Memoria.Battle.Utility;
using Memoria.Dungeon.BlockUtility;
using Memoria.Dungeon;

namespace Memoria.Battle.Managers
{
    public enum ElementType
    {
        FIRE = 0,
        WATER = 1,
        WIND = 2,
        THUNDER = 3
    }

    public class BattleMgr : MonoBehaviour {

        public static List<GameObject> actorList = new List<GameObject>();
        private static BattleMgr _instance;

        private DungeonData _dungeonData;
        private Dictionary<State, BattleState> _battleStates;
        private Dictionary<Type, string>  _profiles = new Dictionary<Type, string>(); //Temporary hero list
        private Type[] _party;
        private ActorSpawner _spawner;

        public List<GameObject> enemyList = new List<GameObject> ();

        public Entity NowActor { get; private set; }
        public UIMgr UiMgr { get; private set; }
        public AttackTracker AttackTracker { get; private set; }
        public BattleState CurrentState { get; private set; }

        public ElementType elementalAffinity = ElementType.FIRE;
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
            _dungeonData = FindObjectOfType<DungeonData>();
            if(_dungeonData != null)
            {
                elementalAffinity = _dungeonData.battleType.ToEnum<ElementType, BlockType>();
            }
            _party = new Type[] {
                typeof(Amelia),
                typeof(Claude),
                typeof(Tracy),
                typeof(Aria)
            };

            //
            InitBattleStates();
            _spawner = FindObjectOfType<ActorSpawner>();
            AttackTracker = GetComponent<AttackTracker>();
            UiMgr = GetComponent<UIMgr> ();
        }
        void Start()
        {
            SpawnHeroes();
            SpawnEnemies();
            CurrentState = _battleStates[State.PREPARE];
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
                hero.transform.localScale *= 0.8f; //Temporary
                hero.GetComponent<BoxCollider2D>().size *= 100;
                hero.GetComponent<Namebar>().spriteResource = hero.GetComponent<Profile>().nameplate;
                hero.name = hero.GetComponent<Profile>().GetType().ToString();
                actorList.Add(hero);
            }
        }

        private void SpawnEnemies()
        {
            Type[] enemies = _spawner.GetRandomEnemies();
            for(int i = 0; i < enemies.Length; i++)
            {
                var randomEnemy = _spawner.Spawn<Enemy>(enemies[i], "monster0" + i);

                _spawner.InitObj(randomEnemy, randomEnemy.GetComponent<Enemy>().components, _spawner.parentObject);

                randomEnemy.GetComponent<Enemy>().battleID = "e0" + i;
                randomEnemy.transform.position = new Vector3((enemies.Length / 2.5f - enemies.Length + i * 3f), 0.0f, -9);
                enemyList.Add(randomEnemy);
                actorList.Add(randomEnemy);
            }
        }

        public void InitBattleStates()
        {
            _battleStates = new Dictionary<State, BattleState>();
            _battleStates.Add(State.PREPARE       ,new StatePrepare());
            _battleStates.Add(State.RUNNING       ,new StateBattleRunning());
            _battleStates.Add(State.SELECT_SKILL  ,new StateSelectSkill());
            _battleStates.Add(State.SELECT_TARGET ,new StateSelectTarget());
            _battleStates.Add(State.ANIMATOIN     ,new StateAnimation());
            _battleStates.Add(State.PLAYER_WON    ,new StatePlayerWon());
        }

        public void SetState(State state)
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
                SetState(State.PLAYER_WON);
                Invoke("LoadLevelTitle", _attackAnimation + 0.5f);
            }
        }

        private void LoadLevelTitle()
        {
            enemyList.Clear();
            actorList.Clear();
            DeathSystem.deadEnemy.Clear();
            Application.LoadLevel("dungeon");
        }

        public void RemoveFromBattle(Entity e)
        {
            AttackTracker.DestroyActor(e);
            UiMgr.DestroyNameplate(e.battleID);
            actorList.RemoveAll(x => x.GetComponent<Entity>().Equals(e));
        }
    }

    public static class Extensions
    {
        public static T1 ToEnum<T1, T2>(this T2 sender)
        {
            string s = sender.ToString().ToUpper();
            return (T1)Enum.Parse(typeof(T1), s);
        }
    }
}