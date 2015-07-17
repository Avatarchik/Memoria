﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Managers
{
    public class AttackTracker : MonoBehaviour {

        public Dictionary<Entity, float> attackOrder;

        public Entity nowActor;

        // Use this for initialization
        void Start ()
        {
            attackOrder = new Dictionary<Entity, float> ();
        }

        // Update is called once per frame
        void Update ()
        {
            nowActor = currentActor;
        }

        public void QueueAction(Entity e, float pos)
        {
            if(attackOrder.ContainsKey(e))
            {
                attackOrder[e] = pos;
                foreach(var orderId in attackOrder)
                {
                    if(orderId.Value > pos) {
                        orderId.Key.charge = true;
                    }
                }
            }
        }

        public void MoveTo(Entity e, float pos)
        {
            if(attackOrder.ContainsKey(e))
            {
                attackOrder[e] = pos;
            }
            else
            {
                Debug.LogWarning("[E] Actor missing in attack order "+ e);
            }
        }

        public Entity currentActor
        {
            get
            {
                return attackOrder.OrderBy(x => x.Value).FirstOrDefault().Key;
            }
        }

        public void GenerateQueue<T>(IList<GameObject> objectList) where T : Entity
        {
            float index = 0;
            foreach (var go in BattleMgr.actorList) {
                T actor = go.GetComponent<T>();
                actor.orderIndex = index;
                if(!attackOrder.ContainsKey(actor)) {
                    AddToQueue(actor, index);
                }
                index++;
            }
        }

        public void AddToQueue(Entity e, float pos)
        {
            attackOrder.Add(e, pos);
        }

        public void RemoveFromQueue(Entity e)
        {
            attackOrder.Remove(e);
        }

        public Vector3[] GetSlots()
        {
            Namebar n = FindObjectOfType<Namebar>();
            Vector3[] result = new Vector3[attackOrder.Count];
            for(int i = 0; i < attackOrder.Count; i++)
            {
                result[i] = new Vector3(n.X, n.Y - ((i - 4)), 1);
            }
            System.Array.Reverse(result);
            return result;
        }
    }
}