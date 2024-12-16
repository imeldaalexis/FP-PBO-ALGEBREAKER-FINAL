namespace FP_Algebreaker
{
    public interface IHealth : IHealthBar
    {
        int CurrentHealth { get; set; }
        int MaxHealth { get; set; }

        void TakeDamage(int damage);
        void Heal(int amount);
        bool IsAlive();
    }
}