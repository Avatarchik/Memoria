using UnityEngine;

namespace Memoria.Battle.States
{
    public class StatePlayerWon : BattleState
    {
        override public void Initialize()
        {
            Sprite result = Resources.Load<Sprite>("UI/win");
            uiMgr.SpawnResult(result);
        }
        override public void Update()
        {
            if (Input.GetMouseButtonDown (0))
            {
                battleMgr.LoadLevel("dungeon");
            }
        }
    }
}