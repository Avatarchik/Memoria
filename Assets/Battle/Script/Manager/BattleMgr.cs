using UnityEngine;
using System.Collections.Generic;
using System;
using Memoria.Battle.States;
using Memoria.Battle.GameActors;
using Memoria.Battle.Utility;
using Memoria.Dungeon.BlockUtility;
using Memoria.Dungeon;
using Memoria.Battle;

namespace Memoria.Battle.Managers
{
    public class BattleMgr : MonoBehaviour {

        public static List<GameObject> actorList = new List<GameObject>();
        private static BattleMgr _instance;

        private DungeonData _dungeonData;
        private Dictionary<State, BattleState> _battleStates;

        private string[] _party;
        private Type[] _profileType;

        private ActorSpawner _spawner;

        public List<GameObject> enemyList = new List<GameObject> ();

        public Entity NowActor { get; private set; }
        public UIMgr UiMgr { get; private set; }
        public AttackTracker AttackTracker { get; private set; }
        public BattleState CurrentState { get; private set; }

        public Element elementalAffinity = Element.FIRE;
        public float AttackAnimation { get; set; }

        public static BattleMgr Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<BattleMgr>() as BattleMgr;
                }
                return _instance;
            }
        }

        void Awake ()
        {
            _dungeonData = FindObjectOfType<DungeonData>();

            if(_dungeonData != null)
            {
                elementalAffinity = _dungeonData.battleType.ToEnum<Element, BlockType>();
            }

            _party = new string[]
                {
                    "Amelia",
                    "Tracy",
                    "Aria",
                    "Claude"
                };

            _profileType = new Type[]
                {
                    typeof(Amelia),
                    typeof(Tracy),
                    typeof(Aria),
                    typeof(Claude)
                };

            InitBattleStates();
            _spawner = FindObjectOfType<ActorSpawner>();
            AttackTracker = GetComponent<AttackTracker>();
            UiMgr = GetComponent<UIMgr> ();
        }

        void Start()
        {
            _spawner._defaultComponents.Add(typeof(BoxCollider2D));

            SpawnHeroes();
            SpawnEnemies();
            CurrentState = _battleStates[State.PREPARE];
        }

        void Update () {
            CheckWinLoss();
            if(CurrentState.Initialized)
            {
                CurrentState.Update();
            }
            else
            {
                CurrentState.PreInitialize(this, UiMgr, NowActor);
                CurrentState.Initialize();
                CurrentState.Initialized = true;
            }
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
//                hero.transform.localScale *= 0.8f; //Temporary

                //Change to relative positions
                float xOffset = (i < 2) ? 2.0f : -5.3f;
                skillPos.x = hero.transform.position.x + xOffset;
                skillPos.y = hero.transform.position.y;

                hero.name = hero.GetComponent<Profile>().GetType().ToString();
                hero.GetComponent<Profile>().skillPos = skillPos;
                hero.GetComponent<BoxCollider2D>().enabled = false;
                hero.GetComponent<Namebar>().spriteResource = hero.GetComponent<Profile>().nameplate;
                hero.GetComponent<Hero>().battleID = "h0" + i;

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

        public void InitBattleStates()
        {
            _battleStates = new Dictionary<State, BattleState>()
                {
                    { State.PREPARE       ,new StatePrepare()       },
                    { State.RUNNING       ,new StateBattleRunning() },
                    { State.SELECT_SKILL  ,new StateSelectSkill()   },
                    { State.SELECT_TARGET ,new StateSelectTarget()  },
                    { State.ANIMATOIN     ,new StateAnimation()     },
                    { State.PLAYER_WON    ,new StatePlayerWon()     }
                };
        }

        public void SetState(State state)
        {
            CurrentState.EndState();
            if(_battleStates.ContainsKey(state))
            {
                CurrentState = _battleStates[state];
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
                Invoke("LoadLevelTitle", AttackAnimation + 0.5f);
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
            UiMgr.DestroyElement(e.GetComponent<Namebar>().spriteResource);
            actorList.RemoveAll(x => x.GetComponent<Entity>().Equals(e));
        }

        private Type[] GetRandomEnemies()
        {
            Type[] result = { typeof(Golem) };
            return result;
        }

    }
}
