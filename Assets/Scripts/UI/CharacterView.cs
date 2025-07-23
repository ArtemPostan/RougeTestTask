using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(HealthComponent))]
public class CharacterView : MonoBehaviour
{
    [Header("Data Components")]
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private ManaComponent _manaComponent; // может быть null для врагов

    [Header("UI Elements")]
    [SerializeField] private Image _healthFill;     // Image с типом Fill → Fill Amount
    [SerializeField] private TMP_Text _healthText;  // опционально, для текста "HP 50/100"
    [SerializeField] private Image _manaFill;       // аналогично для маны
    [SerializeField] private TMP_Text _manaText;    // опционально

    private void Reset()
    {
        // Автоматически подцепляем ссылки на компоненты, если забыли в инспекторе
        _healthComponent = GetComponent<HealthComponent>();
        _manaComponent = GetComponent<ManaComponent>();
    }

    private void OnEnable()
    {
        // Подписываемся на события
        _healthComponent.OnHealthChanged += HandleHealthChanged;
        _healthComponent.OnDeath += HandleDeath;

        if (_manaComponent != null)
            _manaComponent.OnManaChanged += HandleManaChanged;
    }

    private void OnDisable()
    {
        // Отписываемся, чтобы не было утечек
        _healthComponent.OnHealthChanged -= HandleHealthChanged;
        _healthComponent.OnDeath -= HandleDeath;

        if (_manaComponent != null)
            _manaComponent.OnManaChanged -= HandleManaChanged;
    }

    private void Start()
    {
        // Инициализируем UI текущими значениями
        HandleHealthChanged(_healthComponent.CurrentHealth, _healthComponent.MaxHealth);
        if (_manaComponent != null)
            HandleManaChanged(_manaComponent.CurrentMana, _manaComponent.MaxMana);
    }

    private void HandleHealthChanged(int current, int max)
    {
        if (_healthFill != null)
            _healthFill.fillAmount = (float)current / max;

        if (_healthText != null)
            _healthText.text = $"{current}/{max}";
    }

    private void HandleManaChanged(int current, int max)
    {
        if (_manaFill != null)
            _manaFill.fillAmount = (float)current / max;

        if (_manaText != null)
            _manaText.text = $"{current}/{max}";
    }

    private void HandleDeath()
    {
        // Можно тут запустить анимацию смерти, скрыть UI и т.п.
        gameObject.SetActive(false);
    }
}
