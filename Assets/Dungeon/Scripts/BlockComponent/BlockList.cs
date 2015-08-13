using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Memoria.Dungeon.Managers;
using UniRx;

namespace Memoria.Dungeon.BlockComponent
{
    public class BlockList : MonoBehaviour
    {
        private static DungeonManager dungeonManager { get { return DungeonManager.instance; } }
        private static BlockManager blockManager { get { return BlockManager.instance; } }

        public List<BlockFactor> blockFactors = new List<BlockFactor>();

        private bool[] flags;

        [SerializeField]
        private Button randomizeButton;

        private Subject<Unit> onRandomize;

        public IObservable<Unit> OnRandomizeAsObservable()
        {
            return onRandomize ?? (onRandomize = new Subject<Unit>());
        }

        // Use this for initialization
        private void Start()
        {
            CreateBlockList();

            // ランダマイズの登録
            randomizeButton.OnClickAsObservable()
            .Where(_ => dungeonManager.activeState == DungeonState.None)
            .Where(_ => CanRandomize(ParameterManager.instance.parameter.sp))
            .Subscribe(RandomizeBlockList);
        }

        private void CreateBlockList()
        {
            flags = new bool[blockManager.NumberOfBlockShapeType];

            blockFactors.ForEach(blockFactor =>
                {
                    ShapeData randomShapeData = blockManager.GetRandomShapeData(shapeType => !flags[shapeType]);
                    BlockType randomBlockType = blockManager.GetRandomBlockType(blockType => blockType != BlockType.None);

                    blockFactor.CreateBlock(randomShapeData, randomBlockType);

                    flags[randomShapeData.typeID] = true;
                });
        }

        public void RandomizeBlockList(Unit _ = null)
        {
            bool[] nextFlags = new bool[blockManager.NumberOfBlockShapeType];

            blockFactors.ForEach(blockFactor =>
                {
                    ShapeData randomShapeData = blockManager.GetRandomShapeData(shapeType => !flags[shapeType] && !nextFlags[shapeType]);
                    BlockType randomBlockType = blockManager.GetRandomBlockType(blockType => blockType != BlockType.None);

                    blockFactor.SetBlock(randomShapeData, randomBlockType);

                    nextFlags[randomShapeData.typeID] = true;
                });

            if (onRandomize != null)
            {
                onRandomize.OnNext(Unit.Default);
            }

            flags = nextFlags;
        }
        
        private bool CanRandomize(int sp)
        {
            int consumption = 2;
            return sp > consumption;
        }
    }
}