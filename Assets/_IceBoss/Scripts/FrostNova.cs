using UnityEngine;

public class FrostNova : MonoBehaviour
{
    int damage;

    float freezeDuration;
    float expandTime;
    float maxScale;
    float jumpClearHeight;

    bool hasInitialized;

    // Update is called once per frame
    void Update()
    {
        if (!hasInitialized)
        {
            return; 
        }

        transform.localScale += new Vector3(expandTime, 0f,expandTime) * Time.deltaTime;

        if (transform.localScale.x >= maxScale)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.y < jumpClearHeight)
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
            }
        }
        else
        {
            return;
        }
    }

    public void Initialize(int _damage, float _freezeDuration, float _expandTime, float _maxScale, float _jumpClearance)
    {
        damage = _damage;
        freezeDuration = _freezeDuration;
        expandTime = _expandTime;
        maxScale = _maxScale;
        jumpClearHeight = _jumpClearance;
        hasInitialized = true;
        transform.localScale = new Vector3(0.1f, 0.05f, 0.1f);
    }
}
