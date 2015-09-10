using UnityEngine;
using System.Collections;
using Memoria.Battle.Events;
using Memoria.Battle.Managers;

namespace Memoria.Battle.GameActors
{
    public class HasteSkill : MonoBehaviour
    {
        [SerializeField]
        private int _rounds = 3;

        [SerializeField]
        private int _hasteValue = 1;

        public Entity user;

        void Start()
        {
            EventMgr.Instance.AddListener<BeforeTurnEnds>(Tick);
        }

        void Update()
        {
            if(_rounds == 0)
            {
                EventMgr.Instance.RemoveListener<BeforeTurnEnds>(Tick);
                Destroy(this.transform.gameObject);
            }          
        }

        private void Tick(BeforeTurnEnds gameEvent)
        {
            if(gameEvent.actor == user)
            {
                _rounds--;
                _hasteValue++;
                foreach(var ga in BattleMgr.Instance.actorList)
                {
                    var entity = ga.GetComponent<Entity>();
                    if(entity.orderIndex >= _hasteValue && entity != user)
                    {
                        entity.orderIndex++;
                    }
                }
                user.orderIndex = _hasteValue;
            }
        }
    }
}