using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(BuffComponent))]
public class AttackComponent : MonoBehaviour
{
    public event Action<ICharacter, ICharacter> OnAttackAttempt;

    public event Action<float> OnStunned;

    BuffComponent buffComponent;

    private float _attackInterval;
    private int _baseDamage;
    private float _stunChance;
    private float _stunDuration;
    private float _healChance;
    private int _healAmount;

    private bool _isPaused;
    private ICharacter _owner;
    private ICharacter _target;
    private Coroutine _routine;
    private float _speedModifier = 1f;

    public int BaseDamage => _baseDamage;
    public float StunChance => _stunChance;
    public float StunDuration => _stunDuration;
    public float HealChance => _healChance;
    public int HealAmount => _healAmount;

    private void Awake()
    {
        buffComponent = GetComponent<BuffComponent>();
    }
    public void Initialize(
        float attackInterval,
        int baseDamage,
        float stunChance,
        float stunDuration,
        float healChance,
        int healAmount
    )
    {
        _attackInterval = attackInterval;
        _baseDamage = baseDamage;
        _stunChance = stunChance;
        _stunDuration = stunDuration;
        _healChance = healChance;
        _healAmount = healAmount;
        _owner = GetComponent<ICharacter>();
        _routine = StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_attackInterval / _speedModifier);

            if (_isPaused || _target == null)
                continue;

            OnAttackAttempt?.Invoke(_owner, _target);
        }
    }

    public void SetTarget(ICharacter target)
    {
        _target = target;
    }

    public void PauseAttack() => _isPaused = true;

    public void ResumeAttack() => _isPaused = false;

    public void SetAttackSpeedModifier(float modifier)
    {
        _speedModifier = modifier;
    }
    
    public void TriggerAttack(ICharacter target)
    {
        OnAttackAttempt?.Invoke(_owner, target);
    }    
    private Coroutine _stunRoutine;

    public void StunFor(float duration)
    {
        if (_owner.Health.IsAlive)
        {
            if (_stunRoutine != null)
                StopCoroutine(_stunRoutine);

            _stunRoutine = StartCoroutine(StunCoroutine(duration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        PauseAttack();

        OnStunned?.Invoke(duration); 
        EffectsPool.Instance.PlayEffect("Stun", transform.position, 2f);
        yield return new WaitForSeconds(duration);
        ResumeAttack();
        _stunRoutine = null;
    }

    public int CalculatedDamage
    {
        get
        {
            float bonus = buffComponent.DamageBonusPercent;
            return Mathf.RoundToInt(_baseDamage * (1 + bonus));
        }
    }

    public float CalculatedAttackSpeed
    {
        get
        {            
            return (_attackInterval * _speedModifier);
        }
    }
}
