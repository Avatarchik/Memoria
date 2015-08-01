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
        
        public int floor { get; set; }
        
        public int maxSp { get; set; }

        private int _getKeyNum;

        public int getKeyNum
        {
            get { return _getKeyNum; }
            set { _getKeyNum = Mathf.Clamp(value, 0, allKeyNum); }
        }

        public int allKeyNum { get; set; }
        
        public int silling { get; set; }
        
        private string skill { get; set; }
        
        private int enemyPattern { get; set; }
        private bool isBossBattle { get; set; }

        public DungeonParameter(int maxHp, int hp, int maxSp, int sp, int floor, int allKeyNum, int silling, string skill, int enemyPattern = 0, bool isBossBattle = false)
        {
            this.maxHp = maxHp;
            this.hp = hp;
            this.maxSp = maxSp;
            this.sp = sp;
            this.floor = floor;
            this.allKeyNum = allKeyNum;
            this.getKeyNum = 0;
            this.silling = silling;
            this.skill = skill;
            this.enemyPattern = enemyPattern;
            this.isBossBattle = isBossBattle;
        }
    }
}