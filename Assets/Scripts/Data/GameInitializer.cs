using System.Collections;
using UnityEngine;

/// <summary>
/// �������������� ������ � ������ �� �����: ������� �������,
/// ����������� ������ �� ScriptableObject
/// � ������������� �� ������� ������ ��� �������� ������.
/// ��������� � ������� GameObject, �������� "SceneController".
/// </summary>
public class GameInitializer : MonoBehaviour
{
    [Header("Data Objects (ScriptableObjects)")]
    [Tooltip("������ ��������� ������")]
    public CharacterData playerData;
    [Tooltip("������ ����� (������ ����) ��� ���� ������)")]
    public CharacterData enemyData;

    [Header("Abilities (ScriptableObjects)")]
    [Tooltip("������ ����������� ������ (����)")]
    public AbilityData strikeAbilityData;
    [Tooltip("������ ����������� ������ (����)")]
    public AbilityData buffAbilityData;

    [Header("Spawn Points & Prefabs")]
    [Tooltip("����� ������ ��� ������ (��������, PlayerRoot)")]
    public Transform playerSpawnPoint;
    [Tooltip("������ ������, ������ ��������� ����������: HealthComponent, ManaComponent, AttackComponent, AbilityComponent x2, CharacterView, PlayerController")]
    public GameObject playerPrefab;

    [Tooltip("����� ������ ��� ������ (Place 3 Transforms on the right side)")]
    public Transform[] enemySpawnPoints;
    [Tooltip("������ �����, ������ ��������� ����������: HealthComponent, AttackComponent, CharacterView, EnemyAI, SelectableComponent")]
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

        // ����� ������ � ��������� �����
        _playerInstance = Instantiate(
            playerPrefab,
            playerSpawnPoint.position,
            playerSpawnPoint.rotation
        );

        // ������������� ��������
        var health = _playerInstance.GetComponent<HealthComponent>();
        health.Initialize(playerData.maxHealth);

        // ������������� �����
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

        // ������������� ������
        var abilities = _playerInstance.GetComponents<AbilityComponent>();
        if (abilities.Length >= 2)
        {
            abilities[0].Initialize(strikeAbilityData);
            abilities[1].Initialize(buffAbilityData);
        }
        else
        {
            Debug.LogWarning("�� PlayerPrefab ������ ���� ��� AbilityComponent ��� ���� ������������!");
        }

        // ������������� ����������� ������
        var controller = _playerInstance.GetComponent<PlayerController>();
        controller.Initialize();
        // ������������ � ���������� ������
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

        // ������������� ��������
        var health = enemy.GetComponent<HealthComponent>();
        health.Initialize(enemyData.maxHealth);

        // ������������� �����
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

        // �������� �� ������ ��� ��������
        health.OnDeath += () =>
        {
            Destroy(enemy);
            StartCoroutine(RespawnEnemy(spawnPoint));
        };

        // ������������� AI
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
