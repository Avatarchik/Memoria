using UnityEngine;
using System.Collections;
using Memoria.Dungeon.Managers;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon
{
	public class MapViewer : MonoBehaviour
	{
		public float speed = 1;

		// Use this for initialization
		void Start()
		{
			var dungeonManager = DungeonManager.instance;

			// スクロール
			this.UpdateAsObservable()
			.Where(_ => dungeonManager.activeState == DungeonState.MapViewer)
			.Where(_ => Input.GetMouseButton(0))
			.Select(_ => new Vector2(
				-Input.GetAxis("Mouse X"),
				-Input.GetAxis("Mouse Y")))
			.Subscribe(input => transform.Translate(speed * input));

			// 元の位置に戻す
			var basePosition = transform.localPosition;
			dungeonManager.ActiveStateAsObservable()
			.DistinctUntilChanged()
			.Where(state => state != DungeonState.MapViewer)
			.Subscribe(_ => transform.localPosition = basePosition);
		}
	}
}