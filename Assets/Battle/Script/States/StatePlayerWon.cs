﻿using UnityEngine;

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
                result = Resources.Load<Sprite>("UI/clear");
                uiMgr.SpawnResult(result, true);
                loadScene = "menu";
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