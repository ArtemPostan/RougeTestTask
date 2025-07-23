using UnityEngine;

/// <summary>
/// Универсальный «мост» между логикой боевых событий и визуальными анимациями персонажей.
/// Подписывается на события AttackComponent, HealthComponent и др. и запускает соответствующие Spine-анимации.
/// Может использоваться как для игрока, так и для врагов.
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
        // Автоатаки и навыки
        _attack.OnAttackAttempt += OnAttack;
        // Реакция на получение урона
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
        // Проверяем, инициатор атаки — этот персонаж
        if (source == _character)
        {
            _spine.attack_1();
            //// Запустить одну из анимаций атаки
            //if (Random.value < 0.5f) _spine.attack_1();
            //else _spine.attack_2();
        }
    }

    private void OnHealthChanged(int current, int max)
    {
        // Попытка получить имя анимации попадания
        if (current < max)
        {
            // проигрываем хит-анимацию один раз
            _spine.getHit();
        }
    }

    private void OnDeath()
    {
        // проигрываем анимацию смерти
        _spine.death();
    }
}
