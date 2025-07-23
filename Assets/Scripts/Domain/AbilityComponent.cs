using System;
using System.Collections;
using UnityEngine;
using static AbilityData;

[RequireComponent(typeof(ManaComponent))]
public class AbilityComponent : MonoBehaviour
{
    public event Action<AbilityComponent> OnCooldownStarted;
    public event Action<AbilityComponent> OnCooldownEnded;

    private AbilityData _data;
    private ManaComponent _mana;
    private float _cdRemaining;
    private Coroutine _cdRoutine;

    public AbilityData Data => _data;
    public void Initialize(AbilityData data)
    {
        _data = data;
        _mana = GetComponent<ManaComponent>();
    }

    public void Use(ICharacter target)
    {       
        if (_cdRemaining > 0f || !_mana.TrySpend(_data.manaCost)) return;

        switch (_data.abilityType)
        {
            case AbilityType.Buff:
                GetComponent<BuffComponent>().Apply(_data);
                break;

            case AbilityType.Attack:
                if (target != null)
                    GetComponent<AttackComponent>().TriggerAttack(target);
                break;

            case AbilityType.Hybrid:
                if (target != null)
                    GetComponent<AttackComponent>().TriggerAttack(target);
                GetComponent<BuffComponent>().Apply(_data);
                break;

            case AbilityType.Heal:
                GetComponent<HealthComponent>().Heal(_data.healAmount);
                break;
        }

        StartCooldown();
    }

    private void StartCooldown()
    {
        if (_cdRoutine != null) StopCoroutine(_cdRoutine);
        _cdRoutine = StartCoroutine(Cooldown());
        OnCooldownStarted?.Invoke(this);
    }

    private IEnumerator Cooldown()
    {
        _cdRemaining = _data.cooldown;
        while (_cdRemaining > 0f)
        {
            yield return new WaitForSeconds(0.1f);
            _cdRemaining -= 0.1f;
        }
        _cdRemaining = 0f;
        OnCooldownEnded?.Invoke(this);
    }

    public float CooldownRemaining => _cdRemaining;
}