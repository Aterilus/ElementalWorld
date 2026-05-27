using System.Collections;
using UnityEngine;

public class VoltageMine : MonoBehaviour
{
    int damage;
    float explosionRadius;
    float armDelay;
    bool isArmed;
    LightningAbilityPack moves;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(armDelay);
        isArmed = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(int _damage, float _explosionRadius, float _armDelay, LightningAbilityPack _moves)
    {
        damage = _damage;
        explosionRadius = _explosionRadius;
        armDelay = _armDelay;
        isArmed = false;
        moves = _moves;
    }

    public void Explode()
    {
        Collider[] collidersCheck = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in collidersCheck)
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
        

        //Spawn Explosion visuals in version 1.5


        Destroy(gameObject);
    }
}
