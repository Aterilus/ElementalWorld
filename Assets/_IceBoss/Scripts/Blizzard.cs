using UnityEngine;

public class Blizzard : MonoBehaviour
{
    float lifeTime;
    float rotateSpeed;
    float expandSpeed;
    float freezeDuration;
    float tickRate;
    float damageTimer;

    int damage;

    bool hasInitialized;
    bool isPlayerInside;

    ParticleSystem blizzardPS;

    Transform playerTrans;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (blizzardPS == null)
        {
            blizzardPS = GetComponent<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasInitialized)
        {
            return;
        }

        // Play particle effects or other visual/audio effects here if needed
        if (blizzardPS != null && !blizzardPS.isPlaying)
        {
            blizzardPS.Play();
        }

        // Rotate the blizzard effect for visual flair
        RotateBlizzard();

        //Expand blizzard radius and visuals over time
        ExpandBlizzard();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            playerTrans = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPlayerInside)
            {
                // Play sounds or other effects here if needed

                damageTimer += Time.deltaTime;
                if (damageTimer >= tickRate)
                {
                    if (playerTrans != null)
                    {
                        playerTrans.GetComponent<IDamage>();
                        if (playerTrans.GetComponent<IDamage>() != null)
                        {
                            playerTrans.GetComponent<IDamage>().TakeDamage(damage);
                        }

                        playerTrans.GetComponent<IFreeze>();
                        if (playerTrans.GetComponent<IFreeze>() != null)
                        {
                            playerTrans.GetComponent<IFreeze>().Freeze(freezeDuration);
                        }

                    }
                    damageTimer = 0;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerTrans = null;
            damageTimer = 0;
        }
    }

    public void Initialize(float _lifeTime, float _rotateSpeed, float _expandSpeed, float _freezeDuration, float _tickRate, float _damageTimer, int _damage)
    {
        lifeTime = _lifeTime;
        rotateSpeed = _rotateSpeed;
        expandSpeed = _expandSpeed;
        freezeDuration = _freezeDuration;
        tickRate = _tickRate;
        damageTimer = _damageTimer;
        damage = _damage;
        hasInitialized = true;
        Destroy(gameObject, lifeTime);
    }

    void RotateBlizzard()
    { 
        gameObject.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void ExpandBlizzard()
    {
        gameObject.transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;
    }
}
