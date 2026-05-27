using System.Collections;
using UnityEngine;

public class IonCrashImpact : MonoBehaviour
{
    float impactRadius;
    float lifeTime;
    float nearbyMineDetonationRadius;

    int damage;

    LightningAbilityPack moveSet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        Collider[] coliders = Physics.OverlapSphere(transform.position, impactRadius);

        foreach (Collider col in coliders)
        {
            if (col.CompareTag("Player"))
            {
                IDamage dmg = col.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.TakeDamage(damage);
                }
                if (moveSet != null)
                {
                    moveSet.ApplyStaticMark();
                }
            }
        }

        if (moveSet != null)
        {
            moveSet.DetonateNearbyMines(transform.position, nearbyMineDetonationRadius);
        }

        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void Initialize(int _damage, float _impactRadius, float _lifeTime, float _nearbyDestonateRadius, LightningAbilityPack moves)
    {
        damage = _damage;
        impactRadius = _impactRadius;
        lifeTime = _lifeTime;
        nearbyMineDetonationRadius = _nearbyDestonateRadius;
        moveSet = moves;
    }
}
