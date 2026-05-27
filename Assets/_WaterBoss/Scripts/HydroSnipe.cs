using UnityEngine;

public class HydroSnipe : MonoBehaviour
{
    private float lifeTime;
    private float speed;
    private int damage;

    Vector3 dir;

    public void Init(Vector3 targetPos, float projectileSpeed, float lifeeTime, int dmg)
    {
        speed = projectileSpeed;
        damage = dmg;
        dir = (targetPos - transform.position).normalized;
        lifeTime = lifeeTime;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}
