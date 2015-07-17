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
		public List<Vector2Int> keyLocations;
		
		public List<JewelData> jewelDatas;
    }
}