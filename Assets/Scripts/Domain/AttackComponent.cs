using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BuffComponent))]
public class AttackComponent : MonoBehaviour
{
    public event Action<ICharacter, ICharacter> OnAttackAttempt;

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
            if (_target != null)
            {
                if (_isPaused)
                {
                    Debug.Log($"[{name}] ATTACK PAUSED against {_target as MonoBehaviour}");
                }
                else
                {
                    Debug.Log($"[{name}] ATTACK {_target as MonoBehaviour}");
                    OnAttackAttempt?.Invoke(_owner, _target);
                }
            }
            else
            {
                Debug.Log($"[{name}] no target set");
            }
            yield return new WaitForSeconds(_attackInterval / _speedModifier);
        }
    }

    public void SetTarget(ICharacter target)
    {
        _target = target;
    }

    public void PauseAttack()
    {
        _isPaused = true;
    }

    public void ResumeAttack()
    {
        Debug.Log($"[{name}] ResumeAttack()");
        _isPaused = false;
    }

    public void SetAttackSpeedModifier(float modifier)
    {
        _speedModifier = modifier;
    }

    /// <summary>
    /// ѕоднимает событие атаки извне (дл€ навыков)
    /// </summary>
    public void TriggerAttack(ICharacter target)
    {
        OnAttackAttempt?.Invoke(_owner, target);
    }
}