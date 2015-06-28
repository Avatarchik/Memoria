using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon
{
	public class Cursor : MonoBehaviour
	{
//		private DungeonManager dungeonManager;
//
//		public Animator animator { get; set; }

		// Use this for initialization
		void Start()
		{
			Animator animator = GetComponent<Animator>();
			animator.SetBool("isVisible", false);

			DungeonManager dungeonManager = DungeonManager.instance;        
//			dungeonManager.changingDungeonState += HandleChangingDungeonState;

			// 表示切り替えの登録
			dungeonManager.ActiveStateAsObservable()
			.DistinctUntilChanged()
			.Select(nextState => nextState == DungeonState.BlockOperating)
			.Subscribe(visible => animator.SetBool("isVisible", visible));

			// 画像切り替えの登録
			this.UpdateAsObservable()
			.Select(_ => dungeonManager.operatingBlock ? dungeonManager.operatingBlock.CanPut() : false)
			.Do(canPut => animator.SetBool("canPut", canPut))
			.Select(_ =>
			{
				Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2 blockSize = dungeonManager.blockSize;
				return new Vector2(
					Mathf.Round(touchPosition.x * 100 / blockSize.x) * (blockSize.x / 100),
					Mathf.Round(touchPosition.y * 100 / blockSize.y) * (blockSize.y / 100)
				);
			})
			.Subscribe(position => transform.position = position);

		}

		//		void HandleChangingDungeonState(object sender, ChangeDungeonStateEventArgs e)
		//		{
		//			if (e.nextState == DungeonState.BlockOperating)
		//			{
		//				animator.SetBool("isVisible", true);
		//			}
		//			else
		//			{
		//				animator.SetBool("isVisible", false);
		//			}
		//		}
	
		// Update is called once per frame
//		void Update()
//		{
//			bool canPut = dungeonManager.operatingBlock ? dungeonManager.operatingBlock.CanPut() : false;
//			animator.SetBool("canPut", canPut);
//
//			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			Vector2 blockSize = dungeonManager.blockSize;
//			position.x = Mathf.Round(position.x * 100 / blockSize.x) * (blockSize.x / 100);
//			position.y = Mathf.Round(position.y * 100 / blockSize.y) * (blockSize.y / 100);
//			transform.position = position;
//		}
	}
}