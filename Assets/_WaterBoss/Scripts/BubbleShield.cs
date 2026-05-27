using UnityEngine;

public class BubbleShield : MonoBehaviour
{
    enum BubbleState
    {
        LaunchingToShield,
        Orbiting,
        FiringOutward
    }
    private BubbleState state;

    private Transform boss;

    private float radius;
    private float duration;
    private float orbitSpeed;
    private float heightOffset;
    private float currentAngle;
    private float launchDuration;
    private float launchTimer;
    private float shootSpeed;
    private float shootLifeTime;
    private float shootTimer;

    private int damage;

    private Vector3 launchStartPos;
    private Vector3 orbitTargetPos;
    private Vector3 shootDirection;

    private void Update()
    {
        if (state == BubbleState.LaunchingToShield)
        {
            LaunchToShield();
        }
        else if (state == BubbleState.Orbiting)
        {
            OrbitAroundBoss();
        }
        else if (state == BubbleState.FiringOutward)
        {
            MoveOutward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != BubbleState.FiringOutward)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            IDamage damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            Destroy(gameObject);
        //}
        //else if (!other.CompareTag("WaterBoss"))
        //{
        //    Destroy(gameObject);
        }
    }

    public void Initialize(Transform _boss, float _radius, float _heightOffset, int dmg, float _orbitSpeed,float startingAngle, float _launchDuration, Vector3 _oribitTargetPos, float _shootSpeed, float _shootLifeTime)
    {
        boss = _boss;
        radius = _radius;
        heightOffset = _heightOffset;
        damage = dmg;
        orbitSpeed = _orbitSpeed;
        currentAngle = startingAngle;

        launchDuration = _launchDuration;
        launchTimer = 0f;

        launchStartPos = transform.position;
        orbitTargetPos = _oribitTargetPos;

        shootSpeed = _shootSpeed;
        shootLifeTime = _shootLifeTime;
        shootTimer = 0f;

        state = BubbleState.LaunchingToShield;

        //Destroy(gameObject, duration);
    }

    void LaunchToShield()
    {
        launchTimer += Time.deltaTime;

        float t = launchTimer / launchDuration;
        t = Mathf.Clamp01(t);

        transform.position = Vector3.Lerp(launchStartPos, orbitTargetPos, t);

        if (t >= 1f)
        {
            state = BubbleState.Orbiting;
        }
    }

    void OrbitAroundBoss()
    {
        if (boss == null) 
        {
            Destroy(gameObject);
            return;
        }

        currentAngle += orbitSpeed * Mathf.Deg2Rad * Time.deltaTime;

        Vector3 offset = new Vector3(Mathf.Cos(currentAngle) * radius, heightOffset, Mathf.Sin(currentAngle) * radius);

        transform.position = boss.position + offset;
    }

    public void LaunchOutward(Vector3 bossPos)
    {
        Vector3 center = bossPos + Vector3.up * heightOffset;
        shootDirection = (transform.position - center).normalized;

        if (shootDirection.sqrMagnitude < 0.001f)
        {
                        shootDirection = Vector3.forward;
        }

        shootTimer = 0f;
        state = BubbleState.FiringOutward;
    }

    void MoveOutward()
    {
        transform.position += shootDirection * shootSpeed * Time.deltaTime;

        shootTimer += Time.deltaTime;

        if (shootTimer >= shootLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
