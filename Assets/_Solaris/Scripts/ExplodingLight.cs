using UnityEngine;
using System.Collections;

public class ExplodingLight : MonoBehaviour
{
    [SerializeField] GameObject warningVisuals;
    [SerializeField] GameObject explosionVisuals;

    [SerializeField] float delayBeforeExplosion;
    [SerializeField] float explosionRadius;
    [SerializeField] int explosionDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LightExplosionRoutine());
    }

    IEnumerator LightExplosionRoutine()
    {
        warningVisuals.SetActive(true);

        yield return new WaitForSeconds(delayBeforeExplosion);

        warningVisuals.SetActive(false);
        explosionVisuals.SetActive(true);

        float timer = 0f;
        float duration = 0.2f;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one * explosionRadius;

        while (timer < duration)
        {
            explosionVisuals.transform.localScale = Vector3.Lerp(startScale, endScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        explosionVisuals.transform.localScale = endScale;

        CheckIfPlayerIsInRadius();

        yield return new WaitForSeconds(0.04f);

        Destroy(gameObject);
    }

    void CheckIfPlayerIsInRadius()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                IDamage dmg = hit.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.TakeDamage(explosionDamage);
                }
            }
        }
    }
}
