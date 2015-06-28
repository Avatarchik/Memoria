using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	public class ObservableOnMouseLongDownTrigger : ObservableTriggerBase
	{
		public float IntervalSecond = 1f;

		private Subject<Unit> onMouseLongDown;

		private float? raiseTime;

		// Update is called once per frame
		void Update()
		{
			if (raiseTime != null && raiseTime <= Time.realtimeSinceStartup)
			{
				if (onMouseLongDown != null)
				{
					onMouseLongDown.OnNext(Unit.Default);
				}

				raiseTime = null;
			}
		}

		void OnMouseDown()
		{
			raiseTime = Time.realtimeSinceStartup + IntervalSecond;
		}

		void OnMouseExit()
		{
			raiseTime = null;
		}

		void OnMouseUp()
		{
			raiseTime = null;
		}

		public IObservable<Unit> OnMouseLongDownAsObservable()
		{
			return onMouseLongDown ?? (onMouseLongDown = new Subject<Unit>());
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (onMouseLongDown != null)
			{
				onMouseLongDown.OnCompleted();
			}
		}
	}
}