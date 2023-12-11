using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enemy SpawnPoint
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")] 
    public List<Enemy> enemyPrefabs; // List to hold different enemy prefabs

    [Header("Parameters")] 
    public float m_spawn_interval_min = 2;
    public float m_spawn_interval_max = 4;

    //------------------------------------------------------------------------------

    public void StartRunning()
    {
        StartCoroutine(MainCoroutine());
    }

    private IEnumerator MainCoroutine()
    {
        while (true)
        {
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


    //------------------------------------------------------------------------------

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}