using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;

namespace Memoria.Dungeon.Managers
{
	public class ParameterManager : MonoBehaviour
	{
		private ReactiveProperty<DungeonParameter> _parameter = new ReactiveProperty<DungeonParameter>();

		public DungeonParameter parameter
		{ 
			get { return _parameter.Value; } 
			set { _parameter.Value = value; }
		}

		[SerializeField]
		private Text
			hpText;

		[SerializeField]
		private Text
			spText;

		public void Start()
		{
			var parameterChanged = _parameter.AsObservable();
			string format = "{0:000}/{1:000}";

			// HPの変化イベントの追加
			parameterChanged
			.DistinctUntilChanged(param => param.hp)
			.Subscribe(param => hpText.text = string.Format(format, parameter.hp, parameter.maxHp));

			// SPの変化イベントの追加
			parameterChanged
			.DistinctUntilChanged(param => param.sp)
			.Subscribe(param => spText.text = string.Format(format, parameter.sp, parameter.maxSp));
		}
	}
}