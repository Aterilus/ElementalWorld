using System.Collections;
using UnityEngine;

public class AcidWave : MonoBehaviour
{
    private float speed;
    private float lifeTime;
    private float tickInterval;
    private float dotDuration;

    private int damage;
    private int tickDamage;

    private Vector3 direction;

    private bool hasHitPlayer;

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initialize(Vector3 _direction, float _speed, float _lifeTime, int _damage, int _tickDamage, float _tickInterval, float _dotDuration)
    {
        direction = _direction.normalized;
        speed = _speed;
        lifeTime = _lifeTime;
        damage = _damage;
        tickDamage = _tickDamage;
        tickInterval = _tickInterval;
        dotDuration = _dotDuration;

        Destroy(gameObject, _lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }

        if (hasHitPlayer) { return; }

        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            StartCoroutine(ApplyTickDamage(damageable));
        }
    }

    IEnumerator ApplyTickDamage(IDamage damage)
    {
        float timer = 0f;

        while (timer < dotDuration)
        {
            yield return new WaitForSeconds(tickInterval);

            if (damage != null)
            {
                damage.TakeDamage(tickDamage);
            }

            timer += tickInterval;
        }
    }
}
