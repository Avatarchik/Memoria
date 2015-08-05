using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Memoria.Battle.States;
using Memoria.Battle.GameActors;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon;
using Memoria.Battle;

namespace Memoria.Battle.Managers
{
    public class BattleMgr : Singleton<BattleMgr> {

        [SerializeField]
        private DungeonData _dungeonData;

        [SerializeField]
        private ActorSpawner _spawner;

        [SerializeField]
        private UIMgr _uiMgr;

        [SerializeField]
        private AttackTracker _attackTracker;

        [SerializeField]
        private Entity _nowActor;

        [SerializeField]
        private string[] _party;

        private BattleState _currentState;
        private Dictionary<State, BattleState> _battleStates;
        private Type[] _profileType;

        private bool _setResultRunning;

        public List<GameObject> actorList;
        public Element elementalAffinity = Element.THUNDER;
        public List<GameObject> enemyList;
        public List<GameObject> heroList;
        public MainPlayer mainPlayer;

        public UIMgr UiMgr { get { return _uiMgr; } }
        public AttackTracker AttackTracker { get { return _attackTracker; } }

        public float AttackAnimation { get; set; }

        override protected void Init()
        {
            _dungeonData = FindObjectOfType<DungeonData>();

            if(_dungeonData != null)
            {
                elementalAffinity = _dungeonData.battleType.ToEnum<Element, BlockType>();
            }

            _party = new string[]
                {
                    "Amelia",
                    "Dhiel",
                    "Aria",
                    "Iska"
                };

            _profileType = new Type[]
                {
                    typeof(Amelia),
                    typeof(Dhiel),
                    typeof(Aria),
                    typeof(Iska)
                };

            InitBattleStates();

            mainPlayer = FindObjectOfType<MainPlayer>();
            mainPlayer.health.hp = 10000;

            actorList = new List<GameObject>();
            enemyList = new List<GameObject> ();
            _spawner = FindObjectOfType<ActorSpawner>();
            _attackTracker = FindObjectOfType<AttackTracker>();
            _uiMgr = FindObjectOfType<UIMgr> ();

        }

        void Start()
        {
            _spawner._defaultComponents.Add(typeof(BoxCollider2D));

            SpawnHeroes();
            SpawnEnemies();
           _currentState = _battleStates[State.PREPARE];
        }

        void Update ()
        {
            if(_currentState.Initialized)
            {
                _currentState.Update();
            }
            else
            {
                _currentState.PreInitialize(this, _uiMgr, _nowActor, _attackTracker);
                _currentState.Initialize();
                _currentState.Initialized = true;
            }
        }

        public void InitBattleStates()
        {
            _battleStates = new Dictionary<State, BattleState>()
                {
                    { State.PREPARE       ,new StatePrepare()       },
                    { State.RUNNING       ,new StateBattleRunning() },
                    { State.SELECT_SKILL  ,new StateSelectSkill()   },
                    { State.SELECT_TARGET ,new StateSelectTarget()  },
                    { State.ANIMATOIN     ,new StateAnimation()     },
                    { State.PLAYER_WON    ,new StatePlayerWon()     },
                    { State.PLAYER_LOST   ,new StatePlayerLost()    }
                };
        }

        public void SetState(State state)
        {
            _currentState.EndState();
            if(_battleStates.ContainsKey(state))
            {
                _currentState = _battleStates[state];
            }
        }

        public void SetCurrentActor()
        {
            _nowActor = _attackTracker.currentActor;
        }

        public bool BattleOver()
        {
            if (actorList.Count == 4 && !_setResultRunning)
            {
                StartCoroutine(SetResult(State.PLAYER_WON, AttackAnimation));
                return true;
            }
            if(mainPlayer.health.hp <= 0 && !_setResultRunning)
            {
                StartCoroutine(SetResult(State.PLAYER_LOST, AttackAnimation));
                return true;
            }
            return false;
        }

