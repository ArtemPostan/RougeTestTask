using System.Collections;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance { get; private set; }

    [Tooltip("Задержка до применения урона (секунд), должна совпадать с моментом удара в анимации")]
    public float hitDelay = 0.3f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
    }

    public void RegisterAttack(AttackComponent attack)
    {
        attack.OnAttackAttempt += (src, tgt) => StartCoroutine(DelayedHandleAttack(src, tgt));
    }

    private IEnumerator DelayedHandleAttack(ICharacter source, ICharacter target)
    {
        // запускаем анимацию атаки сразу (AnimationController уже слушает OnAttackAttempt)
        // но ждем, пока «дойдёт» удар
        yield return new WaitForSeconds(hitDelay);

        // 1) ВЫЧИСЛЯЕМ УРОН
        int baseDmg = source.Attack.BaseDamage;
        float bonusPct = source.Buffs.DamageBonusPercent;
        int dmg = Mathf.RoundToInt(baseDmg * (1 + bonusPct));

        // 2) НАНОСИМ УРОН
        target.Health.TakeDamage(dmg);

        // 3) ШАНС ОГЛУШИТЬ (только игрок)
        if (source == GameManager.Instance.Player &&
            Random.value < source.Attack.StunChance)
        {
            var tgtAttack = (AttackComponent)target.Attack;
            //tgtAttack.StunFor(source.Attack.StunDuration);
        }

        // 4) ШАНС ЛЕЧЕНИЯ (для врагов)
        if (source.Attack.HealChance > 0f &&
            Random.value < source.Attack.HealChance)
        {
            source.Health.Heal(source.Attack.HealAmount);
        }
    }
}
