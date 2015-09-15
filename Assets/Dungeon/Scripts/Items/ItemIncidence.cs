using UnityEngine;

namespace Memoria.Dungeon.Items
{
    [System.Serializable]
    public struct ItemIncidence
    {
        public float spawn;
        public float jewel;
        public float soul;
        public float magicPlate;
        
        public float GetIncidence(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Jewel:
                    return jewel;
                    
                case ItemType.Soul:
                    return soul;
                    
                case ItemType.MagicPlate:
                    return magicPlate;
            }
            
            throw new UnityException("It could not be converted to incidence from the item-type `" + itemType + "`");
        }
        
        public ItemType GetRandomItemType()
        {
            float rangeOfJewel = 0 + jewel;
            float rangeOfSoul = rangeOfJewel + soul;
            float rangeOfMagicPlate = rangeOfSoul + magicPlate;
            
            float value = Random.Range(0, rangeOfMagicPlate);
            if (value < rangeOfJewel)
            {
                return ItemType.Jewel;
            }
            else if (value < rangeOfSoul)
            {
                return ItemType.Soul;
            }
            else
            {
                return ItemType.MagicPlate;
            }
        }
    }
}