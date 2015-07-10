using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Memoria.Dungeon.BlockUtility
{
	public struct ShapeData
	{
		private static Dictionary<string, int> codeToId = new Dictionary<string, int>()
		{
			{ "0011",  0 },
			{ "0101",  1 },
			{ "0110",  2 },
			{ "0111",  3 },
			{ "1001",  4 },
			{ "1010",  5 },
			{ "1011",  6 },
			{ "1100",  7 },
			{ "1101",  8 },
			{ "1110",  9 },
			{ "1111", 10 },
		};

		private static Dictionary<int, string> idToCode = new Dictionary<int, string>()
		{
			{ 0, "0011" },
			{ 1, "0101" },
			{ 2, "0110" },
			{ 3, "0111" },
			{ 4, "1001" },
			{ 5, "1010" },
			{ 6, "1011" },
			{ 7, "1100" },
			{ 8, "1101" },
			{ 9, "1110" },
			{ 10, "1111" },
		};
		
		//		private int _type;
		//
		//		public int type
		//		{
		//			get
		//			{
		//				if (_type < 0 || _type > 10)
		//				{
		//					throw new UnityException("Error : Get Block Type, blockType = " + _type);
		//				}
		//
		//				return _type;
		//			}
		//
		//			set
		//			{
		//				if (value < 0 || value > 10)
		//				{
		//					throw new UnityException("Error : Get Block Type, blockType = " + _type);
		//				}
		//
		//				_type = value;
		//				_directions = ConvertTypeToFlags(_type);
		//			}
		//		}

		//		private bool[] _directions;
		//
		//		public bool[] directions
		//		{
		//			get
		//			{
		//				if (_directions == null)
		//				{
		//					_directions = new bool[4];
		//				}
		//
		//				return _directions.Clone() as bool[];
		//			}
		//		}

		//		public bool left { get { return directions[0]; } }
		//
		//		public bool right { get { return directions[1]; } }
		//
		//		public bool down { get { return directions[2]; } }
		//
		//		public bool up { get { return directions[3]; } }

		public bool left { get; private set; }

		public bool right { get; private set; }

		public bool down { get; private set; }

		public bool up { get; private set; }

		public ShapeData(int typeID)
		{
			_typeCode = "";
			this.typeID = typeID;
//			_type = 0;
//			_directions = null;
//			this.type = type;
		}

		public ShapeData(string typeCode)
		{
			_typeCode = "";
			this.typeCode = typeCode;
		}

		// type から 各方向のオープンフラグへ変換
		//		private bool[] ConvertTypeToFlags(int type)
		//		{
		//			int flags = type + 3;
		//
		//			if (type > 3)
		//			{
		//				flags += 2;
		//			}
		//			else if (type > 0)
		//			{
		//				flags += 1;
		//			}
		//
		//			return Enumerable.Range(0, directions.Length)
		//				.Select(i => (flags & (1 << i)) != 0)
		//				.ToArray();
		//		}

		private string _typeCode;

		public string typeCode
		{
			get
			{
				return _typeCode;
			}
			set
			{
				left = value[3] == '1';
				right = value[2] == '1';
				down = value[1] == '1';
				up = value[0] == '1';

				_typeCode = value;
			}
		}

		public int typeID
		{
			get
			{
				return codeToId[typeCode];
			}
			set
			{
				typeCode = idToCode[value];
			}
		}

		public bool Opend(Vector2Int baseDirection)
		{
			return new []
			{
				new { checkDirection = Vector2Int.left,	opend = left },
				new { checkDirection = Vector2Int.right,opend = right },
				new { checkDirection = Vector2Int.down, opend = down },
				new { checkDirection = Vector2Int.up, opend = up },
			}
				.Where(param => baseDirection == param.checkDirection)
				.Any(param => param.opend);
			
//			if (direction == Vector2Int.left)
//			{
//				return right;
//			}
//			if (direction == Vector2Int.right)
//			{
//				return left;
//			}
//			if (direction == Vector2Int.down)
//			{
//				return up;
//			}
//			if (direction == Vector2Int.up)
//			{
//				return down;
//			}
//
//			return false;
		}
	}
}