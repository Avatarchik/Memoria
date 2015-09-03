using UnityEngine;
using Memoria.Dungeon.BlockComponent;

namespace Memoria.Dungeon
{
    [System.Serializable]
    public struct AttributeIncidence
    {
        public float fire;
        public float wind;
        public float thunder;
        public float water;
        public float recovery;

        public float GetIncidence(BlockType attribute)
        {
            switch (attribute)
            {
                case BlockType.Fire:
                    return fire;

                case BlockType.Wind:
                    return wind;

                case BlockType.Thunder:
                    return thunder;

                case BlockType.Water:
                    return water;

                case BlockType.Recovery:
                    return recovery;
            }

            throw new UnityException("It could not be converted to incidence from the attribute `" + attribute + "`");
        }

        public BlockType GetRandomAttribute()
        {
            float rangeOfFire = 0 + fire;
            float rangeOfWind = rangeOfFire + wind;
            float rangeOfThunder = rangeOfWind + thunder;
            float rangeOfWater = rangeOfThunder + water;
            float rangeOfRecovery = rangeOfWater + recovery;

            float value = Random.Range(0, rangeOfRecovery);
            if (value < rangeOfFire)
            {
                return BlockType.Fire;
            }
            else if (value < rangeOfWind)
            {
                return BlockType.Wind;
            }
            else if (value < rangeOfThunder)
            {
                return BlockType.Thunder;
            }
            else if (value < rangeOfWater)
            {
                return BlockType.Water;
            }
            else
            {
                return BlockType.Recovery;
            }
        }
    }
}