using UnityEngine;

namespace Memoria.Battle.States
{
    public class StatePlayerWon : BattleState
    {
        override public void Initialize()
        {
            Sprite result = Resources.Load<Sprite>("UI/win");
            FadeAttackScreen.DeFlash();
            uiMgr.SpawnResult(result);
        }
        override public void Update()
        {
            Debug.Log("Won");
        }
    }
}