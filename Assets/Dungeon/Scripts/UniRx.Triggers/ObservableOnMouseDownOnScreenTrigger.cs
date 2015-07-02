/* 
 * 画面内のどこかで、マウスボタンが押された瞬間に呼び出されるトリガー
 * */

using UnityEngine;
using System.Collections;
using UniRx;

namespace UniRx.Triggers
{
	public class ObservableOnMouseDownOnScreenTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onMouseDownOnScreen;
	
		// Update is called once per frame
		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				onMouseDownOnScreen.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDownOnScreenAsObservable()
		{
			return onMouseDownOnScreen ?? (onMouseDownOnScreen = new Subject<Unit>());
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (onMouseDownOnScreen != null)
			{
				onMouseDownOnScreen.OnCompleted();
			}
		}
	}
}