using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Memoria.Dungeon.BlockUtility
{
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
				return blockSpriteArray ?? (blockSpriteArray = _blockSprites.Select(s => s.sprites).ToArray());
			}
		}
	}
}