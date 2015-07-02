using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Memoria.Dungeon.BlockUtility;

namespace Memoria.Dungeon.Managers
{
	public class MapManager : MonoBehaviour
	{
		/// <summary>
		/// マップ
		/// </summary>
		public Dictionary<Vector2Int, Block> map = new Dictionary<Vector2Int, Block>();

		private DungeonManager _dungeonManager;
		private DungeonManager dungeonManager
		{
			get
			{
				if (_dungeonManager == null)
				{
					_dungeonManager = DungeonManager.instance;
				}

				return _dungeonManager;
			}
		}

		private BlockManager blockManager;

		private Rect _canPutBlockArea = new Rect(-7, -5, 14, 10);

		public Rect canPutBlockArea
		{
			get
			{
				Rect ret = _canPutBlockArea;
				ret.position += (Vector2)Camera.main.transform.position;
				return ret;
			} 
		}

		void Awake()
		{
//			dungeonManager = DungeonManager.instance;
			blockManager = dungeonManager.blockManager;

			//SetMap("");
		}

		// Use this for initialization
		//	void Start()
		//	{
		//	}
	
		// Update is called once per frame
		//	void Update()
		//	{
		//	}

		public void SetMap(List<BlockData> blockDatas)
		{
			foreach (var data in blockDatas)
			{
				blockManager.CreateBlock(null, data, true);
			}
		}

		/// <summary>
		/// 指定の位置からマップ上に配置されるときの位置を取得する
		/// </summary>
		/// <returns>マップ上に配置されるときの位置</returns>
		/// <param name="position">指定の位置</param>
		public Vector3 ConvertPosition(Vector3 position)
		{
			Vector2Int location = ToLocation(position);
			Vector3 converted = ToPosition(location);
			return converted;
		}

		/// <summary>
		/// 指定の位置からマップ座標を取得する
		/// </summary>
		/// <returns>マップ座標</returns>
		/// <param name="position">指定の位置</param>
		public Vector2Int ToLocation(Vector3 position)
		{
			Vector2 blockSize = dungeonManager.blockSize;
			Vector2Int location = new Vector2Int();

			location.x = (int)Mathf.Round(position.x / blockSize.x * 100);
			location.y = (int)Mathf.Round(position.y / blockSize.y * 100);

			return location;
		}

		/// <summary>
		/// 指定のマップ座標から位置を取得する
		/// </summary>
		/// <returns>位置</returns>
		/// <param name="location">指定のマップ座標</param>
		public Vector3 ToPosition(Vector2Int location)
		{
			Vector2 blockSize = dungeonManager.blockSize;
			Vector3 position = new Vector3();
		
			position.x = location.x / 100f * blockSize.x;
			position.y = location.y / 100f * blockSize.y;
			position.z = 0;

			return position;
		}
	}
}