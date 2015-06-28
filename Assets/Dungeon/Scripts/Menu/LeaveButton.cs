using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Memoria.Dungeon.Managers;
using UniRx;

namespace Memoria.Dungeon.Menu
{
	public class LeaveButton : MonoBehaviour
	{
		[SerializeField]
		private GameObject message;

		[SerializeField]
		private GameObject yesButton;

		[SerializeField]
		private GameObject noButton;

		// Use this for initialization
		void Start()
		{   
			var dungeonManager = DungeonManager.instance;

			GetComponent<Button>().OnClickAsObservable()
			.Where(_ => dungeonManager.activeState == DungeonState.OpenMenu)
			.Subscribe(_ =>
			{
				dungeonManager.EnterState(DungeonState.LeaveSelect);
				this.SetUIActive(true);
			});

			yesButton.GetComponent<Button>().OnClickAsObservable()
			.Subscribe(_ => dungeonManager.Leave());

			noButton.GetComponent<Button>().OnClickAsObservable()
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
			yesButton.SetActive(value);
			noButton.SetActive(value);
		}
	}
}