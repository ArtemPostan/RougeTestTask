using UnityEngine;
using TMPro; 

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;

    [SerializeField] private TextMeshProUGUI attackSpeedText;

    private void Update()
    {
        if (GameManager.Instance.Player?.Attack != null)
        {
            var damage = GameManager.Instance.Player.Attack.CalculatedDamage;
            damageText.text = $"Damage: {damage}";

            var attackSpeed = GameManager.Instance.Player.Attack.CalculatedAttackSpeed;
            attackSpeedText.text = $"Attack Speed: {attackSpeed}";
        }
    }
}