using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(HealthComponent))]
public class CharacterView : MonoBehaviour
{
    [Header("Data Components")]
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private ManaComponent _manaComponent; 

    private CharacterAnimationController _characterAnimationController;

    [Header("UI Elements")]
    [SerializeField] private Image _healthFill;   
    [SerializeField] private TMP_Text _healthText; 
    [SerializeField] private Image _manaFill;      
    [SerializeField] private TMP_Text _manaText;   

    private void Reset()
    {        
        _healthComponent = GetComponent<HealthComponent>();
        _manaComponent = GetComponent<ManaComponent>();
    }

    private void Awake()
    {
        _characterAnimationController = GetComponentInChildren<CharacterAnimationController>();
    }
    private void OnEnable()
    {        
        _healthComponent.OnHealthChanged += HandleHealthChanged;
        _healthComponent.OnDeath += HandleDeath;


        if (_manaComponent != null)
            _manaComponent.OnManaChanged += HandleManaChanged;
        
    }

    private void OnDisable()
    {       
        _healthComponent.OnHealthChanged -= HandleHealthChanged;
        _healthComponent.OnDeath -= HandleDeath;

        if (_manaComponent != null)
            _manaComponent.OnManaChanged -= HandleManaChanged;
    }

    private void Start()
    {       
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
        StartCoroutine(Waiter());
             
    }

    private IEnumerator Waiter()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
