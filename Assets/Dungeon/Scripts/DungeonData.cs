using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DungeonData : MonoBehaviour
{
	public int direction;
	public Location location;

	public DungeonParameter parameter { get; set; }

	public List<BlockData> mapData { get; set; }

	public string[] heros { get; set; }

	public List<List<BlockData>> blockDataLists { get; set; }

	private bool initialized = false;

	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void Load()
	{
		DungeonManager dungeonManager = DungeonManager.instance;
		MapManager mapManager = dungeonManager.mapManager;
		BlockManager blockManager = dungeonManager.blockManager;
		ParameterManager parameterManager = dungeonManager.parameterManager;

		Player player = dungeonManager.player;

		if (!initialized)
		{
			direction = 2;
			location = new Location(0, 0);

			mapData = LoadMapData("");
			blockDataLists = GetColorBlockDataLists();
			parameter = new DungeonParameter();
			parameter.Set(1, 100, 100, 100, 100, 1, "none", 0);

			Debug.Log("initialize!!");
		}

		player.direction = direction;
		player.SetPosition(location);

		mapManager.SetMap(mapData);

		//blockManager.SetColorBlockList(blockDataLists);
		blockManager.ActivateColorBlockList(0);

		parameterManager.SetParamater(parameter);

		if (initialized)
		{
			dungeonManager.eventManager.ReturnFromBattle();
		}

		initialized = true;
	}

	public void Save()
	{
		DungeonManager dungeonManager = DungeonManager.instance;
		MapManager mapManager = dungeonManager.mapManager;
		//BlockManager blockManager = dungeonManager.blockManager;
		ParameterManager parameterManager = dungeonManager.parameterManager;

		Player player = dungeonManager.player;

		direction = player.direction;
		location = player.location;

		mapData.Clear();
		mapData.AddRange(mapManager.map.Values.Select(block => block.blockData));

		blockDataLists.Clear();
		//blockDataLists.AddRange(
		//	blockManager.colorBlockLists
		//	.Select(blockList =>
		//		new List<BlockData>()));
		//blockList.blockFactors.Select(blockFactor => blockFactor.block.blockData).ToList()));

		parameter.Set(parameterManager.parameter);
	}

	public List<BlockData> LoadMapData(string mapDataPath)
	{
		print("load map : " + mapDataPath);

		List<BlockData> result = new List<BlockData>();

		result.Add(new BlockData(new Location(0, 0), new BlockShape(10), BlockType.None, false));

		return result;
	}

	private static List<List<BlockData>> GetColorBlockDataLists()
	{
		List<BlockType> blockTypes = new List<BlockType>()
        {
            BlockType.None,
            BlockType.Fire,
            BlockType.Wind,
            BlockType.Thunder,
			BlockType.Water,
            BlockType.Recovery,
        };

		List<List<BlockData>> colorBlockDataLists = new List<List<BlockData>>();
		foreach (BlockType blockType in blockTypes)
		{
			List<BlockData> blockDatas = new List<BlockData>();

			for (int i = 0; i < 11; i++)
			{
				blockDatas.Add(new BlockData() { shape = new BlockShape(i), type = blockType, hasEvent = true });
			}

			colorBlockDataLists.Add(blockDatas);
		}

		return colorBlockDataLists;
	}

	// Update is called once per frame
	//void Update()
	//{

	//}
}