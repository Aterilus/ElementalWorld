using UnityEngine;

public class WindPhantomGust : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    [SerializeField] int damage;

    Vector3 direction;

    bool isInitialized;

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized) { return; }
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initialize(Vector3 dire, float speedd, int damagee, float lifeeTime)
    {
        direction = dire;
        speed = speedd;
        damage = damagee;
        lifeTime = lifeeTime;
        isInitialized = true;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isInitialized) { return; }
        if (other.CompareTag("Player"))
        {
            IDamage damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
