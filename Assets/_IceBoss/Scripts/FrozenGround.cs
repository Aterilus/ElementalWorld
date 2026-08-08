using UnityEngine;

public class FrozenGround : MonoBehaviour
{
    float expandSpeed;
    float maxScale;
    float lifeTime;
    float freezeDuration;

    bool hasInitialized;

    // Update is called once per frame
    void Update()
    {
        if (!hasInitialized) { return; }

        Vector3 currentScale = transform.localScale;

        if (currentScale.x < maxScale)
        {
            currentScale.x += expandSpeed * Time.deltaTime;
            currentScale.z += expandSpeed * Time.deltaTime;

            currentScale.y = 0.1f;

            currentScale.x = Mathf.Min(currentScale.x, maxScale);
            currentScale.z = Mathf.Min(currentScale.z, maxScale);

            transform.localScale = currentScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IFreeze freeze = other.GetComponent<IFreeze>();
            if (freeze != null)
            {
                freeze.Freeze(freezeDuration);
            }
        }
        else if (other.CompareTag("Ground"))
        {
            
        }
    }

    public void Initialize(float _expandSpeed, float _maxScale, float _lifeTime, float _freezeDuration)
    {
        expandSpeed = _expandSpeed;
        maxScale = _maxScale;
        lifeTime = _lifeTime;
        freezeDuration = _freezeDuration;

        transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);

        hasInitialized = true;
        Destroy(gameObject, lifeTime);
    }
}
