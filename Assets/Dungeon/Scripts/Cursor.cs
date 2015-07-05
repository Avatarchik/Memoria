using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon
{
	public class Cursor : MonoBehaviour
	{
		// Use this for initialization
		void Start()
		{
			var animator = GetComponent<Animator>();
			animator.SetBool("visible", false);

			var dungeonManager = DungeonManager.instance;

			// 表示切り替えの登録
			dungeonManager.ActiveStateAsObservable()
			.DistinctUntilChanged()
			.Select(nextState => nextState == DungeonState.BlockOperating)
			.Subscribe(visible => animator.SetBool("visible", visible));

			// 画像切り替えの登録
			this.UpdateAsObservable()
			.Select(_ => dungeonManager.operatingBlock ? dungeonManager.operatingBlock.CanPut() : false)
			.Do(canPut => animator.SetBool("canPut", canPut))
			.Select(_ =>
			{
				Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				return dungeonManager.mapManager.ConvertPosition(touchPosition);
			})
			.Subscribe(position => transform.position = position);
		}
	}
}