using System;
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
        _attack.OnAttackAttempt += OnAttack;      
        _health.OnHealthChanged += OnHealthChanged;
        _health.OnDeath += OnDeath;
        _attack.OnStunned += OnStunned;
       
    }

    private void OnDisable()
    {
        _attack.OnAttackAttempt -= OnAttack;
        _health.OnHealthChanged -= OnHealthChanged;
        _health.OnDeath -= OnDeath;
        _attack.OnStunned -= OnStunned;       
    }

    private void OnAttack(ICharacter source, ICharacter target)
    {
        
        if (source == _character)
        {
            _spine.attack_1();            
        }
    }

    private void OnHealthChanged(int current, int max)
    {        
        if (current < max)
        {           
            _spine.getHit();
        }
    }

    private void OnDeath()
    {
        _spine.death();
    }
    
    private void OnStunned(float duration)
    {
        _spine.stun();
    }

    public void PlayIdle()
    {
        _spine.idle();
    }

}
