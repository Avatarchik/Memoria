using Memoria.Battle.Utility;

namespace Memoria.Battle.GameActors
{
    public class MainPlayer : Entity, IDamageable
    {
        private const int HEALTH_BAR_FULL = 40;

        // Use this for initialization
        void Awake ()
        {
            entityType = "Player";
            health = GetComponent<HealthSystem> ();
            health.maxHp = 250;
            health.hp = 250;
            parameter.elementAff = new NoElement(Element.NONE);
        }

        // Update is called once per frame

        public void Heal(int healValue)
        {
            var effectiveHeal = (healValue * (-1));
            if(health.hp + effectiveHeal > health.maxHp) {
                health.hp = health.maxHp;
            }
            else {
                health.hp += effectiveHeal;
            }
        }

        public void TakeDamage(Damage damage)
        {
            damage.TargetParameters = parameter;

            if(damage.Calculate() < 0) {
                Heal(damage.Calculate());
            }
            else {
                health.hp -= damage.Calculate();
                ShowDamage();
            }
        }

        public void ShowDamage()
        {

        }

        public bool IsAlive()
        {
            return (health.hp >= 0);
        }
    }
}