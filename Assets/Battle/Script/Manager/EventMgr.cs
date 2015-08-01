using System;
using System.Collections.Generic;
using Memoria.Battle.GameActors;
using UnityEngine;


namespace Memoria.Battle.Managers
{
    public class GameEvent
    {
        /*
          Useage:

          AddListener<GameEvent>(Method(GameEvent e));
          RemoveListener<GameEvent>(Method(GameEvent e));
          Raise(new GameEvent());
        */

    }

    public class EventMgr : Singleton<EventMgr>
    {
        public delegate void EventDel<T> (T e) where T : GameEvent;

        private delegate void EventDel (GameEvent e);

        private Dictionary<Type, EventDel> _events = new Dictionary<Type, EventDel>();

        private Dictionary<Delegate, EventDel> _eventHash = new Dictionary<Delegate, EventDel>();

        public void AddListener<T>(EventDel<T> e) where T : GameEvent
        {
            if(_eventHash.ContainsKey(e))
            {
                return;
            }
            EventDel newEvent = (x) => e((T)x);
            _eventHash[e] = newEvent;

            EventDel tmpVar;
            _events[typeof(T)] = (_events.TryGetValue(typeof(T), out tmpVar)) ?
                (tmpVar += newEvent) :
                newEvent;
        }

        public void RemoveListener<T> (EventDel<T> e) where T : GameEvent
        {
            EventDel lookupEvent;
            if(_eventHash.TryGetValue(e, out lookupEvent))
            {
                EventDel tempDel;
                if(_events.TryGetValue(typeof(T), out tempDel))
                {
                    tempDel -= lookupEvent;
                    if(tempDel == null)
                    {
                        _events.Remove(typeof(T));
                    }
                    else
                    {
                        _events[typeof(T)] = tempDel;
                    }
                }
                _eventHash.Remove(e);
            }
        }

        public void Raise (GameEvent e)
        {
            EventDel eDel;
            if(_events.TryGetValue(e.GetType(), out eDel))
            {
                eDel.Invoke(e);
            }
            else
            {
                Debug.LogWarning("[E] Missing listener for event: "+ e.GetType());
            }
        }

        public void Clear()
        {
            _eventHash.Clear();
            _events.Clear();
        }
        // ************************ Deprecated functionality

        public delegate void TurnEnds();
        public delegate void EnemyDies(Enemy e);
        public delegate void BattleEnds();

        public event TurnEnds TurnEnd;
        public event BattleEnds BattleEnd;
        public event EnemyDies EnemyDied;


        public void OnTurnEnd()
        {
            if(TurnEnd != null)
            {
                TurnEnd();
            }
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }

        public void OnBattleEnd()
        {
            if(BattleEnd != null)
            {
                BattleEnd();
            }
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
        }

        public void OnEnemyDied(Enemy e)
        {
            if(EnemyDied != null)
            {
                EnemyDied(e);
            Debug.LogWarning("[I] This method is deprecated, use AddListener function instead");
            }
        }
    }
}