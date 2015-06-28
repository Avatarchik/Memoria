using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Memoria.Dungeon.Managers;
using UniRx;

namespace Memoria.Dungeon.Menu
{
	public class LeaveButton : MonoBehaviour
	{
		//		private DungeonManager dungeonManager;

		[SerializeField]
		private GameObject message;

		[SerializeField]
		private GameObject yesButton;

		[SerializeField]
		private GameObject noButton;

//		void Awake()
//		{
//			var dungeonManager = DungeonManager.instance;
//
//			GetComponent<Button>().OnClickAsObservable()
//			.Where(_ => dungeonManager.activeState == DungeonState.OpenMenu)
//			.Subscribe(_ =>
//			{
//				dungeonManager.EnterState(DungeonState.LeaveSelect);
//				this.SetUIActive(true);
//			});
//				
//			yesButton.GetComponent<Button>().OnClickAsObservable()
//			.Subscribe(_ => dungeonManager.Leave());
//
//			noButton.GetComponent<Button>().OnClickAsObservable()
//			.Subscribe(_ =>
//			{
//				this.SetUIActive(false);
//				dungeonManager.ExitState();
//			});
//		}

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
//			message.SetActive(false);
//			yesButton.SetActive(false);
//			noButton.SetActive(false);
		}
    
		// Update is called once per frame
		//    void Update()
		//    {
		//    }

		//		public void OnClick()
		//		{
		//			if (dungeonManager.activeState == DungeonState.OpenMenu)
		//			{
		//				Enter();
		//			}
		//		}

		//		public void Enter(Unit _ = null)
		//		{
		//			dungeonManager.EnterState(DungeonState.LeaveSelect);
		//			message.SetActive(true);
		//			yesButton.SetActive(true);
		//			noButton.SetActive(true);
		//		}

		//		public void Exit(Unit _ = null)
		//		{
		//			message.SetActive(false);
		//			yesButton.SetActive(false);
		//			noButton.SetActive(false);
		//			dungeonManager.ExitState();
		//		}

		//		public void LeaveDungeon(Unit _ = null)
		//		{
		//			dungeonManager.Leave();
		//		}

		private void SetUIActive(bool value)
		{
			message.SetActive(value);
			yesButton.SetActive(value);
			noButton.SetActive(value);
		}
	}
}