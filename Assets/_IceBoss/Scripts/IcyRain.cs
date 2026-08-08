using UnityEngine;

public class IcyRain : MonoBehaviour
{
    int damage;

    float freezeDuration;
    float fallSpeed;
    float lifeTime;
    float frozenGroundExpandSpeed;
    float frozenGroundMaxScale;
    float frozenGroundLifeTime;
    float frozenGroundFreezeDuration;

    GameObject frozenGroundPrefab;

    Vector3 groundSpawnPosition;

    bool hasInitialized;

    public void Initialize(int _damage, float _freezeDamage, float _fallSpeed, float _lifeTime, float _frozenGroundExpandSpeed, float _frozenGroundMaxScale, float _frozenGroundLifeTime,
        float _frozenGroundFreezeDuration, GameObject _frozenGroundPrefab, Vector3 spawnPoint)
    {
        damage = _damage;
        freezeDuration = _freezeDamage;
        fallSpeed = _fallSpeed;
        lifeTime = _lifeTime;
        frozenGroundExpandSpeed = _frozenGroundExpandSpeed;
        frozenGroundMaxScale = _frozenGroundMaxScale;
        frozenGroundLifeTime = _frozenGroundLifeTime;
        frozenGroundFreezeDuration = _frozenGroundFreezeDuration;
        frozenGroundPrefab = _frozenGroundPrefab;
        groundSpawnPosition = spawnPoint;
        hasInitialized = true;
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasInitialized) {  return; }

        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
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
}
