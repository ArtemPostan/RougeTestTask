using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Data/AbilityData")]
public class AbilityData : ScriptableObject
{
    public enum AbilityType { Attack, Buff, Hybrid, Heal }
    public AbilityType abilityType;

    public string abilityName;
    public int manaCost;
    public float cooldown;        
    public float damageBonusPercent;
    public float attackSpeedModifier; 
    public float duration;
    public int healAmount;
}
