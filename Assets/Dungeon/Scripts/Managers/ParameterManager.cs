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
		//		private DungeonParameter _parameter;
		private ReactiveProperty<DungeonParameter> _parameter = new ReactiveProperty<DungeonParameter>();

		public DungeonParameter parameter
		{ 
			get { return _parameter.Value; } 
			set { _parameter.Value = value; }
		}
		//		{
		//			get
		//			{
		//				if (_parameter == null)
		//				{
		//					_parameter = new DungeonParameter();
		//
		//					_parameter.HpAsObservable()
		//					.Subscribe(UpdateHpText);
		//
		//					_parameter.SpAsObservable()
		//					.Subscribe(UpdateSpText);
		//				}
		//
		//				return _parameter;
		//			}
		//		}

		[SerializeField]
		private Text
			hpText;

		[SerializeField]
		private Text
			spText;

		public void Initialize()
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

		//		public void SetParamater(DungeonParameter parameter)
		//		{
		//			this.parameter.Set(parameter);
		//		}

		//		private void UpdateHpText(int _ = default(int))
		//		{
		//			int hp = parameter.hp;
		//			int maxHp = parameter.maxHp;
		//			hpText.text = string.Format("{0:000}/{1:000}", hp, maxHp);
		//		}

		//		private void UpdateSpText(int _ = default(int))
		//		{
		//			int sp = parameter.sp;
		//			int maxSp = parameter.maxSp;
		//			spText.text = string.Format("{0:000}/{1:000}", sp, maxSp);
		//		}

		//		private void UpdateHpText(DungeonParameter paramater)
		//		{
		//			hpText.text = string.Format("{0:0000}/{1:0000}", paramater.hp, paramater.maxHp);
		//		}

		//		private void UpdateSpText(DungeonParameter parameter)
		//		{
		//			spText.text = string.Format("{0:0000}/{1:0000}", parameter.sp, parameter.maxSp);
		//		}
	}
}