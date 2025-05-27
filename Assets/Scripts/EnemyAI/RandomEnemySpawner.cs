using UnityEngine;
using System.Collections.Generic;

public class RandomEnemySpawner : MonoBehaviour
{
    // Serializable class for a prefab and its spawn ratio
    [System.Serializable]
    public class EnemySpawnOption
    {
        public GameObject prefab; // Enemy prefab to spawn
        public int spawnRatio = 1; // Ratio weight for this prefab
    }

    public EnemySpawnOption[] enemyOptions; // Array of possible enemies to spawn

    void Start()
    {
        SpawnRandomEnemy();
    }

    void SpawnRandomEnemy()
    {
        // Calculate total ratio sum
        int totalRatio = 0;
        foreach (var option in enemyOptions)
        {
            if (option.prefab != null && option.spawnRatio > 0)
            {
                totalRatio += option.spawnRatio;
            }
        }

        // Roll a random number from 0 to totalRatio-1
        int randomRoll = Random.Range(0, totalRatio);

        // Find which enemy corresponds to this roll
        int currentSum = 0;
        GameObject selectedPrefab = null;

        foreach (var option in enemyOptions)
        {
            if (option.prefab != null && option.spawnRatio > 0)
            {
                currentSum += option.spawnRatio;

                // If our roll falls within this range, select this enemy
                if (randomRoll < currentSum)
                {
                    selectedPrefab = option.prefab;
                    break;
                }
            }
        }

        // Spawn the selected enemy
        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, transform.position, transform.rotation);
            Debug.Log($"Spawned: {selectedPrefab.name} (Roll: {randomRoll}/{totalRatio})");
        }

        // Remove the spawner
        Destroy(gameObject);
    }
}