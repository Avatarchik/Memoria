using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.Items
{
    public class ItemCreator : MonoBehaviour
    {        
        [SerializeField]
        private GameObject keyPrefab;
        [SerializeField]
        private GameObject jewelPrefab;
        [SerializeField]
        private GameObject soulPrefab;
        [SerializeField]
        private GameObject magicPlatePrefab;
        
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
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
            item.transform.position = (Vector3)MapManager.instance.ToPosition(itemData.location);

            return item;
        }
    }
}