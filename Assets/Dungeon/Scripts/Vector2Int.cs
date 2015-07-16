using UnityEngine;

namespace Memoria.Dungeon
{
    [System.Serializable]
    public struct Vector2Int
    {
        [SerializeField]
        private int _x;
        public int x
        {
            get { return _x; }
            set { _x = value; }
        }

        [SerializeField]
        private int _y;
        public int y
        {
            get { return _y; }
            set { _y = value; }
        }

        public Vector2Int(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int SqrMagnitude()
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

        public static Vector2Int left { get { return new Vector2Int(-1, 0); } }

        public static Vector2Int right { get { return new Vector2Int(1, 0); } }

        public static Vector2Int down { get { return new Vector2Int(0, -1); } }

        public static Vector2Int up { get { return new Vector2Int(0, 1); } }

        public static Vector2Int zero { get { return new Vector2Int(0, 0); } }

        public static Vector2Int operator -(Vector2Int v)
        {
            return new Vector2Int(-v.x, -v.y);
        }

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
            return v1 + (-v2);
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
    }
}