using UnityEngine;
using System.Collections;

public struct BlockShape
{
	private int _type;
	public int type
	{
		get
		{
			if (_type < 0 | _type > 10)
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
			int flags = value + 3;

			if (value > 3)
			{
				flags += 2;
			}
			else if (value > 0)
			{
				flags += 1;
			}

			for (int i = 0; i < directions.Length; i++)
			{
				_directions[i] = ((flags & (1 << i))) != 0;
			}
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

	public BlockShape(int type)
	{
		_type = 0;
		_directions = null;
		this.type = type;
	}
}