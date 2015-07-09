namespace Memoria.Battle.GameActors
{
    public interface IDamageable
    {
        void TakeDamage(Damage d);
        void ShowDamage();
        bool IsAlive();
    }
}