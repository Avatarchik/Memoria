using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.BlockUtility;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon
{

	public class DungeonData : MonoBehaviour
	{
		public int direction;
		public Vector2Int location;

		public DungeonParameter parameter { get; set; }

		public List<BlockData> mapData { get; set; }

		public BlockType battleType { get; private set; }

		public int[] stocks { get; set; }

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
			ParameterManager parameterManager = dungeonManager.parameterManager;

			Player player = dungeonManager.player;

			if (!initialized)
			{
				direction = 2;
				location = new Vector2Int(0, 0);

				mapData = LoadMapData("");
				parameter = new DungeonParameter();
				parameter.Set(100, 100, 100, 100, 1, "none");

				stocks = new [] { 0, 0, 0, 0 };

				Debug.Log("initialize!!");
			}

			player.direction = direction;
			player.SetPosition(location);

			mapManager.SetMap(mapData);

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
			ParameterManager parameterManager = dungeonManager.parameterManager;

			Player player = dungeonManager.player;

			direction = player.direction;
			location = player.location;

			mapData.Clear();
			mapData.AddRange(mapManager.map.Values.Select(block => block.blockData));

			parameter.Set(parameterManager.parameter);
		}

		public void SetBattleType(BlockType battleType)
		{
			this.battleType = battleType;
		}

		public List<BlockData> LoadMapData(string mapDataPath)
		{
			print("load map : " + mapDataPath);

			List<BlockData> result = new List<BlockData>();

			// Dummy Data
			result.Add(new BlockData(Vector2Int.zero, new ShapeData(10), BlockType.None));

			return result;
		}
	}
}