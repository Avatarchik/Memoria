using UnityEngine;
using System.Collections.Generic;
using UniRx;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.Items
{
    public class ItemSpawner : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            var player = DungeonManager.instance.player;

            System.Func<Rect, Vector2Int> locationSelector = stageSize =>
            {
                Vector2Int location;

                do
                {
                    location = new Vector2Int(
                        x: (int)Random.Range(stageSize.xMin / 2, stageSize.xMax / 2),
                        y: (int)Random.Range(stageSize.yMin / 2, stageSize.yMax / 2)
                    );
                }
                while (MapManager.instance.ExistsItem(location) || location == player.location);

                return location;
            };

            EventManager.instance.OnEndBlockEventAsObservable()
                .Select(_ => DungeonManager.instance.dungeonData.stageData)
                .Where(stageData => Random.value < stageData.itemIncidence.spawn)
                .Subscribe(stageData =>
                {
                    var itemIncidence = stageData.itemIncidence;
                    Dictionary<ItemType, AttributeIncidence> attributeIncidenceTable = new Dictionary<ItemType, AttributeIncidence>()
                    {
                        { ItemType.Jewel, stageData.attributeIncidenceOfJewel },
                        { ItemType.Soul, stageData.attributeIncidenceOfSoul },
                        { ItemType.MagicPlate, stageData.attributeIncidenceOfMagicPlate }
                    };

                    SpawnItem(itemIncidence, attributeIncidenceTable, () => locationSelector(stageData.stageSize));
                });
        }

        // Update is called once per frame
        //  void Update()
        //  {

        //  }

        public Item SpawnItem(ItemIncidence itemIncidence, Dictionary<ItemType, AttributeIncidence> attributeIncidenceTable, System.Func<Vector2Int> locationSelector)
        {
            ItemData itemData = new ItemData();

            // アイテムの種類を決定　
            itemData.type = itemIncidence.GetRandomItemType();

            // アイテムの属性を決定
            itemData.attribute = attributeIncidenceTable[itemData.type].GetRandomAttribute();

            // アイテムの位置を決定
            itemData.location = locationSelector();

            itemData.visible = true;

            Item item = ItemManager.instance.CreateItem(itemData);
            return item;
        }
    }
}