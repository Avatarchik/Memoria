using UnityEngine;
using Memoria.Dungeon;

namespace Memoria.Battle.States
{
    public class StatePlayerLost : BattleState
    {
        override public void Initialize()
        {
            Sprite result = Resources.Load<Sprite>("UI/lose");
            uiMgr.SpawnResult(result);
        }

        override public void Update()
        {
            if (Input.GetMouseButtonDown (0))
            {
                MonoBehaviour.Destroy(GameObject.FindObjectOfType<DungeonData>().gameObject);
                battleMgr.LoadLevel("title");
            }
        }
    }
}
