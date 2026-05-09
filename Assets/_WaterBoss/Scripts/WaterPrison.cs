using UnityEngine;
using System.Collections;

public class WaterPrison : MonoBehaviour
{
    private Transform player;
    private GameObject[] spawnedMinionsPrefab;
    private float duration;
    private int numberOfSpawnedMinions;
    private int radius;

    Vector3 prisonCenter;

    bool hasActivated;

    /// <summary>
    /// Initializes the water prison with the given parameters. Must be called immediately after instantiating the water prison prefab.
    /// </summary>
    /// <param name="plyr">The transform of the player to be trapped in the water prison.</param>
    /// <param name="dur">The duration for which the water prison will remain active.</param>
    /// <param name="minionPrefab">An array of minion prefabs to be spawned within the water prison.</param>
    /// <param name="minionCount">The number of minions to spawn within the water prison.</param>
    /// <param name="rad">The radius within which the minions will be spawned.</param>
    public void Initialize(Transform plyr, float dur, GameObject[] minionPrefab, int minionCount, int rad)
    {
        player = plyr;
        duration = dur;
        spawnedMinionsPrefab = minionPrefab;
        numberOfSpawnedMinions = minionCount;
        radius = rad;

        StartCoroutine(ActivateWaterPrison());
    }

    /// <summary>
    /// Activates the water prison, trapping the player in place and spawning minions around them for the duration of the prison. After the duration ends, the water prison is destroyed.
    /// </summary>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    IEnumerator ActivateWaterPrison()
    {
        if (player == null) 
        { 
            Destroy(gameObject);
            yield break;
        }
        
        hasActivated = true;

        prisonCenter = player.position;
        transform.position = prisonCenter;

        if (spawnedMinionsPrefab != null && spawnedMinionsPrefab.Length > 0)
        {
            for (int i = 0; i < numberOfSpawnedMinions; i++)
            {

                Vector3 spawnOffset = Random.insideUnitSphere * radius;
                spawnOffset.y = 0f;

                Vector3 spawnPos = transform.position + spawnOffset;

                int prefabIndex = Random.Range(0, spawnedMinionsPrefab.Length);
                Instantiate(spawnedMinionsPrefab[prefabIndex], spawnPos, Quaternion.identity);
            }
        }

        float timer = 0f;
        while(timer < duration)
        {
            if (player != null)
            {
                player.position = prisonCenter;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
