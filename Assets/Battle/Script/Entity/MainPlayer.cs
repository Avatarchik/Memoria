using UnityEngine;
using Memoria.Battle.Utility;
using Memoria.Managers;

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
            var healthBar = GameObject.FindObjectOfType<HealthBar>();
            damage.TargetParameters = parameter;

            if(damage.totalDamage < 0) {
                Heal(damage.totalDamage);
                damage.Appear(healthBar.gameObject.transform.position, true);
                SoundManager.instance.PlaySound(8);
            }
            else {
                health.hp -= damage.Calculate();
                damage.Appear(healthBar.gameObject.transform.position);
                SoundManager.instance.PlaySound(6);
            }
        }

        public bool IsAlive()
        {
            return (health.hp >= 0);
        }
    }
}