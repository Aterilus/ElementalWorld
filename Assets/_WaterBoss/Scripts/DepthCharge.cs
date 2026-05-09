using System.Collections;
using UnityEngine;

public class DepthCharge : MonoBehaviour
{
    [SerializeField] LayerMask damageLayers;

    [SerializeField] private GameObject telegraphCircle;
    [SerializeField] private GameObject expandingBubble;
    [SerializeField] private GameObject splashEffect;

    [SerializeField] private float expandDuration;
    [SerializeField] private float splashLifeTime;

    [SerializeField] private Vector3 startBubbleScale = Vector3.one * 0.1f;
    [SerializeField] private Vector3 maxBubbleScale = Vector3.one * 2.5f;

    private float delay;
    private float radius;

    private int damage;

    private bool hasExploded = false;
    public void Initialize(float _delay, float _radius, int _damage)
    {
        delay = _delay;
        radius = _radius;
        damage = _damage;

        StartCoroutine(ExplosionSequence());
    }

    IEnumerator ExplosionSequence()
    {
        telegraphCircle.SetActive(true);
        expandingBubble.SetActive(false);
        splashEffect.SetActive(false);

        yield return new WaitForSeconds(delay);

        telegraphCircle.SetActive(false);

        expandingBubble.SetActive(true);
        expandingBubble.transform.localScale = startBubbleScale;

        float timer = 0f;

        while (timer < expandDuration)
        {
            float t = timer / expandDuration;

            expandingBubble.transform.localScale = Vector3.Lerp(startBubbleScale, maxBubbleScale, t);

            timer += Time.deltaTime;
            yield return null;
        }

        splashEffect.SetActive(true);

        ExplodeDamage();

        yield return new WaitForSeconds(splashLifeTime);

        Destroy(gameObject);
    }

    void ExplodeDamage()
    {
        if (hasExploded)
        {
            return;
        }
        hasExploded = true;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, damageLayers);

        foreach (Collider hit in hitColliders)
        {
            IDamage dmg = hit.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
