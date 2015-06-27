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

		private int _hp;

		public int hp
		{
			get { return _hp; }        

			set
			{ 
				int nextHp = Mathf.Clamp(value, 0, maxHp); 
				DungeonParameter nextParamater = this.Clone() as DungeonParameter;
				nextParamater._hp = nextHp;
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
				_skill = nextSkill;
				changedSkillValue(this, new ParamaterEventArgs(nextParamater));
			}
		}

		public void Set(int maxHp, int hp, int maxSp, int sp, int floor, string skill)
		{
			this.maxHp = maxHp;
			this.hp = hp;
			this.maxSp = maxSp;
			this.sp = sp;
			this.floor = floor;
			this.skill = skill;
		}

		public void Set(DungeonParameter parameter)
		{
			Set(parameter.maxHp,
			    parameter.hp,
			    parameter.maxSp,
			    parameter.sp,
			    parameter.floor,
			    parameter.skill);
		}

#region ICloneable implementation

		public object Clone()
		{
			DungeonParameter paramater = new DungeonParameter();
			paramater._hp = this._hp;
			paramater._sp = this._sp;
			paramater._floor = this._floor;
			paramater._skill = this._skill;

			return paramater;
		}

#endregion
	}
}