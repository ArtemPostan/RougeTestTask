using UnityEngine;

/// <summary>
/// ������������� ����� ����� ������� ������ ������� � ����������� ���������� ����������.
/// ������������� �� ������� AttackComponent, HealthComponent � ��. � ��������� ��������������� Spine-��������.
/// ����� �������������� ��� ��� ������, ��� � ��� ������.
/// </summary>
[RequireComponent(typeof(Character))]
public class CharacterAnimationController : MonoBehaviour
{
    private Character _character;
    private AttackComponent _attack;
    private HealthComponent _health;
    private KnightControl _spine;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _attack = GetComponent<AttackComponent>();
        _health = GetComponent<HealthComponent>();
        _spine = GetComponentInChildren<KnightControl>();
    }

    private void OnEnable()
    {
        // ��������� � ������
        _attack.OnAttackAttempt += OnAttack;
        // ������� �� ��������� �����
        _health.OnHealthChanged += OnHealthChanged;
        _health.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        _attack.OnAttackAttempt -= OnAttack;
        _health.OnHealthChanged -= OnHealthChanged;
        _health.OnDeath -= OnDeath;
    }

    private void OnAttack(ICharacter source, ICharacter target)
    {
        // ���������, ��������� ����� � ���� ��������
        if (source == _character)
        {
            _spine.attack_1();
            //// ��������� ���� �� �������� �����
            //if (Random.value < 0.5f) _spine.attack_1();
            //else _spine.attack_2();
        }
    }

    private void OnHealthChanged(int current, int max)
    {
        // ������� �������� ��� �������� ���������
        if (current < max)
        {
            // ����������� ���-�������� ���� ���
            _spine.getHit();
        }
    }

    private void OnDeath()
    {
        // ����������� �������� ������
        _spine.death();
    }
}
