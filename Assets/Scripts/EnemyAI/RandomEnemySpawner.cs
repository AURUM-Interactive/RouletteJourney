using UnityEngine;
using System.Collections.Generic;

public class RandomEnemySpawner : MonoBehaviour
{
    // Serializable class for a prefab and its spawn chance
    [System.Serializable]
    public class EnemySpawnOption
    {
        public GameObject prefab; // Enemy prefab to spawn
        [Range(0f, 1f)] public float spawnChance = 1f; // Chance this prefab is selected
    }

    [Tooltip("Enemy prefabs with individual spawn chances")]
    public EnemySpawnOption[] enemyOptions; // Array of possible enemies to spawn

    void Start()
    {
        // Start by spawning an enemy
        SpawnRandomEnemy(); 
    }

    void SpawnRandomEnemy()
    {
        // Prefabs that passed their spawn chance
        List<GameObject> validOptions = new List<GameObject>();

        // Get a random value between 0 and 1
        float randomValue = Random.value;

        // Check each option against its spawn chance
        foreach (var option in enemyOptions)
        {
            if (randomValue <= option.spawnChance)
            {
                validOptions.Add(option.prefab); // Add to valid list
            }
        }

        // If none passed, choose a fallback at random
        if (validOptions.Count == 0 && enemyOptions.Length > 0)
        {
            int fallbackIndex = Random.Range(0, enemyOptions.Length);
            validOptions.Add(enemyOptions[fallbackIndex].prefab);
        }

        // Pick one from the valid options and spawn it
        GameObject selected = validOptions[Random.Range(0, validOptions.Count)];
        Instantiate(selected, transform.position, transform.rotation); // Spawn at current position
        Destroy(gameObject); // Remove the spawner
    }
}
