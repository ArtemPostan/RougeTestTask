// Assets/Scripts/Domain/Character.cs
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(AttackComponent))]
[RequireComponent(typeof(BuffComponent))]
public class Character : MonoBehaviour, ICharacter
{
    public HealthComponent Health { get; private set; }
    public AttackComponent Attack { get; private set; }
    public BuffComponent Buffs { get; private set; }

    private void Awake()
    {
        Health = GetComponent<HealthComponent>();
        Attack = GetComponent<AttackComponent>();
        Buffs = GetComponent<BuffComponent>();
    }
}
