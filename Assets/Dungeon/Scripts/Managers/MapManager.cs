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

        private Dictionary<Vector2Int, Block> map = new Dictionary<Vector2Int, Block>();
        private Dictionary<Vector2Int, Item> itemMap = new Dictionary<Vector2Int, Item>();

        public List<Block> blocks { get { return map.Values.ToList(); } }

        public List<Item> items { get { return itemMap.Values.ToList(); } }

        private Rect _canPutBlockArea = new Rect(-7, -5, 14, 10);
        public Rect stageArea { get; private set; }

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

        private Subject<Item> onTakeItem;

        public IObservable<Item> OnTakeItemAsObservable()
        {
            return onTakeItem ?? (onTakeItem = new Subject<Item>());
        }

        private void OnTakeItem(Item item)
        {
            if (onTakeItem != null)
            {
                onTakeItem.OnNext(item);
            }
        }

        void Awake()
        {
            // Block の Map に関わるイベント
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

            // Item の Map に関わるイベント 
            ItemManager.instance.OnCreateItemAsObservable()
                .Subscribe(item =>
                {
                    itemMap.Add(item.itemData.location, item);

                    item.OnTakeAsObservable()
                        .Subscribe(_ =>
                        {
                            itemMap.Remove(item.itemData.location);
                        });
                });
        }

        public void SetMap(List<BlockData> blockDatas, StageData stageData, List<ItemData> itemDatas)
        {
            blockDatas.ForEach(data => BlockManager.instance.CreateBlockAsDefault(data));
            stageArea = stageData.stageSize;

            itemDatas.ForEach(itemData => ItemManager.instance.CreateItem(itemData));
        }

        public bool ExistsBlock(Vector2Int location)
        {
            return map.ContainsKey(location);
        }

        public bool ExistsItem(Vector2Int location)
        {
            return itemMap.ContainsKey(location);
        }

        public Block GetBlock(Vector2Int location)
        {
            return map[location];
        }

        public Item GetItem(Vector2Int location)
        {
            return itemMap[location];
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