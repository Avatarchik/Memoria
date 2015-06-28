using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Memoria.Dungeon.Managers;
using UniRx;

namespace Memoria.Dungeon.Menu
{
	public class MenuButton : MonoBehaviour
	{
		//		private DungeonManager dungeonManager;

		[SerializeField]
		private GameObject mapButton;

		[SerializeField]
		private GameObject leaveButton;

		[SerializeField]
		private GameObject returnButton;

		//		void Awake()
		//		{
		//			dungeonManager = DungeonManager.instance;
		//		}

		// Use this for initialization
		void Start()
		{
//			var dungeonManager = DungeonManager.instance;

			GetComponent<Button>().OnClickAsObservable()
			.Subscribe(EnterMenu);

			returnButton.GetComponent<Button>().OnClickAsObservable()
			.Subscribe(ExitMenu);
			
			SetUIActive(false);
//			mapButton.SetActive(false);
//			leaveButton.SetActive(false);
//			returnButton.SetActive(false);
		}
    
		// Update is called once per frame
		//    void Update()
		//    {
		//    }

		//		public void OnMenuEnter()
		//		{
		//			var dungeonManager = DungeonManager.instance;
		//
		//			if (dungeonManager.activeState != DungeonState.None)
		//			{
		//				return;
		//			}
		//
		//			dungeonManager.EnterState(DungeonState.OpenMenu);
		//			SetUIActive(true);
		//			mapButton.SetActive(true);
		//			leaveButton.SetActive(true);
		//			returnButton.SetActive(true);
		//		}

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

//		public void OnExitMenu()
		public void ExitMenu(Unit _ = null)
		{
			var dungeonManager = DungeonManager.instance;
			
			if (dungeonManager.activeState != DungeonState.OpenMenu)
			{
				return;
			}

			SetUIActive(false);
//			mapButton.SetActive(false);
//			leaveButton.SetActive(false);
//			returnButton.SetActive(false);
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