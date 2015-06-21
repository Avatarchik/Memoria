using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockList : MonoBehaviour
{
	public List<BlockFactor> blockFactors = new List<BlockFactor>();
    protected DungeonManager dungeonManager { get; private set; }
    protected BlockManager blockManager { get; private set; }
    protected ParameterManager paramaterManager { get; private set; }
	private bool[] flags;

	// Use this for initialization
	protected virtual void Start()
	{
        Initialize();
		flags = new bool[blockManager.NumberOfBlockShapeType];
		RandomizeBlockList(true);
	}
	
	// Update is called once per frame
//	void Update()
//	{	
//	}

    protected void Initialize()
    {
        dungeonManager = DungeonManager.instance;
        blockManager = dungeonManager.blockManager;
        paramaterManager = dungeonManager.parameterManager;
    }

	public void RandomizeBlockList(bool initialize = false)
    {
        if (dungeonManager.activeState != DungeonState.None)
        {
            return;
        }

		bool[] nextFlags = new bool[blockManager.NumberOfBlockShapeType];

		foreach(BlockFactor blockFactor in blockFactors)
		{
			int shapeType;
			do
			{
				shapeType = blockManager.GetRandomBlockShapeType();
			}
			while (flags[shapeType] || nextFlags[shapeType]);

			nextFlags[shapeType] = true;

			if (initialize)
			{
				blockFactor.CreateBlock(shapeType);
            }
            else
            {
                blockFactor.SetBlock(shapeType);
                //paramaterManager.paramater.sp -= 1;
            }
        }

		if (!initialize)
		{
			paramaterManager.parameter.sp -= 2;
		}

        flags = nextFlags;
	}
}