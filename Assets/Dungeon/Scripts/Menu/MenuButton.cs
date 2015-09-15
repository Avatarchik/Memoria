using UnityEngine;
using UnityEngine.UI;
using Memoria.Dungeon.Managers;
using UniRx;

namespace Memoria.Dungeon.Menu
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField]
        private Button mapButton;

        [SerializeField]
        private Button leaveButton;

        [SerializeField]
        private Button returnButton;

        void Start()
        {
            var dungeonManager = DungeonManager.instance;

            // メニューを開くイベントの登録
            GetComponent<Button>().OnClickAsObservable()
                .Where(_ => dungeonManager.activeState == DungeonState.None)
                .Subscribe(_ =>
                {
                    dungeonManager.EnterState(DungeonState.OpenMenu);
                    SetUIActive(true);
                });

            // メニューを閉じるイベントの登録
            returnButton.OnClickAsObservable()
                .Where(_ => dungeonManager.activeState == DungeonState.OpenMenu)
                .Subscribe(_ =>
                {
                    SetUIActive(false);
                    dungeonManager.ExitState();
                });

            SetUIActive(false);
        }

        public void SetUIActive(bool value)
        {
            mapButton.gameObject.SetActive(value);
            leaveButton.gameObject.SetActive(value);
            returnButton.gameObject.SetActive(value);
        }
    }
}