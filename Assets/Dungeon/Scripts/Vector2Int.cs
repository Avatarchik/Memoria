using UnityEngine;
using System.Collections;

namespace Memoria.Dungeon
{
	public enum Direction
	{
		Left,
		Right,
		Down,
		Up,
	}

	[System.Serializable]
	public struct Vector2Int
	{
		public int x { get; set; }

		public int y { get; set; }

		public float magnitude { get { return Mathf.Sqrt(SqrMagnitude()); } }

		public float sqrMagnitude { get { return SqrMagnitude(); } }

//		public Vector2Int normalized { get { return new Vector2Int(Sign(x), Sign(y)); } }

		public Vector2Int(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public Vector2Int(Direction direction)
		{
			Vector2Int location = ConvertDirectionToLocation(direction);
			this.x = location.x;
			this.y = location.y;
		}

		public Vector2Int(Vector2 vector)
		{
			this.x = Mathf.FloorToInt(vector.x);
			this.y = Mathf.FloorToInt(vector.y);
		}

		public void Set(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public void Set(Vector2Int location)
		{
			Set(location.x, location.y);
		}

		public void Set(Direction direction)
		{
			Set(ConvertDirectionToLocation(direction));
		}

		public void Move(Vector2Int displacement)
		{
			Set(this + displacement);
		}

		public void Move(Direction direction)
		{
			Set(this + ConvertDirectionToLocation(direction));
		}


//		public void Normalize()
//		{
//			x = Sign(x);
//			y = Sign(y);
//		}
//
//		private int Sign(int value)
//		{
//			return (value > 0) ? 1 : (value < 0) ? -1 : 0;
//		}

		public float SqrMagnitude()
		{
			return x * x + y * y;
		}

		public override string ToString()
		{
			return string.Format("[Vector2Int: x={0}, y={1}]", x, y);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		private static Vector2Int[] directionTable =
			{
				new Vector2Int() { x = -1, y = 0 },
				new Vector2Int() { x = 1, y = 0 },
				new Vector2Int() { x = 0, y = -1 },
				new Vector2Int() { x = 0, y = 1 },
			};

		public static Vector2Int[] directions { get { return directionTable.Clone() as Vector2Int[]; } }

		public static Vector2Int left { get { return directionTable[0]; } }

		public static Vector2Int right { get { return directionTable[1]; } }

		public static Vector2Int down { get { return directionTable[2]; } }

		public static Vector2Int up { get { return directionTable[3]; } }

		public static Vector2Int operator +(Vector2Int v1, Vector2Int v2)
		{
			return new Vector2Int()
			{
				x = v1.x + v2.x,
				y = v1.y + v2.y,
			};
		}

		public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
		{
			return new Vector2Int()
			{
				x = v1.x - v2.x,
				y = v1.y - v2.y,
			};
		}

		public static Vector2Int operator *(Vector2Int v1, Vector2Int v2)
		{
			return new Vector2Int()
			{
				x = v1.x * v2.x,
				y = v1.y * v2.y
			};
		}

		public static bool operator ==(Vector2Int location1, Vector2Int location2)
		{
			return location1.x == location2.x && location1.y == location2.y;
		}

		public static bool operator !=(Vector2Int location1, Vector2Int location2)
		{
			return location1.x != location2.x || location1.y != location2.y;
		}

		public static Vector2Int ConvertDirectionToLocation(Direction direction)
		{
			return directionTable[(int)direction];
		}
	}
}