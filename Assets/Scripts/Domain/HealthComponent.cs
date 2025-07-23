using UnityEngine;
using System;
using UnityEngine.UIElements;

public class HealthComponent : MonoBehaviour
{
    private bool isAlive = true;

    public bool IsAlive => isAlive;
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
        if (IsAlive)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - dmg, 0);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            EffectsPool.Instance.PlayEffect("Hurt", transform.position, 0.5f);
            if (CurrentHealth <= 0)
            {
                isAlive = false;
                EffectsPool.Instance.PlayEffect("Death", transform.position, 0.5f);
                OnDeath?.Invoke();
            }
        }
        
    }

    /// <summary>
    /// Восстанавливает здоровье, не превышая MaxHealth.
    /// </summary>
    public void Heal(int amount)
    {
        if (CurrentHealth <= 0) return;      // не воскрешаем «мертвых»
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        EffectsPool.Instance.PlayEffect("Heal", transform.position, 0.5f);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
}
