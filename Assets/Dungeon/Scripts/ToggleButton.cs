using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon
{
	public class ToggleButton : MonoBehaviour
	{
		private DungeonManager dungeonManager;

		public GameObject randomBlockList;
		public GameObject colorBlockList;

		// Use this for initialization
		void Start()
		{
			dungeonManager = DungeonManager.instance;
			randomBlockList.SetActive(true);
			colorBlockList.SetActive(false);
		}
	
		// Update is called once per frame
		void Update()
		{	
		}

		public void ToggleBlockList()
		{
			if (dungeonManager.activeState != DungeonState.None)
			{
				return;
			}

			bool active = !randomBlockList.activeSelf;
			randomBlockList.SetActive(active);
			colorBlockList.SetActive(!active);
		}
	}
}