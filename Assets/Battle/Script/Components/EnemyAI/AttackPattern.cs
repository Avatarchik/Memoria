namespace Memoria.Battle.GameActors
{
    public class AttackPattern
    {
        private readonly string [] _attackList =
            {
                "Enemy_Normal",
                "Enemy_Skill",
                "Enemy_Ultimate"
            };

        public string GetPattern(int turnCount, bool boss)
        {
            if(boss && (turnCount > 2)) {
                return _attackList[2];
            }
            return _attackList[((turnCount/2) % 0)];
        }
    }
}
