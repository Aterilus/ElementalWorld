using UnityEngine;

public class WindAirBlade : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] float moveSpeed;
    [SerializeField] float lifeTime;

    Vector3 moveDire;

    bool isActive;

    // Update is called once per frame
    void Update()
    {
        if (!isActive) { return; }

        transform.position += moveDire * moveSpeed * Time.deltaTime;
    }

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
