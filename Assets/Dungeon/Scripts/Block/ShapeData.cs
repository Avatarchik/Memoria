using System.Collections.Generic;
using System.Linq;

namespace Memoria.Dungeon.BlockComponent
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

		public bool left { get; private set; }

		public bool right { get; private set; }

		public bool down { get; private set; }

		public bool up { get; private set; }

		public ShapeData(int typeID)
		{
			_typeCode = "";
			this.typeID = typeID;
		}

		public ShapeData(string typeCode)
		{
			_typeCode = "";
			this.typeCode = typeCode;
		}

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
		}
	}
}