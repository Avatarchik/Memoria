using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class BlockSprites
{
	[Serializable]
	class Sprites
	{
		public Sprite[] sprites = new Sprite[11];
	}

	[SerializeField]
	private List<Sprites> _blockSprites = new List<Sprites>();

	private Sprite[][] blockSpriteArray = null;

	public Sprite[][] blockSprites
	{
		get
		{
			if (blockSpriteArray == null)
			{
				List<Sprite[]> sl = new List<Sprite[]>();

				foreach (Sprites sprites in _blockSprites)
				{
					sl.Add(sprites.sprites);
				}

				blockSpriteArray = sl.ToArray();
			}

			return blockSpriteArray;
		}
	}
}
