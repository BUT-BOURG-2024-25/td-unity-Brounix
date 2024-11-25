using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private GameObject[] potionPrefabs;
    [SerializeField]
    private float spawnRadius = 10f;
    [SerializeField]
    private float initialSpawnInterval = 2f;
    [SerializeField]
    private int initialWaveCount = 10;
    [SerializeField]
    private int waveIncrement = 5;

    [SerializeField]
    private Text waveText;
    [SerializeField]
    private Text enemiesLeftText;

    private float spawnInterval;
    private int waveCount = 0;
    private int objectsSpawned = 0;
    private int currentWaveObjects;
    private float spawnCooldown = 0f;
    private bool waveInProgress = false;

    [SerializeField]
    private float[] spawnChances;
    [SerializeField]
    private float potionSpawnChance = 0.2f;
    private float totalChance;

    void Start()
    {
        if (enemyPrefabs.Length == 0 || spawnChances.Length == 0 || enemyPrefabs.Length != spawnChances.Length)
        {
            Debug.LogError("Enemy prefabs and spawn chances must be assigned and have the same length!");
            return;
        }

        totalChance = 0f;
        foreach (float chance in spawnChances)
        {
            totalChance += chance;
        }

        spawnInterval = initialSpawnInterval;
        currentWaveObjects = initialWaveCount;
        UpdateWaveUI();
        UpdateEnemiesLeftUI();
    }

    void Update()
    {
        if (!waveInProgress)
        {
            StartWave();
        }

        if (waveInProgress)
        {
            spawnCooldown -= Time.deltaTime;

            if (spawnCooldown <= 0f && objectsSpawned < currentWaveObjects)
            {
                SpawnObject();
                spawnCooldown = spawnInterval;
            }

            if (objectsSpawned >= currentWaveObjects)
            {
                waveInProgress = false;
                Debug.Log("Wave " + waveCount + " completed!");
            }

            UpdateEnemiesLeftUI();
        }
    }

    private void StartWave()
    {
        waveInProgress = true;
        waveCount++;
        objectsSpawned = 0;
        currentWaveObjects = initialWaveCount + waveCount * waveIncrement;
        spawnInterval = Mathf.Max(spawnInterval - 0.2f, 0.5f);

        UpdateWaveUI();
        spawnCooldown = spawnInterval;
    }

    private void SpawnObject()
    {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = transform.position.y;

        if (Random.Range(0f, 1f) <= potionSpawnChance)
        {
            SpawnPotion(randomPosition);
        }
        else
        {
            SpawnEnemy(randomPosition);
        }
    }

    private void SpawnPotion(Vector3 position)
    {
        if (potionPrefabs.Length > 0)
        {
            int potionIndex = Random.Range(0, potionPrefabs.Length);
            Instantiate(potionPrefabs[potionIndex], position, Quaternion.identity);
            Debug.Log("Potion spawned at position: " + position);
        }
    }

    private void SpawnEnemy(Vector3 position)
    {
        GameObject enemyToSpawn = ChooseEnemy();
        Instantiate(enemyToSpawn, position, Quaternion.identity);
        objectsSpawned++;
        Debug.Log("Enemy spawned at position: " + position);
    }

    private GameObject ChooseEnemy()
    {
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            cumulativeChance += spawnChances[i];
            if (randomValue <= cumulativeChance)
            {
                return enemyPrefabs[i];
            }
        }

        return enemyPrefabs[0];
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + waveCount.ToString();
        }
    }

    private void UpdateEnemiesLeftUI()
    {
        if (enemiesLeftText != null)
        {
            int enemiesLeft = currentWaveObjects - objectsSpawned;
            enemiesLeftText.text = "Enemies Left: " + enemiesLeft.ToString();
        }
    }
}
