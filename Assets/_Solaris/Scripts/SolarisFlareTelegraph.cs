using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarisFlareTelegraph : MonoBehaviour
{
    [Header("----------Prefabs----------")]
    [SerializeField] GameObject flareMarkerPrefab;
    [SerializeField] GameObject flareDamagePrefab;

    [Header("----------Durations----------")]
    [SerializeField] float warningDuration;
    [SerializeField] float damageDuration;

    [Header("----------Flare Count----------")]
    [SerializeField] int flareCount;

    [Header("----------Flare Damage Amount----------")]
    [SerializeField] int flareDamageAmount;

    [Header("----------Radius----------")]
    [SerializeField] float radius;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage damage = other.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.TakeDamage(flareDamageAmount);
            }
        }
    }

    /// <summary>
    /// Triggers the flare telegraph sequence for the given player transform. This will spawn warning markers around the player, then after a delay, spawn damage zones that can harm the player.
    /// </summary>
    /// <param name="player">The transform of the player to target with the flare telegraph.</param>
    public void TriggerFlare(Transform player)
    {
        StartCoroutine(FlareSequence(player));
    }

    /// <summary>
    /// Handles the entire flare sequence: spawning warning markers, waiting for the warning duration, then spawning damage zones that can harm the player.
    /// </summary>
    /// <param name="player">The transform of the player to target with the flare telegraph.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
   IEnumerator FlareSequence(Transform player)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < flareCount; ++i)
        {
            Vector3 p = player.position + Random.insideUnitSphere * radius;
            p.y = 0f;
            positions.Add(p);
        }

        List<GameObject> markers = new List<GameObject>();
        foreach (var pos in positions)
        {
            GameObject m = Instantiate(flareMarkerPrefab, pos, Quaternion.identity);
            markers.Add(m);
        }

        yield return new WaitForSeconds(warningDuration);

        foreach (var marker in markers)
        {
            if (marker != null)
            {
                Destroy(marker);
            }
        }

        foreach (var pos in positions)
        {
            GameObject dz = Instantiate(flareDamagePrefab, pos, Quaternion.identity);
            Destroy(dz, damageDuration);
        }
    }
}
