using UnityEngine;
using System.Collections.Generic;
using Memoria.Dungeon.Items;

namespace Memoria.Dungeon
{
	[System.Serializable]
    public struct StageData
    {
		public string areaSpritePath;
		public Rect stageSize;

        [HideInInspector]
        public int floor;
        public int maxHp;
        public int maxSp;

        public List<ItemData> itemDatas;
    }
}