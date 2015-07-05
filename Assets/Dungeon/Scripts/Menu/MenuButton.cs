using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Memoria.Dungeon.Managers;
using UniRx;

namespace Memoria.Dungeon.Menu
{
	public class MenuButton : MonoBehaviour
	{
		[SerializeField]
		private GameObject mapButton;

		[SerializeField]
		private GameObject leaveButton;

		[SerializeField]
		private GameObject returnButton;

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
			returnButton.GetComponent<Button>().OnClickAsObservable()
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
			mapButton.SetActive(value);
			leaveButton.SetActive(value);
			returnButton.SetActive(value);
		}
	}
}