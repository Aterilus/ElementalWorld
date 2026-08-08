using UnityEngine;

public class IcicleWarning : MonoBehaviour
{
    float lifeTime;
    public void Initialize(float _lifeTime)
    {
        lifeTime = _lifeTime;

        Destroy(gameObject, lifeTime);
    }
}
