using System.Collections;
using UnityEngine;


public class BuffComponent : MonoBehaviour
{
    private AttackComponent _attack;

    private float _damageBonusPercent;
    public float DamageBonusPercent => _damageBonusPercent;

    private void Awake()
    {
        _attack = GetComponent<AttackComponent>();
    }
    

    public void Apply(AbilityData data)
    {
        StartCoroutine(BuffRoutine(data));
    }

    private IEnumerator BuffRoutine(AbilityData data)
    {
        _attack.SetAttackSpeedModifier(data.attackSpeedModifier);
        _damageBonusPercent += data.damageBonusPercent;

        yield return new WaitForSeconds(data.duration);

        _attack.SetAttackSpeedModifier(1f);
        _damageBonusPercent -= data.damageBonusPercent;
    }
}
