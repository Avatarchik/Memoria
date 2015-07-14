using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Memoria.Battle.GameActors;

namespace Memoria.Battle.Managers
{
    public class AttackTracker : MonoBehaviour {

        public Dictionary<Entity, float> attackOrder;
        private int _orderIndex;
        private int _actors;    public Entity nowActor;
        public float test;

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
                Debug.LogWarning("[E] Actor missing in attack order "+ e.ToString());
            }
        }

        public Entity currentActor
        {
            get
            {
                return attackOrder.OrderBy(x => x.Value).FirstOrDefault().Key;
            }
        }

        public void DestroyActor(Entity e)
        {
            attackOrder.Remove(e);
            //TODO: Implement method which moves down everyv actors after.
        }

        private void RefreshQueue(int i)
        {
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