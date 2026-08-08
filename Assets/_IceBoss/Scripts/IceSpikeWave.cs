using UnityEngine;

public class IceSpikeWave : MonoBehaviour
{
    int damage;

    float lifeTime;
    float moveSpeed;
    float spreadSpeed;
    float freezeDuration;

    Vector3 forwardDire;
    Vector3 sideSpreadDire;

    bool hasInitialized;

    private void Update()
    {
        if (!hasInitialized) { return; }

        Vector3 moveDire = forwardDire * moveSpeed + sideSpreadDire * spreadSpeed;
        moveDire.y = 0;
        moveDire.Normalize();
        transform.position += moveDire * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);

                IFreeze player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Freeze(freezeDuration);
                }

                Destroy(gameObject);
            }
        }
    }

    public void Initialize(int _damage, float _lifeTime, float _moveSpeed, float _spreadSpeed, float _freezeDuration, Vector3 _forward, Vector3 _side)
    {
        damage = _damage;
        lifeTime = _lifeTime;
        moveSpeed = _moveSpeed;
        spreadSpeed = _spreadSpeed;
        freezeDuration = _freezeDuration;
        forwardDire = _forward;
        sideSpreadDire = _side;
        hasInitialized = true;

        Destroy(gameObject, lifeTime);
    }
}
