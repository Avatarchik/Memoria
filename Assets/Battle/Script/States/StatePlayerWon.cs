using UnityEngine;
using Memoria.Managers;

namespace Memoria.Battle.States
{
    public class StatePlayerWon : BattleState
    {
        string loadScene = "dungeon";
        Sprite result;
        override public void Initialize()
        {
            if(battleMgr.IsBoss)
            {
                FadeOut.Instance.useFading = true;
                result = Resources.Load<Sprite>("UI/clear");
                uiMgr.SpawnResult(result, true);
                loadScene = "menu";
                SoundManager.instance.PlaySound(28);
            } else {
                result = Resources.Load<Sprite>("UI/win");
                uiMgr.SpawnResult(result, false);
              }
 
        }
        override public void Update()
        {
            if (Input.GetMouseButtonDown (0))
            {
                battleMgr.LoadLevel(loadScene);
            }
        }
    }
}