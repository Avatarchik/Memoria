using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Memoria.Dungeon.BlockComponent;
using Memoria.Dungeon.Items;

namespace Memoria.Dungeon.Managers
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance { get { return DungeonManager.instance.mapManager; } }

        public GameObject keyPrefab;
        public GameObject jewelPrefab;
		
        private Dictionary<Vector2Int, Block> map = new Dictionary<Vector2Int, Block>();
        private Dictionary<Vector2Int, Key> keyMap = new Dictionary<Vector2Int, Key>();
        private Dictionary<Vector2Int, Jewel> jewelMap = new Dictionary<Vector2Int, Jewel>();

        public List<Block> blocks { get { return map.Values.ToList(); } }
        public List<Vector2Int> blockLocations { get { return map.Keys.ToList(); } }

        public List<Key> keys { get { return keyMap.Values.ToList(); } }
        public List<Vector2Int> keyLocations { get { return keyMap.Keys.ToList(); } }

        public List<Jewel> jewels { get { return jewelMap.Values.ToList(); } }
        public List<Vector2Int> jewelLocations { get { return jewelMap.Keys.ToList(); } }

        private Rect _canPutBlockArea = new Rect(-7, -5, 14, 10);
        private Rect stageArea;

        public Rect canPutBlockArea
        {
            get
            {
                Rect ret = _canPutBlockArea;
                ret.position += (Vector2)Camera.main.transform.position;

                ret.yMin = Mathf.Max(ret.yMin, stageArea.yMin);
                ret.yMax = Mathf.Min(ret.yMax, stageArea.yMax);
                ret.xMin = Mathf.Max(ret.xMin, stageArea.xMin);
                ret.xMax = Mathf.Min(ret.xMax, stageArea.xMax);

                return ret;
            }
        }

        void Awake()
        {
            BlockManager.instance.OnCreateBlockAsObservable()
                .Subscribe(block =>
                {
                    var onPut = block.OnPutAsObservable()
                        .Subscribe(_ => map.Add(block.location, block));

                    block.OnBreakAsObservable()
                        .Do(_ => onPut.Dispose())
                        .Subscribe(_ => map.Remove(block.location))
                        .AddTo(block.gameObject);
                });
        }

        public void SetMap(List<BlockData> blockDatas, StageData stageData, List<Vector2Int> keyLocations, List<JewelData> jewelDatas)
        {
            blockDatas.ForEach(data => BlockManager.instance.CreateBlockAsDefault(data));
            stageArea = stageData.stageSize;

            keyLocations
                .Select(location =>
                {
                    var key = Instantiate<GameObject>(keyPrefab).GetComponent<Key>();
                    key.transform.position = (Vector3)ToPosition(location);
                    return new { location, key };
                })
                .ToList()
                .ForEach(keyData =>
                {
                    keyMap.Add(keyData.location, keyData.key);
                });

            jewelDatas
                .Select(data =>
                {
                    var jewel = Instantiate<GameObject>(jewelPrefab).GetComponent<Jewel>();
                    jewel.jewelData = data;
                    jewel.transform.position = (Vector3)ToPosition(data.location);
                    return new { data.location, jewel };
                })
                .ToList()
                .ForEach(jewelData =>
                {
                    jewelMap.Add(jewelData.location, jewelData.jewel);
                });
        }

        public bool ExistsBlock(Vector2Int location)
        {
            return map.ContainsKey(location);
        }

        public bool ExistsKey(Vector2Int location)
        {
            return keyMap.ContainsKey(location);
        }

        public bool ExistsJewel(Vector2Int location)
        {
            return jewelMap.ContainsKey(location);
        }

        public Block GetBlock(Vector2Int location)
        {
            return map[location];
        }

        public Key GetKey(Vector2Int location)
        {
            return keyMap[location];
        }

        public Jewel GetJewel(Vector2Int location)
        {
            return jewelMap[location];
        }

        /// <summary>
        /// 指定の位置からマップ上に配置されるときの位置を取得する
        /// </summary>
        /// <returns>マップ上に配置されるときの位置</returns>
        /// <param name="position">指定の位置</param>
        public Vector2 ConvertPosition(Vector2 position)
        {
            Vector2Int location = ToLocation(position);
            Vector2 converted = ToPosition(location);
            return converted;
        }

        /// <summary>
        /// 指定の位置からマップ座標を取得する
        /// </summary>
        /// <returns>マップ座標</returns>
        /// <param name="position">指定の位置</param>
        public Vector2Int ToLocation(Vector2 position)
        {
            Vector2 blockSize = DungeonManager.instance.blockSize;
            Vector2Int location = new Vector2Int();

            location.x = (int)Mathf.Round(position.x / blockSize.x * 100);
            location.y = (int)Mathf.Round(position.y / blockSize.y * 100);

            return location;
        }

        /// <summary>
        /// 指定のマップ座標から位置を取得する
        /// </summary>
        /// <returns>位置</returns>
        /// <param name="location">指定のマップ座標</param>
        public Vector2 ToPosition(Vector2Int location)
        {
            Vector2 blockSize = DungeonManager.instance.blockSize;
            Vector2 position = new Vector3();

            position.x = location.x / 100f * blockSize.x;
            position.y = location.y / 100f * blockSize.y;

            return position;
        }
    }
}