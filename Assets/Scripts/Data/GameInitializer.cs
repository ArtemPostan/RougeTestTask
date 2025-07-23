using System.Collections;
using UnityEngine;

/// <summary>
/// Инициализирует игрока и врагов на сцене: спавнит префабы,
/// присваивает данные из ScriptableObject
/// и подписывается на события смерти для респауна врагов.
/// Привязать к пустому GameObject, например "SceneController".
/// </summary>
public class GameInitializer : MonoBehaviour
{
    [Header("Data Objects (ScriptableObjects)")]
    [Tooltip("Данные персонажа игрока")]
    public CharacterData playerData;
    [Tooltip("Данные врага (одного типа) для всех врагов)")]
    public CharacterData enemyData;

    [Header("Abilities (ScriptableObjects)")]
    [Tooltip("Первая способность игрока (удар)")]
    public AbilityData strikeAbilityData;
    [Tooltip("Вторая способность игрока (бафф)")]
    public AbilityData buffAbilityData;

    [Header("Spawn Points & Prefabs")]
    [Tooltip("Точка спавна для игрока (например, PlayerRoot)")]
    public Transform playerSpawnPoint;
    [Tooltip("Префаб игрока, должен содержать компоненты: HealthComponent, ManaComponent, AttackComponent, AbilityComponent x2, CharacterView, PlayerController")]
    public GameObject playerPrefab;

    [Tooltip("Точки спавна для врагов (Place 3 Transforms on the right side)")]
    public Transform[] enemySpawnPoints;
    [Tooltip("Префаб врага, должен содержать компоненты: HealthComponent, AttackComponent, CharacterView, EnemyAI, SelectableComponent")]
    public GameObject enemyPrefab;

    private GameObject _playerInstance;

    private void Start()
    {
        InitializePlayer();
        InitializeEnemies();
    }

    private void InitializePlayer()
    {
        if (playerPrefab == null || playerData == null || playerSpawnPoint == null)
        {
            Debug.LogError("Player prefab, data or spawn point not assigned in GameInitializer");
            return;
        }

        // Спавн игрока в указанной точке
        _playerInstance = Instantiate(
            playerPrefab,
            playerSpawnPoint.position,
            playerSpawnPoint.rotation
        );

        // Инициализация здоровья
        var health = _playerInstance.GetComponent<HealthComponent>();
        health.Initialize(playerData.maxHealth);

        // Инициализация атаки
        var attack = _playerInstance.GetComponent<AttackComponent>();
        attack.Initialize(

            playerData.attackInterval,
            playerData.baseDamage,
            playerData.stunChance,
            playerData.stunDuration,
            playerData.healChance,
            playerData.healAmount
        );

        CombatSystem.Instance.RegisterAttack(attack);

        // Инициализация умений
        var abilities = _playerInstance.GetComponents<AbilityComponent>();
        if (abilities.Length >= 2)
        {
            abilities[0].Initialize(strikeAbilityData);
            abilities[1].Initialize(buffAbilityData);
        }
        else
        {
            Debug.LogWarning("На PlayerPrefab должно быть два AbilityComponent для двух способностей!");
        }

        // Инициализация контроллера игрока
        var controller = _playerInstance.GetComponent<PlayerController>();
        controller.Initialize();
        // Регистрируем в глобальном фасаде
        GameManager.Instance.RegisterPlayer(controller);
    }

    private void InitializeEnemies()
    {
        if (enemyPrefab == null || enemyData == null || enemySpawnPoints == null || enemySpawnPoints.Length == 0)
        {
            Debug.LogError("Enemy prefab, data or spawn points not assigned in GameInitializer");
            return;
        }

        foreach (var spawnPoint in enemySpawnPoints)
        {
            SpawnEnemy(spawnPoint);
        }
    }

    private void SpawnEnemy(Transform spawnPoint)
    {
        var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Инициализация здоровья
        var health = enemy.GetComponent<HealthComponent>();
        health.Initialize(enemyData.maxHealth);

        // Инициализация атаки
        var attack = enemy.GetComponent<AttackComponent>();
        attack.Initialize(
            enemyData.attackInterval,
            enemyData.baseDamage,
            enemyData.stunChance,
            enemyData.stunDuration,
            enemyData.healChance,
            enemyData.healAmount
        );
        CombatSystem.Instance.RegisterAttack(attack);

        // Подписка на смерть для респауна
        health.OnDeath += () =>
        {
            Destroy(enemy);
            StartCoroutine(RespawnEnemy(spawnPoint));
        };

        // Инициализация AI
        var ai = enemy.GetComponent<EnemyAI>();
        if (ai != null && _playerInstance != null)
            ai.Initialize(_playerInstance.GetComponent<ICharacter>());
    }

    private IEnumerator RespawnEnemy(Transform spawnPoint)
    {
        yield return new WaitForSeconds(2f);
        SpawnEnemy(spawnPoint);
    }
}
