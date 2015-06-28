using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.Managers;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon.Menu
{
	public class MapButton : MonoBehaviour
	{
//		private DungeonManager dungeonManager;

		[SerializeField]
		private GameObject returnButton;

		[SerializeField]
		private MenuButton menuButton;

		[SerializeField]
		private List<GameObject> setActiveObjects;

//		private List<GameObject> activeObjects;

//		void Awake()
//		{
//			returnButton.SetActive(false);
//		}

		// Use this for initialization
		void Start()
		{
			
			var dungeonManager = DungeonManager.instance;
			var activeObjects = new List<GameObject>();

			// MapViewモードに入る
			GetComponent<Button>().onClick.AsObservable()
			.Where(_ => dungeonManager.activeState == DungeonState.OpenMenu)
			.Subscribe(_ => 
			{
				dungeonManager.EnterState(DungeonState.MapViewer);
				activeObjects = setActiveObjects.Where(g => g.activeSelf).ToList();
				activeObjects.ForEach(g => g.SetActive(false));
				returnButton.SetActive(true);
			});
				
			// MapViewモードから出る
			dungeonManager.ActiveStateAsObservable()
			.Buffer(2, 1)
			.Select(states => new
			{
				current = states.ElementAt(0),
				next = states.ElementAt(1)
			})
			.Where(states => states.current == DungeonState.MapViewer && states.next == DungeonState.OpenMenu)
			.Subscribe(_ =>
			{
				returnButton.SetActive(false);
				activeObjects.ForEach(g => g.SetActive(true));
				menuButton.ExitMenu();
			});

//			returnButton.SetActive(false);

//			dungeonManager.changedDungeonState += (sender, e) =>
//			{
//				if (e.nowState == DungeonState.MapViewer && e.nextState == DungeonState.OpenMenu)
//				{
//					Exit();
//				}
//			};
		}
    
		// Update is called once per frame
		//    void Update()
		//    {
		//    }

//		public void Enter(Unit _ = null)
//		{
//			if (dungeonManager.activeState == DungeonState.OpenMenu)
//			{
//				dungeonManager.EnterState(DungeonState.MapViewer);
//				activeObjects = setActiveObjects.Where(g => g.activeSelf).ToList();
//				activeObjects.ForEach(g => g.SetActive(false));
//				returnButton.SetActive(true);
//			}
//		}

//		public void Exit(Unit _ = null)
//		{
//			returnButton.SetActive(false);
//			activeObjects.ForEach(g => g.SetActive(true));
//			menuButton.OnMenuExit();
//		}
	}
}