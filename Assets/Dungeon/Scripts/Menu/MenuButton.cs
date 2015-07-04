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
			GetComponent<Button>().OnClickAsObservable()
			.Subscribe(EnterMenu);

			returnButton.GetComponent<Button>().OnClickAsObservable()
			.Subscribe(ExitMenu);
			
			SetUIActive(false);
		}

		public void EnterMenu(Unit _ = null)
		{
			var dungeonManager = DungeonManager.instance;
						
			if (dungeonManager.activeState != DungeonState.None)
			{
				return;
			}
			
			dungeonManager.EnterState(DungeonState.OpenMenu);
			SetUIActive(true);
		}

		public void ExitMenu(Unit _ = null)
		{
			var dungeonManager = DungeonManager.instance;
			
			if (dungeonManager.activeState != DungeonState.OpenMenu)
			{
				return;
			}

			SetUIActive(false);
			dungeonManager.ExitState();
		}

		private void SetUIActive(bool value)
		{
			mapButton.SetActive(value);
			leaveButton.SetActive(value);
			returnButton.SetActive(value);
		}
	}
}