using UnityEngine;
using System;

public class HealthComponent : MonoBehaviour
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action OnDeath;

    public void Initialize(int maxHp)
    {
        MaxHealth = maxHp;
        CurrentHealth = maxHp;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(int dmg)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - dmg, 0);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth == 0) OnDeath?.Invoke();
    }

    /// <summary>
    /// Восстанавливает здоровье, не превышая MaxHealth.
    /// </summary>
    public void Heal(int amount)
    {
        if (CurrentHealth <= 0) return;      // не воскрешаем «мертвых»
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
}
