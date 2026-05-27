using UnityEngine;

public class WindPhantomGust : MonoBehaviour
{
    private float speed;
    private float lifeTime;
    private int damage;

    Vector3 direction;

    bool isInitialized;

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized) { return; }
        transform.position += direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// Initializes the gust with the given parameters. Must be called immediately after instantiating the gust prefab.
    /// </summary>
    /// <param name="dire">The direction in which the gust will move.</param>
    /// <param name="speedd">The speed at which the gust will move.</param>
    /// <param name="damagee">The amount of damage the gust will deal upon hitting the player.</param>
    /// <param name="lifeeTime">The lifetime of the gust before it is automatically destroyed.</param>
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
