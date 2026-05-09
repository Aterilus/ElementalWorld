using UnityEngine;

public class PhaseSwim : MonoBehaviour
{
    private float speed;
    private float lifeTime;
    private float knockbackForce;

    private int damage;

    private Vector3 direction;

    private bool hasHitPlayer;

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initialize(Vector3 _direction, float _speed, float _lifeTime, int _damage, float _knockbackForce)
    {
        direction = _direction.normalized;
        speed = _speed;
        lifeTime = _lifeTime;
        damage = _damage;
        knockbackForce = _knockbackForce;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if  (!other.CompareTag("Player"))
        {
            return;
        }

        if (hasHitPlayer)
        {
            return;
        }

        hasHitPlayer = true;

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplyKnockback(direction, knockbackForce);
        }
    }
}
