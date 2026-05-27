using UnityEngine;

public class ArcSpear : MonoBehaviour
{
    int speed;
    int damage;

    float lifeTime;

    LightningAbilityPack moves;

    Vector3 dire;

    // Update is called once per frame
    void Update()
    {
        transform.position += dire * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(int _speed, int _damage, float _lifeTime, Vector3 _direction, LightningAbilityPack _moves)
    {
        speed = _speed;
        damage = _damage;
        lifeTime = _lifeTime;
        dire = _direction;
        moves = _moves;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
                moves.ApplyStaticMark();
                Destroy(gameObject);
            }
        }
    }
}
