using UnityEngine;
using System.Collections.Generic;

namespace Memoria.Dungeon
{
	[System.Serializable]
    public struct StageData
    {
		public string areaSpritePath;
		public Rect stageSize;
		public List<Vector2Int> keyLocations;
		
		public List<Vector2Int> jewelLocations;
    }
}