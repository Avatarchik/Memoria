using UnityEngine;
using System.Collections.Generic;
using Memoria.Dungeon.Items;

namespace Memoria.Dungeon
{
	[System.Serializable]
    public struct StageData
    {
		public Rect stageSize;

        public Sprite areaSprite;
        public string areaSpritePath;
        public int floor;
        public int maxHp;
        public int maxSp;

        public float probabilityOfEncounter;
        
        public ItemIncidence itemIncidence;
        public AttributeIncidence attributeIncidenceOfJewel;
        public AttributeIncidence attributeIncidenceOfSoul;
        public AttributeIncidence attributeIncidenceOfMagicPlate;
        
        public List<ItemData> itemDatas;
    } 
}