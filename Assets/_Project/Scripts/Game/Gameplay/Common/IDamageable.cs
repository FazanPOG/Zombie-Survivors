namespace _Project.Gameplay
{
    public interface IDamageable
    {
        bool CanTakeDamage { get; }
        void TakeDamage(int damage);
    }
}