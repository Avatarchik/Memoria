using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Memoria.Dungeon.BlockComponent;

namespace Memoria.Dungeon.Managers
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance { get { return DungeonManager.instance.mapManager; } }
        
        public GameObject keyPrefab;

        /// <summary>
        /// マップ
        /// </summary>
        public Dictionary<Vector2Int, Block> map = new Dictionary<Vector2Int, Block>();
        public List<GameObject> keys = new List<GameObject>();

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

        public void SetMap(List<BlockData> blockDatas, StageData stageData, List<Vector2Int> keyLocations)
        {
            blockDatas.ForEach(data => BlockManager.instance.CreateBlockAsDefault(data));
            stageArea = stageData.stageSize;
            
            keys.AddRange(keyLocations
                .Select(location => (Vector3)ToPosition(location))
                .Select(position => Instantiate(keyPrefab, position, Quaternion.identity) as GameObject));
            
            //  keyLocations.ForEach(location =>
            //      {
            //          Instantiate(keyPrefab, (Vector3)ToPosition(location), Quaternion.identity); 
            //      });
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