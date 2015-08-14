using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.Managers;
using Memoria.Dungeon.Items;

namespace Memoria.Dungeon
{
    public class DungeonData : MonoBehaviour
    {
        public DungeonParameter parameter { get; set; }

        public StageData stageData { get; set; }

        public BlockType battleType { get; private set; }

        private int direction;

        private Vector2Int location;

        private List<BlockData> mapData;

        private List<ItemData> itemDatas;

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

            // 初期化時
            if (!initialized)
            {
                int floor = 1;
                stageData = StageDataManager.instance.Prepare(floor);
                direction = 2;
                location = new Vector2Int(0, 0);

                mapData = LoadMapData("");

                itemDatas = new List<ItemData>(stageData.itemDatas);
                var keyNum = itemDatas.Count(item => item.type == ItemType.Key);

                parameter = new DungeonParameter(
                    maxHp: stageData.maxHp,
                    hp: stageData.maxHp,
                    maxSp: stageData.maxSp,
                    sp: stageData.maxSp,
                    floor: stageData.floor,
                    allKeyNum: keyNum,
                    silling: 0,
                    skill: "none");
            }

            (new GameObject()).AddComponent<SpriteRenderer>().sprite = stageData.areaSprite;
            player.direction = direction;
            player.SetPosition(location);

            mapManager.SetMap(mapData, stageData, itemDatas);

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
            mapData.AddRange(mapManager.blocks.Select(block => block.blockData));

            itemDatas.Clear();
            itemDatas.AddRange(mapManager.items.Select(item => item.itemData));

            parameter = parameterManager.parameter;
        }

        public void SetIsBossBattle(bool isBossBattle)
        {
            var param = parameter;
            param.isBossBattle = isBossBattle;
            parameter = param;
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