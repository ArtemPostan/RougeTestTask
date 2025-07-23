using System.Collections;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance { get; private set; }
    
    public float hitDelay = 0.3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void RegisterAttack(AttackComponent attack)
    {
        // Чтобы избежать дублирования
        attack.OnAttackAttempt -= HandleAttackAttempt;
        attack.OnAttackAttempt += HandleAttackAttempt;
    }

    private void HandleAttackAttempt(ICharacter source, ICharacter target)
    {
        StartCoroutine(DelayedHit(source, target));
    }

    private IEnumerator DelayedHit(ICharacter source, ICharacter target)
    {
        yield return new WaitForSeconds(hitDelay);
       
        if (target.Health.CurrentHealth <= 0 || source.Health.CurrentHealth <= 0)
            yield break;

        // 1) Урон
        int baseDmg = source.Attack.BaseDamage;
        float bonusPct = source.Buffs.DamageBonusPercent;
        int dmg = Mathf.RoundToInt(baseDmg * (1 + bonusPct));
        target.Health.TakeDamage(dmg);

        // 2) Оглушение 
        if (source.Attack.StunChance > 0f &&
        Random.value < source.Attack.StunChance)
        {
            if (target.Attack is AttackComponent tgtAttack)
                tgtAttack.StunFor(source.Attack.StunDuration);
        }

        // 3) Лечение (только враги)
        if (source.Attack.HealChance > 0f &&
            Random.value < source.Attack.HealChance)
        {
            source.Health.Heal(source.Attack.HealAmount);
        }
    }
}
