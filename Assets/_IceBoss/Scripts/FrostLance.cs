using UnityEngine;

public class FrostLance : MonoBehaviour
{
    int damage;

    float freezeDuration;
    float moveSpeed;
    float lifeTime;
    float trackingStrength;

    Transform target;

    Vector3 moveDire;

    bool hasInitialized;
    bool hasFired;

    // Update is called once per frame
    void Update()
    {
        if (!hasInitialized)
        {
            return;
        }
        if (!hasFired)
        {
            return;
        }

        if (target != null)
        {
            Vector3 targetPos = target.position;
            targetPos.y = transform.position.y;

            Vector3 desiredDire = targetPos - transform.position;
            desiredDire.y = 0;
            desiredDire.Normalize();

            moveDire = Vector3.Lerp(moveDire, desiredDire, trackingStrength * Time.deltaTime);
            moveDire.Normalize();
        }

        transform.position += moveDire * moveSpeed * Time.deltaTime;
        if (moveDire != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDire);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }

            IFreeze player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Freeze(freezeDuration);
            }

            Destroy(gameObject);
        }
    }

    public void Initialize(int _damage, float _freezeDuration, float _moveSpeed, float _lifeTime, float _trackingStrength, Transform _target)
    {
        damage = _damage;
        freezeDuration = _freezeDuration;
        moveSpeed = _moveSpeed;
        lifeTime = _lifeTime;
        trackingStrength = _trackingStrength;
        target = _target;

        hasInitialized = true;
        hasFired = false;

        Destroy(gameObject, lifeTime);
    }

    public void Fire()
    {
        if (target == null)
        {
            target = GameManager.instance.player.transform;
        }

        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;

        moveDire = targetPos - transform.position;
        moveDire.y = 0;
        moveDire.Normalize();

        hasFired = true;
    }
}
