using UnityEngine;
using System.Collections.Generic;

namespace Memoria.Dungeon
{
	[System.Serializable]
    public struct StageData// : MonoBehaviour
    {
		public string areaSpritePath;
		public Rect stageSize;
		public List<Vector2Int> keyLocations;
    }
}