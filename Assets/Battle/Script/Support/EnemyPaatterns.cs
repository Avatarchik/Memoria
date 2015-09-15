using System.Collections.Generic;
using System;

namespace Memoria.Battle.GameActors
{
    public class EnemyPatterns
    {

        private static Type[] SLIMES =
            {
                typeof(ThunderSlime),
                typeof(FireSlime),
                typeof(WaterSlime),
                typeof(WindSlime)
            };

        private static Type[] BOSS =
            {
                typeof(FireBoss),
                typeof(WaterBoss),
                typeof(WindBoss)
            };

        private readonly List<Type[]> _bossPatterns = new List<Type[]>
        {
            new Type[]{ SLIMES[1], BOSS[0], SLIMES[1] },
            new Type[]{ SLIMES[2], BOSS[1], SLIMES[2] },
            new Type[]{ SLIMES[3], BOSS[2], SLIMES[3] }
        };

        private readonly List<List<Type[]>> _normalPatterns = new List<List<Type[]>>
            {
                // Pattern 3
                new List<Type[]>
                {
                    new Type[] { SLIMES[0], SLIMES[1], SLIMES[2], SLIMES[3] },
                    new Type[] { SLIMES[0], SLIMES[0], SLIMES[2] },
                    new Type[] { SLIMES[0], SLIMES[0], SLIMES[3] },
                    new Type[] { SLIMES[1], SLIMES[1], SLIMES[0] },
                    new Type[] { SLIMES[1], SLIMES[1], SLIMES[3] },
                    new Type[] { SLIMES[2], SLIMES[2], SLIMES[1] },
                    new Type[] { SLIMES[3], SLIMES[3], SLIMES[0] },
                    new Type[] { SLIMES[0], SLIMES[1], SLIMES[2] },
                    new Type[] { SLIMES[1], SLIMES[2], SLIMES[3] },
                    new Type[] { SLIMES[2], SLIMES[3] },
                    new Type[] { SLIMES[3], SLIMES[1] },
                    new Type[] { SLIMES[2], SLIMES[1] },
                    new Type[] { SLIMES[0], SLIMES[2] },
                    new Type[] { SLIMES[0], SLIMES[3] },
                    new Type[] { SLIMES[0], SLIMES[1] },
                },
                // Pattern 2
                new List<Type[]>
                {
                    new Type[] { SLIMES[0], SLIMES[0], SLIMES[0], SLIMES[0] },
                    new Type[] { SLIMES[1], SLIMES[1], SLIMES[1], SLIMES[1] },
                    new Type[] { SLIMES[2], SLIMES[2], SLIMES[2], SLIMES[2] },
                    new Type[] { SLIMES[3], SLIMES[3], SLIMES[3], SLIMES[3] },
                    new Type[] { SLIMES[0], SLIMES[0], SLIMES[0] },
                    new Type[] { SLIMES[1], SLIMES[1], SLIMES[1] },
                    new Type[] { SLIMES[2], SLIMES[2], SLIMES[2] },
                    new Type[] { SLIMES[3], SLIMES[3], SLIMES[3] },
                    new Type[] { SLIMES[0], SLIMES[0] },
                    new Type[] { SLIMES[1], SLIMES[1] },
                    new Type[] { SLIMES[2], SLIMES[2] },
                    new Type[] { SLIMES[3], SLIMES[3] },
                },
                // Pattern 1
                new List<Type[]>
                {
                    new Type[] { SLIMES[0], SLIMES[0], SLIMES[3], SLIMES[3] },
                    new Type[] { SLIMES[1], SLIMES[1], SLIMES[3], SLIMES[3] },
                    new Type[] { SLIMES[0], SLIMES[0], SLIMES[1], SLIMES[1] },
                    new Type[] { SLIMES[0], SLIMES[0], SLIMES[2], SLIMES[2] },
                    new Type[] { SLIMES[1], SLIMES[1], SLIMES[2], SLIMES[2] },
                    new Type[] { SLIMES[2], SLIMES[2], SLIMES[3], SLIMES[3] },
                    new Type[] { SLIMES[0], SLIMES[0], SLIMES[1] },
                    new Type[] { SLIMES[1], SLIMES[1], SLIMES[2] },
                    new Type[] { SLIMES[2], SLIMES[2], SLIMES[3] },
                    new Type[] { SLIMES[2], SLIMES[2], SLIMES[0] },
                    new Type[] { SLIMES[3], SLIMES[3], SLIMES[1] },
                    new Type[] { SLIMES[3], SLIMES[3], SLIMES[2] },
                    new Type[] { SLIMES[0], SLIMES[2], SLIMES[3] },
                    new Type[] { SLIMES[0], SLIMES[1], SLIMES[3] },
                }
        };
        public EnemyPatterns(){}

        public Type[] GetBossPattern(int id)
        {
            return _bossPatterns[id];
        }
        public Type[] GetNormalPattern(int floor, int id)
        {
            return _normalPatterns[floor][id];
        }
    }
}