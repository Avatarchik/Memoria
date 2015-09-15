using UnityEngine;
using System.Collections;
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
        
        public Coroutine StartCheckSpRemainCoroutine()
        {
            return coroutineAppended.StartCoroutine(CoroutineCheckSpRemain());
        }
        
        private IEnumerator CoroutineCheckSpRemain()
        {
            if (!RemainSp())
            {
                eventAnimator.SetTrigger("onGameOver");
                yield return new WaitForSeconds(3f);
                Application.LoadLevel("title");
            }
            
            yield return null;
        }
        
        private bool RemainSp()
        {
            return ParameterManager.instance.parameter.sp > 0;
        }
    }
}