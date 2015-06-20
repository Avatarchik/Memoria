using UnityEngine;
using System.Collections;

public enum Direction
{
	Left,
	Right,
	Down,
	Up,
}



[System.Serializable]
public struct Location
{
	public int x { get; set; }
	
	public int y { get; set; }
	
	public float magnitude { get { return Mathf.Sqrt(SqrMagnitude()); } }
	
	public float sqrMagnitude { get { return SqrMagnitude(); } }

    public Location normalized { get { return new Location(Sign(x), Sign(y)); } }

	public Location(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	
	public Location(Direction direction)
	{
		Location location = ConvertDirectionToLocation(direction);
		this.x = location.x;
		this.y = location.y;
	}
	
	public void Set(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	
	public void Set(Location location)
	{
		Set(location.x, location.y);
	}
	
	public void Set(Direction direction)
	{
		Set(ConvertDirectionToLocation(direction));
	}
	
	public void Move(Location displacement)
	{
		Set(this + displacement);
	}
	
	public void Move(Direction direction)
	{
		Set(this + ConvertDirectionToLocation(direction));
	}

    public void Normalize()
    {
        x = Sign(x);
        y = Sign(y);
    }

    private int Sign(int value)
    {
        return (value > 0) ? 1 : (value < 0) ? -1 : 0;
    }

	public float SqrMagnitude()
	{
		return x * x + y * y;
	}
	
	public override string ToString()
	{
		return string.Format("[Location: x={0}, y={1}]", x, y);
	}

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

	private static Location[] directionTable =
	{
		new Location() { x = -1, y = 0 },
		new Location() { x = 1, y = 0 },
		new Location() { x = 0, y = -1 },
		new Location() { x = 0, y = 1 },
	};

    public static Location[] directions { get { return directionTable.Clone() as Location[]; } }
	
	public static Location left { get { return directionTable[0]; } }
	
	public static Location right { get { return directionTable[1]; } }
	
	public static Location down { get { return directionTable[2]; } }
	
	public static Location up { get { return directionTable[3]; } }
	
	public static Location operator +(Location location1, Location location2)
	{
		return new Location()
		{
			x = location1.x + location2.x,
			y = location1.y + location2.y,
		};
	}
	
	public static Location operator -(Location location1, Location location2)
	{
		return new Location()
		{
			x = location1.x - location2.x,
			y = location1.y - location2.y,
		};
	}

    public static bool operator ==(Location location1, Location location2)
    {
        return location1.x == location2.x && location1.y == location2.y;
    }

    public static bool operator !=(Location location1, Location location2)
    {
        return location1.x != location2.x || location1.y != location2.y;
    }
	
	public static Location ConvertDirectionToLocation(Direction direction)
	{
		return directionTable[(int)direction];
	}
}