using UnityEngine;

//  Deprecated

namespace Memoria.Battle.Managers
{
    public class EventListner : EventManager {

        public static new EventListner Instance = new EventListner();

        public void SubscribeTurnEnd(TurnEnds turnEnds)
        {
            EventManager.Instance.TurnEnd += turnEnds;
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }

        public void SubscribeBattleEnd(BattleEnds battleEnds)
        {
            EventManager.Instance.BattleEnd += battleEnds;
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }

        public void SubscribeEnemyDies(EnemyDies enemyDies)
        {
            EventManager.Instance.EnemyDied += enemyDies;
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }
        public void UnsubscribeTurnEnd(TurnEnds turnEnds)
        {
            EventManager.Instance.TurnEnd -= turnEnds;
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }
    }
}
