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

        [SerializeField]
        private GameObject keyPrefab;
        [SerializeField]
        private GameObject jewelPrefab;
        [SerializeField]
        private GameObject soulPrefab;
        [SerializeField]
        private GameObject magicPlatePrefab;

        private Dictionary<Vector2Int, Block> map = new Dictionary<Vector2Int, Block>();
        private Dictionary<Vector2Int, Item> itemMap = new Dictionary<Vector2Int, Item>();
        //  private Dictionary<Vector2Int, Item> keyMap { get { return GetItemMap(ItemType.Key); } }
        //  private Dictionary<Vector2Int, Item> jewelMap { get { return GetItemMap(ItemType.Jewel); } }
        //  private Dictionary<Vector2Int, Item> soulMap { get { return GetItemMap(ItemType.Soul); } }
        //  private Dictionary<Vector2Int, Item> magicPlateMap { get { return GetItemMap(ItemType.MagicPlate); } }

        public List<Block> blocks { get { return map.Values.ToList(); } }
        //  public List<Vector2Int> blockLocations { get { return map.Keys.ToList(); } }

        public List<Item> items { get { return itemMap.Values.ToList(); } }
        //  public List<Vector2Int> itemsLocations { get { return itemMap.Keys.ToList(); } }

        //  public List<Item> keys { get { return GetItems(ItemType.Key); } }
        //  public List<Vector2Int> keyLocations { get { return GetItemLocations(ItemType.Key); } }

        //  public List<Item> jewels { get { return GetItems(ItemType.Jewel); } }
        //  public List<Vector2Int> jewelLocations { get { return GetItemLocations(ItemType.Jewel); } }

        //  public List<Item> souls { get { return GetItems(ItemType.Soul); } }
        //  public List<Vector2Int> soulLocations { get { return GetItemLocations(ItemType.Soul); } }

        //  public List<Item> magicPlates { get { return GetItems(ItemType.MagicPlate); } }
        //  public List<Vector2Int> magicPlateLocations { get { return GetItemLocations(ItemType.MagicPlate); } }

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

        public void SetMap(List<BlockData> blockDatas, StageData stageData, List<ItemData> itemDatas)
        {
            blockDatas.ForEach(data => BlockManager.instance.CreateBlockAsDefault(data));
            stageArea = stageData.stageSize;

            itemDatas
                .Select(itemData => CreateItem(itemData))
                .ToList()
                .ForEach(item =>
                {
                    itemMap.Add(item.itemData.location, item);
                });
        }

        public bool ExistsBlock(Vector2Int location)
        {
            return map.ContainsKey(location);
        }

        public bool ExistsItem(Vector2Int location)
        {
            return itemMap.ContainsKey(location);
        }

        //  public bool ExistsKey(Vector2Int location)
        //  {
        //      return keyLocations.Contains(location);
        //  }

        //  public bool ExistsJewel(Vector2Int location)
        //  {
        //      return jewelLocations.Contains(location);
        //  }

        //  public bool ExistsSoul(Vector2Int location)
        //  {
        //      return soulLocations.Contains(location);
        //  }

        //  public bool ExistsMagicPlate(Vector2Int location)
        //  {
        //      return magicPlateLocations.Contains(location);
        //  }

        public Block GetBlock(Vector2Int location)
        {
            return map[location];
        }

        public Item GetItem(Vector2Int location)
        {
            return itemMap[location];
        }

        //  public Item GetKey(Vector2Int location)
        //  {
        //      return keyMap[location];
        //  }

        //  public Item GetJewel(Vector2Int location)
        //  {
        //      return jewelMap[location];
        //  }

        //  public Item GetSoul(Vector2Int location)
        //  {
        //      return soulMap[location];
        //  }

        //  public Item GetMagicPlate(Vector2Int location)
        //  {
        //      return magicPlateMap[location];
        //  }
		
		public void TakeItem(Item item)
		{
			OnTakeItem(item);
			itemMap.Remove(item.itemData.location);
			Destroy(item.gameObject);
		}

        private Item CreateItem(ItemData itemData)
        {
            Item item = null;

            switch (itemData.type)
            {
                case ItemType.Key:
                    item = Instantiate<GameObject>(keyPrefab).GetComponent<Item>();
                    break;

                case ItemType.Jewel:
                    item = Instantiate<GameObject>(jewelPrefab).GetComponent<Item>();
                    break;

                case ItemType.Soul:
                    item = Instantiate<GameObject>(soulPrefab).GetComponent<Item>();
                    break;

                case ItemType.MagicPlate:
                    item = Instantiate<GameObject>(magicPlatePrefab).GetComponent<Item>();
                    break;
            }

            item.itemData = itemData;
            item.transform.position = (Vector3)ToPosition(itemData.location);

            return item;
        }

        //  private Dictionary<Vector2Int, Item> GetItemMap(ItemType type)
        //  {
        //      return itemMap
        //          .Where(item => item.Value.itemData.type == type)
        //          .ToDictionary(item => item.Key, item => item.Value);
        //  }

        //  private List<Item> GetItems(ItemType type)
        //  {
        //      return itemMap
        //          .Where(item => item.Value.itemData.type == type)
        //          .Select(item => item.Value)
        //          .ToList();
        //  }

        //  private List<Vector2Int> GetItemLocations(ItemType type)
        //  {
        //      return itemMap
        //          .Where(item => item.Value.itemData.type == type)
        //          .Select(item => item.Key)
        //          .ToList();
        //  }

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