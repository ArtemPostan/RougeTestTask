using System.Collections;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance { get; private set; }

    [Tooltip("�������� �� ���������� ����� (������), ������ ��������� � �������� ����� � ��������")]
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
        // ��������� �������� ����� ����� (AnimationController ��� ������� OnAttackAttempt)
        // �� ����, ���� ������ ����
        yield return new WaitForSeconds(hitDelay);

        // 1) ��������� ����
        int baseDmg = source.Attack.BaseDamage;
        float bonusPct = source.Buffs.DamageBonusPercent;
        int dmg = Mathf.RoundToInt(baseDmg * (1 + bonusPct));

        // 2) ������� ����
        target.Health.TakeDamage(dmg);

        // 3) ���� �������� (������ �����)
        if (source == GameManager.Instance.Player &&
            Random.value < source.Attack.StunChance)
        {
            var tgtAttack = (AttackComponent)target.Attack;
            //tgtAttack.StunFor(source.Attack.StunDuration);
        }

        // 4) ���� ������� (��� ������)
        if (source.Attack.HealChance > 0f &&
            Random.value < source.Attack.HealChance)
        {
            source.Health.Heal(source.Attack.HealAmount);
        }
    }
}
