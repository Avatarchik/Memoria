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

			// HPの変化イベントの追加
			parameterChanged
			.DistinctUntilChanged(param => param.hp)
			.Subscribe(UpdateHpText);
			
			parameterChanged
			.DistinctUntilChanged(param => param.maxHp)
			.Subscribe(UpdateHpText);

			// SPの変化イベントの追加
			parameterChanged
			.DistinctUntilChanged(param => param.sp)
			.Subscribe(UpdateSpText);

			parameterChanged
			.DistinctUntilChanged(param => param.maxSp)
			.Subscribe(UpdateSpText);
		}

		private void UpdateHpText(DungeonParameter parameter)
		{
			hpText.text = string.Format("{0:000}/{1:000}", parameter.hp, parameter.maxHp);
		}

		private void UpdateSpText(DungeonParameter parameter)
		{
			spText.text = string.Format("{0:000}/{1:000}", parameter.sp, parameter.maxSp);
		}
	}
}