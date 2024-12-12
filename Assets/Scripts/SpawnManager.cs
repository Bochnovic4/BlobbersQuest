using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float minimumSpacing = 5f;

    [SerializeField] private List<BoxCollider> spawnAreas;

    private List<Vector3> spawnPositions = new List<Vector3>();

    private void Start()
    {
        SpawnEnemies();
        enemyPrefab.SetActive(true);
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < spawnAreas.Count; i++)
        {
            Bounds bounds = spawnAreas[i].bounds;

            for (int j = 0; j < enemyCount; j++)
            {
                Vector3 spawnPosition = GenerateValidPosition(bounds);

                if (spawnPosition != Vector3.negativeInfinity)
                {
                    spawnPositions.Add(spawnPosition);
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("Failed to find a valid position for enemy spawn.");
                }
            }
        }
    }

    private Vector3 GenerateValidPosition(Bounds bounds)
    {
        int maxAttempts = 50;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 candidatePosition = new Vector3(x, y, z);

            if (IsPositionValid(candidatePosition))
                return candidatePosition;
        }
        return Vector3.negativeInfinity;
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 existingPosition in spawnPositions)
        {
            if (Vector3.Distance(existingPosition, position) < minimumSpacing)
                return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        foreach(BoxCollider spawnArea in spawnAreas) 
        {
            if (spawnArea != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
            }

            Gizmos.color = Color.red;
            foreach (var position in spawnPositions)
            {
                Gizmos.DrawSphere(position, 0.2f);
            }
        }
    }
}
