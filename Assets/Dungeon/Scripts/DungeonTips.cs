using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon
{
    public class DungeonTips : MonoBehaviour
    {
        [SerializeField]
        private Button tipsButton;
        
        // Use this for initialization
        void Start()
        {
            var dungeonManager = DungeonManager.instance;
            var animator = GetComponent<Animator>();

            tipsButton.OnClickAsObservable()
                .Where(_ => dungeonManager.activeState == DungeonState.None)
                .Do(_ => dungeonManager.EnterState(DungeonState.TipsViewer))
                .Subscribe(_ => animator.SetBool("shown", true));

            this.UpdateAsObservable()
                .Where(_ => dungeonManager.activeState == DungeonState.TipsViewer)
                .Where(_ => Input.GetMouseButtonDown(0))
                .Do(_ => Observable.Return(1)
                    .DelayFrame(5)
                    .Subscribe(__ => dungeonManager.ExitState()))
                .Subscribe(_ => animator.SetBool("shown", false));
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}