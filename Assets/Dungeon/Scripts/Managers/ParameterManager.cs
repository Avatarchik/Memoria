using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;

namespace Memoria.Dungeon.Managers
{
	public class ParameterManager : MonoBehaviour
	{
		private DungeonParameter _parameter;

		public DungeonParameter parameter
		{
			get
			{
				if (_parameter == null)
				{
					_parameter = new DungeonParameter();

					_parameter.HpAsObservable()
					.Subscribe(UpdateHpText);

					_parameter.SpAsObservable()
					.Subscribe(UpdateSpText);
				}

				return _parameter;
			}
		}

		[SerializeField]
		private Text
			hpText;

		[SerializeField]
		private Text
			spText;

		public void SetParamater(DungeonParameter parameter)
		{
			this.parameter.Set(parameter);
		}

		private void UpdateHpText(int _ = default(int))
		{
			int hp = parameter.hp;
			int maxHp = parameter.maxHp;
			hpText.text = string.Format("{0:000}/{1:000}", hp, maxHp);
		}

		private void UpdateSpText(int _ = default(int))
		{
			int sp = parameter.sp;
			int maxSp = parameter.maxSp;
			spText.text = string.Format("{0:000}/{1:000}", sp, maxSp);
		}
	}
}