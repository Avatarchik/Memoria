using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Memoria.Dungeon
{

	public class ParamaterEventArgs : EventArgs
	{
		public DungeonParameter parameter { get; private set; }

		public ParamaterEventArgs(DungeonParameter parameter)
		{
			this.parameter = parameter;
		}
	}

	public class DungeonParameter : ICloneable
	{
		// level
		//public event EventHandler<ParamaterEventArgs> changingLevelValue = (s, e) => {};
//		public event EventHandler<ParamaterEventArgs> changedLevelValue = (s, e) => {};
		// hp
		//public event EventHandler<ParamaterEventArgs> changingHpValue = (s, e) => {};
		public event EventHandler<ParamaterEventArgs> changedHpValue = (s, e) => {};
		// sp
		//public event EventHandler<ParamaterEventArgs> changingSpValue = (s, e) => {};
		public event EventHandler<ParamaterEventArgs> changedSpValue = (s, e) => {};
		// floor
		//public event EventHandler<ParamaterEventArgs> changingFloorValue = (s, e) => {};
		public event EventHandler<ParamaterEventArgs> changedFloorValue = (s, e) => {};
		// skill
		//public event EventHandler<ParamaterEventArgs> changingSkillValue = (s, e) => {};
		public event EventHandler<ParamaterEventArgs> changedSkillValue = (s, e) => {};
		// tp
		//public event EventHandler<ParamaterEventArgs> changingTpValue = (s, e) => {};
//		public event EventHandler<ParamaterEventArgs> changedTpValue = (s, e) => {};

//		public int _level;
//
//		public int level
//		{
//			get { return _level; }
//
//			set
//			{
//				int nextLevel = value;
//				DungeonParameter nextParamater = this.Clone() as DungeonParameter;
//				nextParamater._level = nextLevel;
//				//changingLevelValue(this, new ParamaterEventArgs(nextParamater));
//				_level = nextLevel;
//				changedLevelValue(this, new ParamaterEventArgs(nextParamater));
//			}
//		}

		private int _hp;

		public int hp
		{
			get { return _hp; }        

			set
			{ 
				int nextHp = Mathf.Clamp(value, 0, maxHp); 
				DungeonParameter nextParamater = this.Clone() as DungeonParameter;
				nextParamater._hp = nextHp;
				//changingHpValue(this, new ParamaterEventArgs(nextParamater));
				_hp = nextHp;
				changedHpValue(this, new ParamaterEventArgs(nextParamater));
			}
		}

		public int maxHp { get; set; }

		private int _sp;

		public int sp
		{
			get { return _sp; }        
			set
			{
				int nextSp = Mathf.Clamp(value, 0, maxSp); 
				DungeonParameter nextParamater = this.Clone() as DungeonParameter;
				nextParamater._sp = nextSp;
				//changingSpValue(this, new ParamaterEventArgs(nextParamater));
				_sp = nextSp;
				changedSpValue(this, new ParamaterEventArgs(nextParamater));
			}
		}

		public int maxSp { get; set; }

		private int _floor;

		public int floor
		{
			get { return _floor; }

			set
			{
				int nextFloor = value;
				DungeonParameter nextParamater = this.Clone() as DungeonParameter;
				nextParamater._floor = nextFloor;
				//changingFloorValue(this, new ParamaterEventArgs(nextParamater));
				_floor = nextFloor;
				changedFloorValue(this, new ParamaterEventArgs(nextParamater));
			}
		}

		private string _skill;

		public string skill
		{
			get { return _skill; }
			set
			{
				string nextSkill = value;
				DungeonParameter nextParamater = this.Clone() as DungeonParameter;
				nextParamater._skill = nextSkill;
				//changingSkillValue(this, new ParamaterEventArgs(nextParamater));
				_skill = nextSkill;
				changedSkillValue(this, new ParamaterEventArgs(nextParamater));
			}
		}

//		private int _tp;
//
//		public int tp
//		{
//			get { return _tp; }
//        
//			set
//			{
//				int nextTp = Mathf.Max(value, 0);
//				DungeonParameter nextParamater = this.Clone() as DungeonParameter;
//				nextParamater._tp = nextTp;
//				//changingTpValue(this, new ParamaterEventArgs(nextParamater));
//				_tp = nextTp;
//				changedTpValue(this, new ParamaterEventArgs(nextParamater));
//			}
//		}

//		public void Set(int level, int maxHp, int hp, int maxSp, int sp, int floor, string skill) //, int tp)
		public void Set(int maxHp, int hp, int maxSp, int sp, int floor, string skill)
		{
//			this.level = level;
			this.maxHp = maxHp;
			this.hp = hp;
			this.maxSp = maxSp;
			this.sp = sp;
			this.floor = floor;
			this.skill = skill;
//			this.tp = tp;
		}

		public void Set(DungeonParameter parameter)
		{
			Set(//parameter.level,
			    parameter.maxHp,
			    parameter.hp,
			    parameter.maxSp,
			    parameter.sp,
			    parameter.floor,
			    parameter.skill);
//			   parameter.skill,
//			   parameter.tp);
		}

#region ICloneable implementation

		public object Clone()
		{
			DungeonParameter paramater = new DungeonParameter();
//			paramater._level = this._level;
			paramater._hp = this._hp;
			paramater._sp = this._sp;
			paramater._floor = this._floor;
			paramater._skill = this._skill;
//			paramater._tp = this._tp;

			return paramater;
		}

#endregion
	}
}