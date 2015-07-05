using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Memoria.Dungeon.BlockUtility
{
	public struct ShapeData
	{
		private int _type;

		public int type
		{
			get
			{
				if (_type < 0 || _type > 10)
				{
					throw new UnityException("Error : Get Block Type, blockType = " + _type);
				}

				return _type;
			}

			set
			{
				if (value < 0 || value > 10)
				{
					throw new UnityException("Error : Get Block Type, blockType = " + _type);
				}

				_type = value;
				_directions = ConvertTypeToFlags(_type);
			}
		}

		private bool[] _directions;

		public bool[] directions
		{
			get
			{
				if (_directions == null)
				{
					_directions = new bool[4];
				}

				return _directions.Clone() as bool[];
			}
		}

		public bool left { get { return directions[0]; } }

		public bool right { get { return directions[1]; } }

		public bool down { get { return directions[2]; } }

		public bool up { get { return directions[3]; } }

		public ShapeData(int type)
		{
			_type = 0;
			_directions = null;
			this.type = type;
		}

		// type から 各方向のオープンフラグへ変換
		private bool[] ConvertTypeToFlags(int type)
		{
			int flags = type + 3;

			if (type > 3)
			{
				flags += 2;
			}
			else if (type > 0)
			{
				flags += 1;
			}

			return Enumerable.Range(0, directions.Length)
				.Select(i => (flags & (1 << i)) != 0)
				.ToArray();
		}
	}
}