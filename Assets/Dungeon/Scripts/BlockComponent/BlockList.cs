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

            int counter = 0;
            blockFactors.ForEach(blockFactor =>
                {
                    ShapeData randomShapeData = blockManager.GetRandomShapeData(shapeType => !flags[shapeType] && !nextFlags[shapeType]);
                    BlockType randomBlockType = blockManager.GetRandomBlockType(blockType => blockType != BlockType.None);

                    blockFactor.SetBlock(randomShapeData, randomBlockType);
                    
                    var effectPosition = blockFactor.transform.position;
                    effectPosition.z = 0;
                    var effect = EffectManager.instance.InstantiateEffect(5, effectPosition, 2f);
                    
                    if (counter > 0)
                    {
                        Destroy(effect.GetComponent<AudioSource>());
                    }

                    counter++;

                    nextFlags[randomShapeData.typeID] = true;
                });

            if (onRandomize != null)
            {
                onRandomize.OnNext(Unit.Default);
            }

            flags = nextFlags;
        }
        
        public void SetAttributeBlockList(BlockType attribute)
        {
            int counter = 0;
            blockFactors.ForEach(blockFactor =>
                {
                    ShapeData shapeData = blockFactor.block.shapeData;
                    BlockType blockType = attribute;
                    
                    blockFactor.SetBlock(shapeData, blockType);
                    
                    var effectPosition = blockFactor.transform.position;
                    effectPosition.z = 0;
                    
                    var index = 5;
                    switch (attribute)
                    {
                        case BlockType.Thunder:
                            index = 6;
                            break;
                            
                        case BlockType.Water:
                            index = 7;
                            break;
                            
                        case BlockType.Fire:
                            index = 8;
                            break;
                            
                        case BlockType.Wind:
                            index = 9;
                            break;
                            
                        case BlockType.Recovery:
                            index = 10;
                            break;
                    }
                    
                    var effect = EffectManager.instance.InstantiateEffect(index, effectPosition, 2f);
                    
                    if (counter > 0)
                    {
                        Destroy(effect.GetComponent<AudioSource>());
                    }
                    
                    counter++;
                });
        }
        
        private bool CanRandomize(int sp)
        {
            int consumption = 2;
            return sp > consumption;
        }
    }
}