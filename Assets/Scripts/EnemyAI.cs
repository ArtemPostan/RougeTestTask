using UnityEngine;

[RequireComponent(typeof(AttackComponent))]
public class EnemyAI : MonoBehaviour
{
    private AttackComponent _attack;

    public void Initialize(ICharacter player)
    {
        _attack = GetComponent<AttackComponent>();
        _attack.SetTarget(player);
    }
}