﻿using UnityEngine;

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
            Debug.Log("Lost");
        }

    }
}