using System;
using System.Collections.Generic;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Managers
{
    public interface ITriggerable
    {

    }

    /*
      WORK IN PROGRESS
      Useage:

      AddListener<ITriggerable>(EventDel(ITriggerable e))
      RemoveListener<ITriggerable>(EventDel(ITriggerable e))
      TrigEvent(ITriggerable e)



      public class EventMgr
      {
      private static EventManager _instance;

      public delegate void EventDel<T> (T e) where T : ITriggerable;
      private delegate void EventDel (ITriggerable e);

      private Dictionary<Type, EventDel> _events;
      private Dictionary<Delegate, EventDel> _eventHasg;

      public static EventManager Instance
      {
      if(_instance == null) {
      _instance = new EventManager();
      }
      return _instance
      }

      private EventDel AddEvent<T>(EventDel<T> e) where T : ITriggerable
      {
      if(_eventHash.ContainsKey(e)) {
      return null;
      }
      EventDel internalEvent = (x) => e((T)x);
      _eventHash[e] = newEvent;

      EventDel tmpVar;
      if(events.TryGetValue(typeof(T), out tmpVar)) {
      events[typeof(T)] = tmpVar += internalEvent;
      } else {
      events[typeof(T)] = interalEvent;
      }
      return internalEvent;
      }

      public void AddListener<T> (EventDel<T> e) where T : ITriggerable
      {
      AddEvent<T>(e);
      }

      public void RemoveListener<T> (EventDel<T> e)
      {

      }

      public void Clear()
      {
      _events.Clear();
      _eventHash.Clear();
      }

      public void Raise (ITriggerable e)
      {
      EventDel eDel;
      if(events.TryGetValue(e.GetValue(), out eDel)) {
      eDel.Invoke(e);
      } else {
      Debub.LogWarning("[E] Missing listener for event: "+ e.GetType());
      }
      }
      }

    */

    public class EventMgr {

        public static EventMgr Instance = new EventMgr();

        public delegate void TurnEnds();
        public delegate void EnemyDies(Enemy e);
        public delegate void BattleEnds();
        public delegate void GameEvent(object sender, EventArgs e);

        public event TurnEnds TurnEnd;
        public event BattleEnds BattleEnd;
        public event EnemyDies EnemyDied;

        public Dictionary<string, System.Delegate> delegateList;

        public void OnTurnEnd()
        {
            if(TurnEnd != null)
            {
                TurnEnd();
            }
        }

        public void OnBattleEnd()
        {
            if(BattleEnd != null)
            {
                BattleEnd();
            }
        }

        public void OnEnemyDied(Enemy e)
        {
            if(EnemyDied != null)
            {
                EnemyDied(e);
            }
        }
    }
}