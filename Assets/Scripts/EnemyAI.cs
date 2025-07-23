using UnityEngine;

[RequireComponent(typeof(AttackComponent))]
public class EnemyAI : MonoBehaviour
{
    private AttackComponent _attack;
    private CharacterAnimationController _anim;
    private ICharacter _player;

    public void Initialize(ICharacter player)
    {
        _attack = GetComponent<AttackComponent>();
        _anim = GetComponent<CharacterAnimationController>();
        _player = player;

        _attack.SetTarget(player);

        // Подписываемся на смерть игрока
        _player.Health.OnDeath += OnPlayerDeath;
    }
    private void OnPlayerDeath()
    {
        // Останавливаем атаку
        _attack.PauseAttack();

        // Переключаемся в idle-анимацию
        if (_anim != null)
            _anim.PlayIdle();
    }

    private void OnDisable()
    {
        // На всякий случай отписываемся
        if (_player != null)
            _player.Health.OnDeath -= OnPlayerDeath;
    }
}