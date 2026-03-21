using System.Collections;
using UnityEngine;

public class SolarRainStrike : MonoBehaviour
{
    [SerializeField] GameObject warningVisuals;
    [SerializeField] GameObject rainStrikeVisuals;

    [SerializeField] float warningDuration;
    [SerializeField] float strikeStartHeight;
    [SerializeField] float strikeFallSpeed;
    [SerializeField] float strikeRadius;
    [SerializeField] int strikeDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StrikeRoutine());
    }

    IEnumerator StrikeRoutine()
    {
        warningVisuals.SetActive(true);
        rainStrikeVisuals.SetActive(false);

        yield return new WaitForSeconds(warningDuration);

        warningVisuals.SetActive(false);
        rainStrikeVisuals.SetActive(true);

        Vector3 targetPos = transform.position;
        Vector3 startPos = targetPos + Vector3.up * strikeStartHeight;
        rainStrikeVisuals.transform.position = startPos;

        while (Vector3.Distance(rainStrikeVisuals.transform.position, targetPos) > 0.1f)
        {
            rainStrikeVisuals.transform.position = Vector3.MoveTowards(rainStrikeVisuals.transform.position, targetPos, strikeFallSpeed * Time.deltaTime);

            yield return null;
        }

        rainStrikeVisuals.transform.position = targetPos;

        Collider[] hits = Physics.OverlapSphere(transform.position, strikeRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                IDamage dmg = hit.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.TakeDamage(strikeDamage);
                }
            }
        }

        yield return new WaitForSeconds(0.10f);

        Destroy(gameObject);
    }
}
