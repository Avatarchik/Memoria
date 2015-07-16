﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon
{
    public class DungeonData : MonoBehaviour
    {
        public DungeonParameter parameter { get; set; }

        public BlockType battleType { get; private set; }
        
        private int direction;
        
        private Vector2Int location;
        
        private List<BlockData> mapData;
        
        private List<Vector2Int> keyLocations;

        // TODO : CreateJewelData
        private List<Vector2Int> jewelLocations;

        public int[] stocks { get; set; }

        private bool initialized = false;

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Load()
        {
            var dungeonManager = DungeonManager.instance;
            var mapManager = MapManager.instance;
            var parameterManager = ParameterManager.instance;

            var player = dungeonManager.player;
            var stageData = StageDataManager.instance.Prepare(parameter.floor);

            // 初期化時
            if (!initialized)
            {
                direction = 2;
                location = new Vector2Int(0, 0);

                mapData = LoadMapData("");
                keyLocations = new List<Vector2Int>(stageData.keyLocations);
                jewelLocations = new List<Vector2Int>(stageData.jewelLocations);
                parameter = new DungeonParameter(100, 100, 100, 100, 0, keyLocations.Count, 1000, "none");
                stocks = new[] { 0, 0, 0, 0 };
            }

            player.direction = direction;
            player.SetPosition(location);

            mapManager.SetMap(mapData, stageData, keyLocations, jewelLocations);

            parameterManager.parameter = parameter;

            if (initialized)
            {
                dungeonManager.eventManager.ReturnFromBattle();
            }

            initialized = true;
        }

        public void Save()
        {
            var dungeonManager = DungeonManager.instance;
            var mapManager = dungeonManager.mapManager;
            var parameterManager = dungeonManager.parameterManager;

            var player = dungeonManager.player;

            direction = player.direction;
            location = player.location;

            mapData.Clear();
            mapData.AddRange(mapManager.map.Values.Select(block => block.blockData));
            
            keyLocations.Clear();
            keyLocations.AddRange(mapManager.keys.Select(key => mapManager.ToLocation(key.transform.position)));
            
            parameter = parameterManager.parameter;
        }

        public void SetBattleType(BlockType battleType)
        {
            this.battleType = battleType;
        }

        public List<BlockData> LoadMapData(string mapDataPath)
        {
            print("load map : " + mapDataPath);

            var result = new List<BlockData>();

            result.Add(new BlockData(Vector2Int.zero, new ShapeData(10), BlockType.None));

            return result;
        }
    }
}