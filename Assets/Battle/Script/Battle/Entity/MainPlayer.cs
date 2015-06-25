using Memoria.Battle.Utility;

namespace Memoria.Battle.GameActors
{
    public class MainPlayer : Entity, IDamageable {
        private const int HEALTH_BAR_FULL = 40;
    
        // Use this for initialization
        void Awake () {
            entityType = "Player";
            health = GetComponent<HealthSystem> ();
            health.maxHp = 250;
            health.hp = 250;
        }
    
        // Update is called once per frame

        public void TakeDamage(int i)
        {
            health.hp -= i;
        }

        public void TakeDamage(Damage d)
        {
        
        }

        public bool IsAlive()
        {
            if(health.hp >= 0)
                return true;
            return false;
        }
    }
}