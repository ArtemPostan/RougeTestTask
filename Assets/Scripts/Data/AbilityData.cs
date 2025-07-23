using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Data/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public int manaCost;
    public float cooldown;           // ���.
    public float damageBonusPercent; // 1.0 == +100%
    public float attackSpeedModifier; // 0.6 == 60% ��������
    public float duration;           // ������������ �����, ���.
}
