using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Memoria.Dungeon.Managers
{
    public class StageDataManager : MonoBehaviour
    {
        public static StageDataManager instance { get { return DungeonManager.instance.stageDataManager; } }
        public List<StageData> stageDatas = new List<StageData>();

        public StageData Prepare(int floor)
        {
            var equalsFloorStageDatas = stageDatas.Where(s => s.floor == floor);
            int selectedStageIndex = Random.Range(0, equalsFloorStageDatas.Count());
            
            var stageData = equalsFloorStageDatas.ElementAt(selectedStageIndex);

            return stageData;
        }
    }
}