        public bool StateResult()
        {
            return (actorList.Count <= 4 || mainPlayer.health.hp <= 0);
        }

        public void LoadLevel(string scene)
        {
            EventMgr.Instance.Clear();
            UpdateParameters();
            Application.LoadLevel(scene);
        }
 
        public void RemoveFromBattle(Entity e)
        {
            EventMgr.Instance.Raise(new Memoria.Battle.Events.MonsterDies(e));

            var entityId = e.GetComponent<Entity>().battleID;
            _uiMgr.DestroyElement("Namebar_"+ entityId);
            _attackTracker.RemoveFromQueue(e);
            actorList.RemoveAll(x => x.GetComponent<Entity>().battleID.Equals(entityId));

        }

        private void SpawnHeroes()
        {
            Vector2 skillPos = new Vector2();
            for(int i = 0; i < _party.Length; i++)
            {
                var pos = new Vector3((3.8f / 1.5f - 4f + i) * 2.8f, -3.2f, -10);
                GameObject hero = _spawner.Spawn<Hero>("Chars/Char_" + _party[i], _profileType[i]);

                hero.LoadComponentsFromList(hero.GetComponent<Entity>().components);
                hero.transform.position = pos;

                //Change to relative positions
                float xOffset = (i < 2) ? 2.0f : -5.3f;
                skillPos.x = hero.transform.position.x + xOffset;
                skillPos.y = hero.transform.position.y;

                hero.name = hero.GetComponent<Profile>().GetType().ToString();
                hero.GetComponent<Profile>().skillPos = skillPos;
                hero.GetComponent<BoxCollider2D>().enabled = false;
                hero.GetComponent<Namebar>().spriteResource = hero.GetComponent<Profile>().nameplate;
                hero.GetComponent<Hero>().battleID = "h0" + i;
//                hero.GetComponent<ElementalPowerStock>().stock = _dungeonData.parameter.stocks[i];
                mainPlayer.health.maxHp += hero.GetComponent<Profile>().parameter.hp;
//                mainPlayer.health.maxHp = _dungeonData.parameter.maxHp;
//                mainPlayer.health.hp = _dungeonData.parameter.hp;
                heroList.Add(hero);
                actorList.Add(hero);
            }
        }

        private void SpawnEnemies()
        {
            Type[] enemies = GetRandomEnemies();
            for(int i = 0; i < enemies.Length; i++)
            {
                var pos = new Vector3((enemies.Length / 2.5f - enemies.Length + i * 3f), 0.0f, -9);
                GameObject randomEnemy = _spawner.Spawn<Enemy>("Monsters/monster0" + i, enemies[i]);

                randomEnemy.LoadComponentsFromList(randomEnemy.GetComponent<Entity>().components);
                randomEnemy.transform.position = pos;
                randomEnemy.GetComponent<Enemy>().battleID = "e0" + i;
                randomEnemy.GetComponent<Namebar>().spriteResource = randomEnemy.GetComponent<Profile>().nameplate;
                randomEnemy.GetComponent<BoxCollider2D>().enabled = false;
                enemyList.Add(randomEnemy);
                actorList.Add(randomEnemy);
            }
        }

        private void UpdateParameters()
        {
            var param = _dungeonData.parameter;
            param.hp = mainPlayer.health.hp;
            param.maxHp = mainPlayer.health.maxHp;
            for(int i = 0; i < _party.Length; i++)
            {
                param.stocks[i] = heroList[i].GetComponent<Hero>().power.stock;
            }
            _dungeonData.parameter = param;
        }

        private IEnumerator SetResult(State state, float waitTime)
        {
           _setResultRunning = true;
            yield return new WaitForSeconds (waitTime);
            SetState(state);
            yield return null;
        }

        // Temporary, final value likely to be recieved from dungeon data.
        private Type[] GetRandomEnemies()
        {
            Type[] result = { typeof(Golem) };
            return result;
        }

        private void SetStock()
        {
        }
    }
}
