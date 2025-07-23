using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Data/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public float attackInterval;      // ���.
    public int baseDamage;
    public float stunChance;         // 0�1
    public float stunDuration;       // ���.
    public float healChance;         // ��� ������, 0�1
    public int healAmount;
}
