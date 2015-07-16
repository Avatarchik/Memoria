using UnityEngine;
using UnityEngine.UI;
//  using System.Collections;
using Memoria.Dungeon.Managers;
using UniRx;

namespace Memoria.Dungeon.Menu
{
    public class LeaveButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject message;

        [SerializeField]
        private Button yesButton;

        [SerializeField]
        private Button noButton;

        // Use this for initialization
        void Start()
        {
            var dungeonManager = DungeonManager.instance;

            // LeaveButton イベントの登録
            GetComponent<Button>().OnClickAsObservable()
                .Where(_ => dungeonManager.activeState == DungeonState.OpenMenu)
                .Subscribe(_ =>
                {
                    dungeonManager.EnterState(DungeonState.LeaveSelect);
                    this.SetUIActive(true);
                });

            // yesButton イベントの登録
            yesButton.OnClickAsObservable()
                .Subscribe(_ => dungeonManager.Leave());

            // noButton イベントの登録
            noButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    this.SetUIActive(false);
                    dungeonManager.ExitState();
                });

            SetUIActive(false);
        }

        private void SetUIActive(bool value)
        {
            message.SetActive(value);
            yesButton.gameObject.SetActive(value);
            noButton.gameObject.SetActive(value);
        }
    }
}