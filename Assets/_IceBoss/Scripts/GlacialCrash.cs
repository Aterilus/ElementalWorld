using UnityEngine;

public class GlacialCrash : MonoBehaviour
{
    GameObject frozenGroundPrefab;

    int damage;

    float freezeDuration;
    float moveSpeed;
    float lifeTime;
    float frozenGroundExpandSpeed;
    float frozenGroundMaxScale;
    float frozenGroundLifeTime;
    float frozenGroundFreezeDuration;

    Vector3 impactPos;
    Vector3 moveDire;

    bool hasInitialized;

    void Update()
    {
        if (!hasInitialized) { return; }
        
        transform.position += moveDire * moveSpeed * Time.deltaTime;
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

            IFreeze freeze = other.GetComponent<IFreeze>();
            if (freeze != null)
            {
                freeze.Freeze(freezeDuration);
            }
        }
        else if (other.CompareTag("Ground"))
        {
            GameObject ground = Instantiate(frozenGroundPrefab, transform.position, Quaternion.identity);

            FrozenGround frozenGround = ground.GetComponent<FrozenGround>();
            if (frozenGround != null)
            {
                frozenGround.Initialize(frozenGroundExpandSpeed, frozenGroundMaxScale, frozenGroundLifeTime, frozenGroundFreezeDuration);
            }

            Destroy(gameObject);
        }
    }

    public void Initialize(int _damage, float _freezeDuration, float _moveSpeed, float _lifeTime, GameObject _frozenGroundPrefab, Vector3 _impactPos,
        float _frozenGroundExpandSpeed, float _frozenGroundMaxScale, float _frozenGroundLifeTime, float _frozenGroundFreezeDuration)
    {
        damage = _damage;
        freezeDuration = _freezeDuration;
        moveSpeed = _moveSpeed;
        lifeTime = _lifeTime;
        frozenGroundPrefab = _frozenGroundPrefab;
        impactPos = _impactPos;
        frozenGroundExpandSpeed = _frozenGroundExpandSpeed;
        frozenGroundMaxScale = _frozenGroundMaxScale;
        frozenGroundLifeTime = _frozenGroundLifeTime;
        frozenGroundFreezeDuration = _frozenGroundFreezeDuration;

        moveDire = (impactPos - transform.position);
        moveDire = moveDire.normalized;

        hasInitialized = true;

        Destroy(gameObject, lifeTime);
    }
}
