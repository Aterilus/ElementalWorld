using UnityEngine;

public class DaggerProjectile : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] float lifetime;

    Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (target == null) { return; }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.forward = direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage damagee = other.GetComponent<IDamage>();
            if (damagee != null)
            {
                damagee.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }
    }

    public void SetTarget(Transform targetTarget)
    {
        target = targetTarget;
    }
}
