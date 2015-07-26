using UnityEngine;
using System.Collections;
using UniRx;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockEvents
{
    public class SpRemainCheckEvent
    {
        private MonoBehaviour coroutineAppended;
        private Animator eventAnimator;
        
        public SpRemainCheckEvent(MonoBehaviour coroutineAppended, Animator eventAnimator)
        {
            this.coroutineAppended = coroutineAppended;
            this.eventAnimator = eventAnimator;
        }
        
        public IObservable<Unit> CreateCheckSpRemainAsObservable()
        {
            return Observable.FromCoroutine(CoroutineCheckSpRemain);
        }
        
        private IEnumerator CoroutineCheckSpRemain()
        {
            if (!RemainSp())
            {
                Debug.Log("Leave Dungeon!!");
            }
            
            yield return null;
        }
        
        private bool RemainSp()
        {
            return ParameterManager.instance.parameter.sp > 0;
        }
    }
}