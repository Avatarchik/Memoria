namespace Memoria.Battle.Managers
{
    public class EventListner : EventMgr {

        public static new EventListner Instance = new EventListner();

        public void SubscribeTurnEnd(TurnEnds turnEnds)
        {
            EventMgr.Instance.TurnEnd += turnEnds;
        }

        public void SubscribeBattleEnd(BattleEnds battleEnds)
        {
            EventMgr.Instance.BattleEnd += battleEnds;
        }

        public void SubscribeEnemyDies(EnemyDies enemyDies)
        {
            EventMgr.Instance.EnemyDied += enemyDies;
        }

        public void SubscribeRecievedStatus(RecievedStatus recivedStatus)
        {
            EventMgr.Instance.GotStatus += recivedStatus;
        }
    }
}