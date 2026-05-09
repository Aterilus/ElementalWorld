using UnityEngine;

public class WindHurricane : MonoBehaviour
{
    private int damageAmount;
    private float speed;
    private float lifeTime;

    Vector3 moveDire;

    bool isActive;

    // Update is called once per frame
    void Update()
    {
        if (!isActive) { return; }

        transform.position += moveDire * speed * Time.deltaTime;
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

    /// <summary>
    /// Initializes the hurricane with the given parameters. Must be called immediately after instantiating the hurricane prefab.
    /// </summary>
    /// <param name="dire">The direction in which the hurricane will move.</param>
    /// <param name="speedd">The speed at which the hurricane will move.</param>
    /// <param name="damageeAmount">The amount of damage the hurricane will deal upon hitting the player.</param>
    /// <param name="lifeeTime">The lifetime of the hurricane before it is automatically destroyed.</param>
    public void Initialize(Vector3 dire, float speedd, int damageeAmount, float lifeeTime)
    {
        moveDire = dire;
        speed = speedd;
        damageAmount = damageeAmount;
        lifeTime = lifeeTime;
        isActive = true;

        Destroy(gameObject, lifeTime);
    }
}
