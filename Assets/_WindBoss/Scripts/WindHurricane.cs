using UnityEngine;

public class WindHurricane : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] float speed;
    [SerializeField] float lifeTime;

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
