using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveRoom : MonoBehaviour
{
    [Header("Doors")]
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    [Header("Spawning")]
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesToSpawn = 5;

    [Header("Timing")]
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float minSpawnInterval = 2.5f;
    [SerializeField] private float maxSpawnInterval = 3.5f;

    [Header("Dependency Injection")]
    [SerializeField] private CharacterSwap characterSwap;

    private int enemiesAlive = 0;
    private bool roomActive = false;
    private bool finishedSpawning = false;

    public void StartRoom()
    {
        if (roomActive) return;

        roomActive = true;
        CloseDoors();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemiesOverTime());
    }

    private IEnumerator SpawnEnemiesOverTime()
    {
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Count)];

            GameObject enemy = Instantiate(enemyPrefab, spawn.position, Quaternion.identity);

            HomingEnemy homingEnemy = enemy.GetComponent<HomingEnemy>();
            BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
            if (baseEnemy != null)
                baseEnemy.SetCharacterSwap(characterSwap);
            if (homingEnemy != null)
                homingEnemy.SetDetectionRange(5000);

            enemiesAlive++;

            Health h = enemy.GetComponent<Health>();
            if (h != null)
            {
                h.OnDeath.AddListener(OnEnemyDied);
            }

            float delay = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(delay);
        }

        finishedSpawning = true;

        CheckRoomComplete();
    }

    private void OnEnemyDied()
    {
        enemiesAlive--;
        CheckRoomComplete();
    }

    private void CheckRoomComplete()
    {
        if (finishedSpawning && enemiesAlive <= 0)
        {
            EndRoom();
        }
    }

    private void CloseDoors()
    {
        leftDoor.SetActive(true);
        rightDoor.SetActive(true);
    }

    private void EndRoom()
    {
        OpenDoors();
    }

    private void OpenDoors()
    {
        leftDoor.SetActive(false);
        rightDoor.SetActive(false);
    }
}