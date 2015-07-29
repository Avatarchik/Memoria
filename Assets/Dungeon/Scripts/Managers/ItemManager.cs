﻿using UnityEngine;
using Memoria.Dungeon.Items;

namespace Memoria.Dungeon.Managers
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance { get { return DungeonManager.instance.itemManager; } }

        [SerializeField]
        private GameObject keyPrefab;
        [SerializeField]
        private GameObject jewelPrefab;
        [SerializeField]
        private GameObject soulPrefab;
        [SerializeField]
        private GameObject magicPlatePrefab;

        public Item CreateItem(ItemData itemData)
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

            return item;
        }
    }
}