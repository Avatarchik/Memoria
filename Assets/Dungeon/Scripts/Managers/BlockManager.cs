﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlockManager : MonoBehaviour
{
	[SerializeField]
	private GameObject blockPrefab;

	public readonly int NumberOfBlockType = 11;

	[SerializeField]
	private BlockSprites _blockSprites = new BlockSprites();

	public Sprite[][] blockSprites { get { return _blockSprites.blockSprites; } }

	[SerializeField]
	private GameObject noneBlock;

	[SerializeField]
	private GameObject battleBlock;

	[SerializeField]
	private GameObject acquisitionBlock;

	[SerializeField]
	private GameObject subRecoveryBlock;

	[SerializeField]
	private GameObject recoveryBlock;

	[SerializeField]
	private GameObject trapBlock;

	private List<GameObject> colorBlocks;

	[SerializeField]
	private ColorBlockList noneBlockList;

	[SerializeField]
	private ColorBlockList battleBlockList;

	[SerializeField]
	private ColorBlockList acquisitionBlockList;

	[SerializeField]
	private ColorBlockList subRecoveryBlockList;

	[SerializeField]
	private ColorBlockList recoveryBlockList;

	[SerializeField]
	private ColorBlockList trapBlockList;

	public List<ColorBlockList> colorBlockLists { get; private set; }

	void Awake()
	{
		colorBlocks = new List<GameObject>()
        {
            noneBlock,
            battleBlock,
            acquisitionBlock,
            subRecoveryBlock,
            recoveryBlock,
            trapBlock,
        };

		colorBlockLists = new List<ColorBlockList>()
        {
            noneBlockList,
            battleBlockList,
            acquisitionBlockList,
            subRecoveryBlockList,
            recoveryBlockList,
            trapBlockList,
        };
	}

	// Use this for initialization
	//	void Start()
	//	{
	//	}

	// Update is called once per frame
	//	void Update()
	//	{	
	//	}

	public Block CreateBlock(BlockFactor blockFactor, int shapeType = 0, BlockType type = BlockType.None, bool isDefault = false, Location location = default(Location))
	{
		GameObject blockObject = Instantiate<GameObject>(blockPrefab);
		Block block = blockObject.GetComponent<Block>();
		block.Initialize();
		block.blockFactor = blockFactor;

		BlockShape shape = new BlockShape();
		shape.type = shapeType;

		if (isDefault)
		{
			block.SetAsDefault(location, shape, type);
		}
		else
		{
			block.shapeType = shapeType;
			block.type = type;
			block.onEventType = type;
			block.location = location;
		}

		return block;
	}

	public Block CreateBlock(BlockFactor blockFactor, BlockData blockData, bool isDefault = false)
	{
		int shapeType = blockData.shape.type;
		return CreateBlock(blockFactor, shapeType, blockData.type, isDefault, blockData.location);
	}

	public int GetRandomBlockShapeType()
	{
		int min = 0;
		int max = NumberOfBlockType;
		return Random.Range(min, max);
	}

	public Sprite GetBlockSprite(BlockShape shape, BlockType type)
	{
		return blockSprites[(int)type][shape.type];
	}

	public void ActivateColorBlockList(int id)
	{
		if (DungeonManager.instance.activeState != DungeonState.None)
		{
			return;
		}

		colorBlocks.ForEach(colorBlockList =>
		{
			//            colorBlockList.gameObject.SetActive(false);
		});

		//        colorBlocks[id].gameObject.SetActive(true);
	}

	//public void SetColorBlockList(List<List<BlockData>> blockDataLists)
	//{
	//	foreach (var item in colorBlockLists.Zip(blockDataLists, (blockList, datas) => new {blockList, datas }))
	//	{
	//		item.blockList.CreateColorBlockList(item.datas);
	//	}
	//}
}