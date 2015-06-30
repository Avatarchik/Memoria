using UnityEngine;
using System.Collections;
using UniRx;

namespace UniRx.Triggers
{
	public class ObservableOnMouseUpOnScreenTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onMouseUpOnScreen;

		// Update is called once per frame
		void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				onMouseUpOnScreen.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDownOnScreenAsObservable()
		{
			return onMouseUpOnScreen ?? (onMouseUpOnScreen = new Subject<Unit>());
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (onMouseUpOnScreen != null)
			{
				onMouseUpOnScreen.OnCompleted();
			}
		}
	}
}