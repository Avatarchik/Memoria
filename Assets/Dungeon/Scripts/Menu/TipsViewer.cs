using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.Menu
{
    public class TipsViewer : MonoBehaviour
    {
        [SerializeField]
        private Button viewTipButton;
        
        [SerializeField]
        private Animator tipsViewerAnimator;

        // Use this for initialization
        void Start()
        {
            viewTipButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    DungeonManager.instance.EnterState(DungeonState.OpenMenu/*TipsViewer*/);
                    tipsViewerAnimator.SetBool("show", true);
                    DungeonManager.instance.ExitState();
                });
        }

        // Update is called once per frame
        //  void Update()
        //  {

        //  }


    }
}