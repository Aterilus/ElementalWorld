using System.Collections;
using UnityEngine;

public class StaticDetonation : MonoBehaviour
{
    int damage;
    float detonationRadius;
    float lifeTime;
    LightningAbilityPack moveSet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detonationRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                IDamage dmg = collider.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.TakeDamage(damage);
                }
            }
        }

        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void Initialize(int _damage, float _detonationRadius, float _lifeTime, LightningAbilityPack _moveSet)
    {
        damage = _damage;
        detonationRadius = _detonationRadius;
        lifeTime = _lifeTime;
        moveSet = _moveSet;
    }
}
