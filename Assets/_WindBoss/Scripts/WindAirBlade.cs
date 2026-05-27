using UnityEngine;

public class WindAirBlade : MonoBehaviour
{
    private int damageAmount;
    private float moveSpeed;
    private float lifeTime;

    Vector3 moveDire;

    bool isActive;

    // Update is called once per frame
    void Update()
    {
        if (!isActive) { return; }

        transform.position += moveDire * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Initializes the air blade with the given parameters. Must be called immediately after instantiating the air blade prefab.
    /// </summary>
    /// <param name="dire">The direction in which the air blade will move.</param>
    /// <param name="speedd">The speed at which the air blade will move.</param>
    /// <param name="damageeAmount">The amount of damage the air blade will deal upon hitting the player.</param>
    /// <param name="lifeeTime">The lifetime of the air blade before it is automatically destroyed.</param>
    public void Initialize(Vector3 dire, float speedd, int damageeAmount, float lifeeTime)
    {
        moveDire = dire;
        moveSpeed = speedd;
        damageAmount = damageeAmount;
        lifeTime = lifeeTime;
        isActive = true;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) { return; }

        if (other.CompareTag("Player"))
        {
            IDamage damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }
}
