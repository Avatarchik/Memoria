﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum DungeonState
{
	None,
	BlockOperating,
	BlockPutting,
	PlayerMoving,
	BlockEvent,
	CharacterEvent,
    OpenMenu,
    LeaveSelect,
	MapViewer,
}

public class ChangeDungeonStateEventArgs : EventArgs
{
	public DungeonState nowState { get; private set; }
	public DungeonState nextState { get; private set; }

	public ChangeDungeonStateEventArgs(DungeonState nowState, DungeonState nextState)
	{
		this.nowState = nowState;
		this.nextState = nextState;
	}
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
    private Player
        _player;

    public Player player { get { return _player; } }

	[SerializeField]
	private EventManager
		_eventManager;

    public EventManager eventManager { get { return _eventManager; } }

	[SerializeField]
	private MapManager
		_mapManager;

    public MapManager mapManager { get { return _mapManager; } }

    [SerializeField]
    private BlockManager
        _blockManager;

    public BlockManager blockManager { get { return _blockManager; } }

    [SerializeField]
    private ParameterManager _parameterManager;

    public ParameterManager parameterManager { get { return _parameterManager; } }

    public Vector2 blockSize = new Vector2(200, 200);

	private Stack<DungeonState> states = new Stack<DungeonState>(new [] { DungeonState.None });

	public DungeonState activeState { get { return states.Peek(); } }

	public event EventHandler<ChangeDungeonStateEventArgs> changingDungeonState = (s, e) => 
    {
//        Debug.Log("changing now : " + e.nowState + ", next : " + e.nextState);
    };
    public event EventHandler<ChangeDungeonStateEventArgs> changedDungeonState = (s, e) =>
    {
//        Debug.Log("changed now : " + e.nowState + ", next : " + e.nextState);        
    };

    public Block operatingBlock { get; set; }

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

	void Awake()
	{
		// DungeonManager の重複チェック
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
			return;
		}

		_instance = this;
		//DontDestroyOnLoad(gameObject);
		dungeonData.Load();
	}

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	public void EnterState(DungeonState nextState)
	{
		var args = new ChangeDungeonStateEventArgs(activeState, nextState);
		changingDungeonState(this, args);
		states.Push(nextState);
        changedDungeonState(this, args);
	}

	public void ExitState()
	{
		if (activeState == DungeonState.None)
		{
			throw new UnityException("State Stack Error!!");
		}

		DungeonState nextState = states.ElementAt(1);
		var args = new ChangeDungeonStateEventArgs(activeState, nextState);
		changingDungeonState(this, args);
		states.Pop();
        changedDungeonState(this, args);
	}

	public void Leave()
	{
		Destroy(dungeonData.gameObject);
		Application.LoadLevel("menu");
	}
}