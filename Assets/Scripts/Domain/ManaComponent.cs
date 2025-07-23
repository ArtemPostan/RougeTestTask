using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaComponent : MonoBehaviour
{
    public int MaxMana = 100;
    public int CurrentMana { get; private set; }
    public float RegenPerSecond = 1f;
    public event System.Action<int, int> OnManaChanged;

    private void Start()
    {
        CurrentMana = MaxMana;
        StartCoroutine(RegenLoop());
    }
    private IEnumerator RegenLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CurrentMana = Mathf.Min(CurrentMana + Mathf.RoundToInt(RegenPerSecond), MaxMana);
            OnManaChanged?.Invoke(CurrentMana, MaxMana);
        }
    }
    public bool TrySpend(int amount)
    {
        if (CurrentMana < amount) return false;
        CurrentMana -= amount;
        OnManaChanged?.Invoke(CurrentMana, MaxMana);
        return true;
    }
}
