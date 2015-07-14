using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Memoria.Dungeon.Managers;

namespace Memoria.Dungeon.BlockUtility
{
	public class BlockBreaker : MonoBehaviour
	{
		[SerializeField]
		private float breakTapSecond = 1f;

		private DungeonManager dungeonManager;
		private MapManager mapManager;

		private Subject<Unit> onBreakBlock;

		public Subject<Unit> OnBreakBlockAsObservable()
		{
			return onBreakBlock ?? (onBreakBlock = new Subject<Unit>());
		}

		// Use this for initialization
		void Start()
		{
			dungeonManager = DungeonManager.instance;
			
			var block = GetComponent<Block>();
			var setter = GetComponent<BlockSetter>();

			float raiseTime = 0;
			this.UpdateAsObservable()
			.Where(_ => setter.putted)
			.Where(_ => dungeonManager.activeState == DungeonState.None)
			.Where(_ => block.location != dungeonManager.player.location)
			.SkipUntil(this.OnMouseDownAsObservable()
				       .Do(_ => raiseTime = Time.realtimeSinceStartup + breakTapSecond))
			.TakeUntil(this.OnMouseUpAsObservable()
			           .Merge(this.OnMouseExitAsObservable()))
			.Repeat()
			.Where(_ => Time.realtimeSinceStartup >= raiseTime)
			.Subscribe(_ =>
			{
				if (onBreakBlock != null)
				{
					onBreakBlock.OnNext(Unit.Default);
				}

				Destroy(gameObject);
			});
		}
	}
}