using UnityEngine;
using Memoria.Dungeon;
using Memoria.Managers;

namespace Memoria.Battle.States
{
    public class StatePlayerLost : BattleState
    {
        override public void Initialize()
        {
            Sprite result = Resources.Load<Sprite>("UI/lose");
            uiMgr.SpawnResult(result, false);
            SoundManager.instance.PlaySound(5);
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
