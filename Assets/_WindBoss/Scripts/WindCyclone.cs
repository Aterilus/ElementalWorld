using UnityEngine;

public class WindCyclone : MonoBehaviour
{
    private int damageAmount;
    private float moveSpeed;
    private float lifeTime;

    Vector3 moveDire;

    bool hasBeenInitialized;

    // Update is called once per frame
    void Update()
    {
        if (!hasBeenInitialized) { return; }

        transform.position += moveDire * moveSpeed * Time.deltaTime;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasBeenInitialized) { return; }

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

    /// <summary>
    /// Initializes the cyclone with the given parameters. Must be called immediately after instantiating the cyclone prefab.
    /// </summary>
    /// <param name="moveDirec">The direction in which the cyclone will move.</param>
    /// <param name="moveeSpeed">The speed at which the cyclone will move.</param>
    /// <param name="damageeAmount">The amount of damage the cyclone will deal upon hitting the player.</param>
    /// <param name="lifeeTime">The lifetime of the cyclone before it is automatically destroyed.</param>
    public void Initialize(Vector3 moveDirec, float moveeSpeed, int damageeAmount, float lifeeTime)
    {
        moveDire = moveDirec.normalized;
        moveSpeed = moveeSpeed;
        damageAmount = damageeAmount;
        lifeTime = lifeeTime;
        hasBeenInitialized = true;
    }
}
