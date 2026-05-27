using System.Collections;
using UnityEngine;

public class ThunderStrike : MonoBehaviour
{
    float strikeRadius;
    float lifeTime;

    int damage;

    LightningAbilityPack moves;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, strikeRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                IDamage dmg = collider.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.TakeDamage(damage);
                    moves.ApplyStaticMark();
                }
            }
        }

        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(int _damage, float _strikeRadius, float _lifeTime, LightningAbilityPack _moves)
    {
        damage = _damage;
        strikeRadius = _strikeRadius;
        lifeTime = _lifeTime;
        moves = _moves;
    }
}
