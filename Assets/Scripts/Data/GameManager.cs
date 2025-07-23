using System;
using UnityEngine;

/// <summary>
/// Singleton‑фасад для всего «контекста» игры:
/// даёт доступ к игроку, камерам, системам и т.п.
/// Будет существовать сквозь все сцены (DontDestroyOnLoad).
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /// <summary>Ссылка на PlayerController заспавненного игрока.</summary>
    public PlayerController Player { get; private set; }

    /// <summary>Событие, когда вся сцена инициализирована и Player готов.</summary>
    public event Action OnGameReady;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Вызывается после спавна игрока в GameInitializer.
    /// </summary>
    public void RegisterPlayer(PlayerController player)
    {
        Player = player;
        OnGameReady?.Invoke();
    }
}
