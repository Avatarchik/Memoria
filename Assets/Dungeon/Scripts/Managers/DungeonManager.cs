using UnityEngine;
using System.Collections.Generic;
using UniRx;

namespace Memoria.Dungeon.Managers
{
    public enum DungeonState
    {
        None,
        BlockOperating,
        PlayerMoving,
        BlockEvent,
		StockTaking,
        OpenMenu,
        LeaveSelect,
        MapViewer,
        TipsViewer
    }

    public class DungeonManager : MonoBehaviour
    {
        private static DungeonManager _instance = null;

        public static DungeonManager instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = GameObject.FindObjectOfType<DungeonManager>();

                    if (!_instance)
                    {
                        throw new UnityException("Dungeon Manager is not found.");
                    }
                }

                return _instance;
            }
        }

        [SerializeField]
        private Player _player;

        public Player player { get { return _player; } }

        #region Manager

        [SerializeField]
        private EventManager _eventManager;

        public EventManager eventManager { get { return _eventManager; } }

        [SerializeField]
        private MapManager _mapManager;

        public MapManager mapManager { get { return _mapManager; } }

        [SerializeField]
        private BlockManager _blockManager;

        public BlockManager blockManager { get { return _blockManager; } }

        [SerializeField]
        private ParameterManager _parameterManager;

        public ParameterManager parameterManager { get { return _parameterManager; } }

        [SerializeField]
        private StageDataManager _stageDataManager;

        public StageDataManager stageDataManager { get { return _stageDataManager; } }

        #endregion


        #region State

        private Stack<DungeonState> states = new Stack<DungeonState>();

        public DungeonState activeState { get { return activeStateProperty.Value; } }

        private ReactiveProperty<DungeonState> activeStateProperty = new ReactiveProperty<DungeonState>();

        #endregion

        public Vector2 blockSize = new Vector2(200, 200);

        [SerializeField]
        private GameObject dungeonDataPrefab;

        private DungeonData _dungeonData;

        public DungeonData dungeonData
        {
            get
            {
                if (!_dungeonData)
                {
                    _dungeonData = FindObjectOfType<DungeonData>();

                    if (!_dungeonData)
                    {
                        _dungeonData = Instantiate<GameObject>(dungeonDataPrefab).GetComponent<DungeonData>();
                    }
                }

                return _dungeonData;
            }
        }

        void Start()
        {
            // DungeonManager の重複チェック
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            EnterState(DungeonState.None);

            dungeonData.Load();
        }

        public void EnterState(DungeonState nextState)
        {
            states.Push(nextState);
            activeStateProperty.Value = nextState;
        }

        public void ExitState()
        {
            if (activeState == DungeonState.None)
            {
                throw new UnityException("State Stack Error!!");
            }

            states.Pop();
            DungeonState nextState = states.Peek();
            activeStateProperty.Value = nextState;
        }

        public void Leave()
        {
            Destroy(dungeonData.gameObject);
            Application.LoadLevel("menu");
        }
    }
}