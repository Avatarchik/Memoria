using UnityEngine;
using System.Collections.Generic;

namespace Memoria.Dungeon.Managers
{
    public class StageDataManager : MonoBehaviour
    {
        public static StageDataManager instance { get { return DungeonManager.instance.stageDataManager; } }
        public List<StageData> stageDatas = new List<StageData>();

        public StageData Prepare(int floor)
        {
            var stageData = stageDatas[floor];
            (new GameObject()).AddComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>(stageData.areaSpritePath);

            stageData.floor = floor;

            return stageData;
        }
    }
}