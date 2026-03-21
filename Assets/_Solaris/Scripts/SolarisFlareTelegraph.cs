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

    public void TriggerFlare(Transform player)
    {
        StartCoroutine(FlareSequence(player));
    }

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
