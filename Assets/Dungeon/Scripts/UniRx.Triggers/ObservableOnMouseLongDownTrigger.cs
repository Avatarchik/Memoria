﻿/*
 * オブジェクト上で、指定した時間以上タップしていると呼び出されるトリガー
 * */

using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	public class ObservableOnMouseLongDownTrigger : ObservableTriggerBase
	{
		public float intervalSecond = 1f;

		private Subject<Unit> onMouseLongDown;

		private float? raiseTime;

		// Update is called once per frame
		void Update()
		{
			if (raiseTime != null && Time.realtimeSinceStartup >= raiseTime)
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
			raiseTime = Time.realtimeSinceStartup + intervalSecond;
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