using UnityEngine;

//  Deprecated

namespace Memoria.Battle.Managers
{
    public class EventListner : EventMgr {

//        public static new EventListner Instance = new EventListner();

        public void SubscribeTurnEnd(TurnEnds turnEnds)
        {
            EventMgr.Instance.TurnEnd += turnEnds;
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }

        public void SubscribeBattleEnd(BattleEnds battleEnds)
        {
            EventMgr.Instance.BattleEnd += battleEnds;
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }

        public void SubscribeEnemyDies(EnemyDies enemyDies)
        {
            EventMgr.Instance.EnemyDied += enemyDies;
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }
        public void UnsubscribeTurnEnd(TurnEnds turnEnds)
        {
            EventMgr.Instance.TurnEnd -= turnEnds;
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }
    }
}
