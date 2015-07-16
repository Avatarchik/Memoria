using UnityEngine;

namespace Memoria.Dungeon
{
    public struct DungeonParameter
    {
        private int _hp;

        public int hp
        {
            get { return _hp; }
            set { _hp = Mathf.Clamp(value, 0, maxHp); }
        }

        public int maxHp { get; set; }

        private int _sp;

        public int sp
        {
            get { return _sp; }
            set { _sp = Mathf.Clamp(value, 0, maxSp); }
        }

        public int maxSp { get; set; }

        private int floor { get; set; }

        private string skill { get; set; }

        public DungeonParameter(int maxHp, int hp, int maxSp, int sp, int floor, string skill)
        {
            this.maxHp = maxHp;
            this.hp = hp;
            this.maxSp = maxSp;
            this.sp = sp;
            this.floor = floor;
            this.skill = skill;
        }
    }
}