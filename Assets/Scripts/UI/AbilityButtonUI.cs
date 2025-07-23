using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AbilityButtonUI : MonoBehaviour
{
    [Tooltip("Индекс способности (0 — Strike, 1 — Buff)")]
    public int abilityIndex;
 
    public Image cooldownOverlay;

    private PlayerController _player;
    private AbilityComponent _ability;
    private Button _button;
    private Coroutine _cdRoutine;

    private void Awake()
    {        
        GameManager.Instance.OnGameReady += Init;       
        if (GameManager.Instance.Player != null)
            Init();
    }

    private void Init()
    {     
        GameManager.Instance.OnGameReady -= Init;

        _player = GameManager.Instance.Player;
        _button = GetComponent<Button>();
       
        var abilities = _player.GetComponents<AbilityComponent>();
        if (abilityIndex < 0 || abilityIndex >= abilities.Length)
        {
            Debug.LogError($"[{name}] Неверный abilityIndex={abilityIndex}");
            enabled = false;
            return;
        }
        _ability = abilities[abilityIndex];
       
        cooldownOverlay.fillAmount = 0f;
        _button.onClick.AddListener(() => _player.UseAbility(abilityIndex));
        _ability.OnCooldownStarted += OnCooldownStarted;
        _ability.OnCooldownEnded += OnCooldownEnded;
    }

    private void OnDestroy()
    {
        if (_ability != null)
        {
            _ability.OnCooldownStarted -= OnCooldownStarted;
            _ability.OnCooldownEnded -= OnCooldownEnded;
        }
    }

    private void OnCooldownStarted(AbilityComponent _)
    {
        if (_cdRoutine != null) StopCoroutine(_cdRoutine);
        _cdRoutine = StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        float cd = _ability.Data.cooldown;
        float t = cd;
        while (t > 0f)
        {
            cooldownOverlay.fillAmount = t / cd;
            t -= Time.deltaTime;
            yield return null;
        }
        cooldownOverlay.fillAmount = 0f;
    }

    private void OnCooldownEnded(AbilityComponent _)
    {
        if (_cdRoutine != null) StopCoroutine(_cdRoutine);
        cooldownOverlay.fillAmount = 0f;
    }
}
