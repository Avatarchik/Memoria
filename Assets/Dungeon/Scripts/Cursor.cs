using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon
{
	public class Cursor : MonoBehaviour
	{
		private DungeonManager dungeonManager;

		public Animator animator { get; set; }

		// Use this for initialization
		void Start()
		{
			animator = GetComponent<Animator>();
			animator.SetBool("isVisible", false);

			dungeonManager = DungeonManager.instance;        
			dungeonManager.changingDungeonState += HandleChangingDungeonState;
		}

		void HandleChangingDungeonState(object sender, ChangeDungeonStateEventArgs e)
		{
			if (e.nextState == DungeonState.BlockOperating)
			{
				animator.SetBool("isVisible", true);
			}
			else
			{
				animator.SetBool("isVisible", false);
			}
		}
	
		// Update is called once per frame
		void Update()
		{
			bool canPut = dungeonManager.operatingBlock ? dungeonManager.operatingBlock.CanPut() : false;
			animator.SetBool("canPut", canPut);

			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 blockSize = dungeonManager.blockSize;
			position.x = Mathf.Round(position.x * 100 / blockSize.x) * (blockSize.x / 100);
			position.y = Mathf.Round(position.y * 100 / blockSize.y) * (blockSize.y / 100);
			transform.position = position;
		}
	}
}