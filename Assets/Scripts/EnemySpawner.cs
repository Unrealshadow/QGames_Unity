using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enemy SpawnPoint
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")] public List<Enemy> enemyPrefabs;

    [Header("Parameters")] public float m_spawn_interval_min = 6;
    public float m_spawn_interval_max = 8;

    public float minimumSpawnInterval = 2; // Minimum spawn interval

    private int lastScoreCheck = 0;
    private int scoreInterval = 100; // Score interval for decreasing spawn rate


    //------------------------------------------------------------------------------
    public void StartRunning()
    {
        StartCoroutine(MainCoroutine());
    }

    private IEnumerator MainCoroutine()
    {
        while (true)
        {
            AdjustSpawnRateBasedOnScore();

            yield return new WaitForSeconds(Random.Range(m_spawn_interval_min, m_spawn_interval_max));


            // Randomly select an enemy to spawn from the list
            if (enemyPrefabs != null && enemyPrefabs.Count > 0)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Count);
                Enemy selectedEnemyPrefab = enemyPrefabs[randomIndex];

                if (selectedEnemyPrefab != null)
                {
                    Enemy enemy = Instantiate(selectedEnemyPrefab, transform.parent);
                    enemy.transform.position = new Vector3(
                        CameraDimensions.Instance.GetRandomTopPosition().x,
                        CameraDimensions.Instance.GetRandomTopPosition().y + 1,
                        0);
                }
            }
        }
    }

    private void AdjustSpawnRateBasedOnScore()
    {
        int currentScore = StageLoop.Instance.GetCurrentScore(); // Get the current score

        if (currentScore - lastScoreCheck >= scoreInterval)
        {
            lastScoreCheck = currentScore;

            m_spawn_interval_min = Mathf.Max(minimumSpawnInterval, m_spawn_interval_min - 0.1f);
            m_spawn_interval_max = Mathf.Max(minimumSpawnInterval, m_spawn_interval_max - 0.1f);
        }
    }

    //------------------------------------------------------------------------------

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}