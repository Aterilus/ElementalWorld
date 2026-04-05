using UnityEngine;

public class WindCyclone : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] float moveSpeed;
    [SerializeField] float lifeTime;

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

    public void Initialize(Vector3 moveDirec, float moveeSpeed, int damageeAmount, float lifeeTime)
    {
        moveDire = moveDirec.normalized;
        moveSpeed = moveeSpeed;
        damageAmount = damageeAmount;
        lifeTime = lifeeTime;
        hasBeenInitialized = true;
    }
}
