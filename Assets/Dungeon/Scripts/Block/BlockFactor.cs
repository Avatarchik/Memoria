using UnityEngine;
using System.Collections;

public class BlockFactor : MonoBehaviour
{
	public Block block { get; set; }
	private BlockManager blockManager;

	void Awake()
	{
		blockManager = DungeonManager.instance.blockManager;
	}

	public virtual void OnPutBlock()
	{
		int shapeType = blockManager.GetRandomBlockShapeType();
		CreateBlock(shapeType);
	}

	public void CreateBlock(int shapeType = 0, BlockType type = BlockType.None)
	{
		block = blockManager.CreateBlock(this, shapeType, type);
	}

	public void SetBlock(int shapeType = 0, BlockType type = BlockType.None)
	{
		block.shapeType = shapeType;
		block.type = type;
		block.onEventType = type;
	}
}