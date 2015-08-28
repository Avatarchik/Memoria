using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UniRx;
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
        public int enemyPattern { get; private set; }
        public bool isBossBattle { get; private set; }

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
                dungeonManager.EnterState(DungeonState.Initialize);
                int floor = 0;
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

                Observable.Return(1)
                    .Delay(System.TimeSpan.FromSeconds(1.5f))
                    .Do(_ =>
                    {
                        Animator ui = GameObject.Find("Canvas").GetComponent<Animator>();
                        ui.SetFloat("floor", stageData.floor);
                        ui.SetTrigger("show");
                    })
                    .Delay(System.TimeSpan.FromSeconds(1))
                    .Subscribe(_ => 
                    {
                        dungeonManager.ExitState();
                    });
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
            this.isBossBattle = isBossBattle;
        }

        public void SetBattleType(BlockType battleType)
        {
            this.battleType = battleType;
        }
        
        public void SetEnemyPattern(int id)
        {
            this.enemyPattern = id;
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