using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TextType
{
    Damage,
    BonusHealing,
    BonusSpeed,
    DamageToPlayer,
    DamageToEnemy,
    Combo,
    NoMoney,
    EnergyShield,
}
public class UniversalText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public TextType textType;
    private string currentText;
    private float currentNumber;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        ResetEffects();
    }

    public void Initialize(TextType textType, float number, string newtext)
    {
        this.textType = textType;

        currentNumber = number;
        currentText = newtext;

        textMeshPro.SetText(currentText);
        SetNewText(textType);

    }

    public void SetNewText(TextType textType)
    {
        switch (textType)
        {
            case TextType.Damage:
                textMeshPro.text = $"+{currentNumber}";
                textMeshPro.color = Color.white;
                //currentText = new ScoreText();
                break;
            case TextType.BonusHealing:
                textMeshPro.text = $"{currentText} + {currentNumber}" ;
                textMeshPro.color = Color.green;
                break;
            case TextType.BonusSpeed:
                textMeshPro.text = $"{currentText} {currentNumber}x";
                textMeshPro.color = Color.yellow;
                break;
            case TextType.Combo:
                textMeshPro.text = $"{currentText} {currentNumber}";
                textMeshPro.color = Color.magenta;
                break;
            case TextType.NoMoney:
                textMeshPro.text = $"{currentText} {currentNumber}";
                textMeshPro.color = Color.white;
                break;
            case TextType.EnergyShield:
                textMeshPro.text = $"{currentText} {currentNumber}";
                textMeshPro.color = Color.blue;
                break;

        }
    }

    public void ResetEffects()
    {
        //currentBonus = null;
        //spriteRenderer.sprite = sprites[0];
    }

    public void ReturnText()
    {
        ScorePool.Instance.ReturnText(this);
    }







}
