using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Data/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public float attackInterval;      // сек.
    public int baseDamage;
    public float stunChance;         // 0Ц1
    public float stunDuration;       // сек.
    public float healChance;         // дл€ врагов, 0Ц1
    public int healAmount;
}
