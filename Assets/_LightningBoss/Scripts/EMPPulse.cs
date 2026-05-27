using System.Collections;
using UnityEngine;

public class EMPPulse : MonoBehaviour
{
    float pulseRadius;
    float lifeTime;

    int damage;

    LightningAbilityPack moveSet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        Collider[] coliders = Physics.OverlapSphere(transform.position, pulseRadius);
        foreach (Collider colider in coliders)
        {
            if (colider.CompareTag("Player"))
            {
                IDamage dmg = colider.GetComponent<IDamage>();
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

        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void Initialize(int _damage, float _pulseRadius, float _lifeTime, LightningAbilityPack _moveSet)
    {
        damage = _damage;
        pulseRadius = _pulseRadius;
        lifeTime = _lifeTime;
        moveSet = _moveSet;
    }
}
