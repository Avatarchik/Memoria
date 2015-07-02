using UnityEngine;
using System.Collections;
using UniRx;

namespace UniRx.Triggers
{
	public class ObservableOnMouseShotUpAsButtonInColliderTrigger : ObservableTriggerBase
	{
		public float limitSecond = 1;

		private Subject<Unit> onMouseUpAsButtonInCollider;

		private float? raiseTime;

		void Update()
		{
			if (raiseTime != null && Time.realtimeSinceStartup > raiseTime)
			{
				raiseTime = null;
			}
		}

		void OnMouseDown()
		{
			raiseTime = Time.realtimeSinceStartup + limitSecond;
		}

		void OnMouseExit()
		{
			raiseTime = null;
		}

		void OnMouseUp()
		{
			if (Time.realtimeSinceStartup <= raiseTime)
			{
				onMouseUpAsButtonInCollider.OnNext(Unit.Default);
			}

			raiseTime = null;
		}

		public IObservable<Unit> OnMouseDownOnScreenAsObservable()
		{
			return onMouseUpAsButtonInCollider ?? (onMouseUpAsButtonInCollider = new Subject<Unit>());
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (onMouseUpAsButtonInCollider != null)
			{
				onMouseUpAsButtonInCollider.OnCompleted();
			}
		}
	}
}