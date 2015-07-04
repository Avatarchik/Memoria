using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UniRx;

namespace Memoria.Dungeon
{
	//public class DungeonParameter //: ICloneable
	public struct DungeonParameter
	{
#region paramater

//		private ReactiveProperty<int> _hp = new ReactiveProperty<int>();
//
//		public int hp
//		{
//			get { return _hp.Value; }
//			set { _hp.Value = Mathf.Clamp(value, 0, maxHp); }
//		}

		private int _hp;

		public int hp
		{
			get { return _hp; }
			set { _hp = Mathf.Clamp(value, 0, maxHp); }
		}

		public int maxHp { get; set; }

//		private ReactiveProperty<int> _sp = new ReactiveProperty<int>();
//
//		public int sp
//		{
//			get { return _sp.Value; }
//			set { _sp.Value = Mathf.Clamp(value, 0, maxSp); }
//		}

		private int _sp;

		public int sp 
		{
			get { return _sp; }
			set { _sp = Mathf.Clamp(value, 0, maxSp); }
		}

		public int maxSp { get; set; }

//		private ReactiveProperty<int> _floor = new ReactiveProperty<int>();
//
//		public int floor
//		{
//			get { return _floor.Value; }
//			set { _floor.Value = value; }
//		}

		private int floor { get; set; }

//		private ReactiveProperty<string> _skill = new ReactiveProperty<string>();
//
//		public string skill
//		{
//			get { return _skill.Value; }
//			set { _skill.Value = value; }
//		}

		private string skill { get; set; }

#endregion

#region ParamaterAsObservable

//		public IObservable<int> HpAsObservable()
//		{
//			return _hp.AsObservable();
//		}
//
//		public IObservable<int> SpAsObservable()
//		{
//			return _sp.AsObservable();
//		}
//
//		public IObservable<int> FloorAsObservable()
//		{
//			return _floor.AsObservable();
//		}
//
//		public IObservable<string> SkillAsObservable()
//		{
//			return _skill.AsObservable();
//		}

#endregion

		public DungeonParameter(int maxHp, int hp, int maxSp, int sp, int floor, string skill)
		{
			this.maxHp = maxHp;
			this.hp = hp;
			this.maxSp = maxSp;
			this.sp = sp;
			this.floor = floor;
			this.skill = skill;
		}

//		public void Set(DungeonParameter parameter)
//		{
//			Set(parameter.maxHp,
//			    parameter.hp,
//			    parameter.maxSp,
//			    parameter.sp,
//			    parameter.floor,
//			    parameter.skill);
//		}

//#region ICloneable implementation
//
//		public object Clone()
//		{
//			DungeonParameter paramater = new DungeonParameter();
//			paramater._hp = this._hp;
//			paramater._sp = this._sp;
//			paramater._floor = this._floor;
//			paramater._skill = this._skill;
//
//			return paramater;
//		}
//
//#endregion
	}
}