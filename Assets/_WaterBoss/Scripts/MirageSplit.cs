using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MirageSplit : MonoBehaviour, IDamage, IHealthUI
{
    [SerializeField] Transform attackSpawnPoint;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] GameObject hpRoot;

    [SerializeField] Image hpFillImage;

    [SerializeField] float roamRadius;
    [SerializeField] float repositionalInterval;

    [SerializeField] int hp;

    Transform player;
    Transform waterBoss;

    WaterAbilityPack moveSet;

    float attackInterval;
    float projectileSpeed;
    float projectileLifeTime;

    int projectileDamage;
    int hpOrig;

    bool isDead;

    GameObject cloneHydroSnipePrefab;

    private void Start()
    {
        if (hpRoot != null)
        {
            hpRoot.SetActive(true);
        }

        hpOrig = hp;
        UpdateHPUI();
    }

    public void Initialize(Transform plyr, Transform boss, GameObject hydroSnipePrefab, float atkInterval, float projSpeed, float projLife, int projDmg, WaterAbilityPack moves)
    {
        player = plyr;
        waterBoss = boss;
        cloneHydroSnipePrefab = hydroSnipePrefab;
        attackInterval = atkInterval;
        projectileSpeed = projSpeed;
        projectileLifeTime = projLife;
        projectileDamage = projDmg;
        moveSet = moves;

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        StopAllCoroutines();
        StartCoroutine(CloneAI());
    }

    [System.Obsolete]
    IEnumerator CloneAI()
    {
        yield return new WaitForSeconds(0.5f);

        while (!isDead && hp > 0)
        {
            if (player == null || waterBoss == null)
            {
                Die();
                yield break;
            }

            MoveToNewSpot();

            float moveTimer = 0f;

            while (moveTimer < repositionalInterval)
            {
                if (player != null)
                {
                    FacePlayer();
                }

                moveTimer += Time.deltaTime;
                yield return null;
            }

            FacePlayer();

            FireHydroSnipe();

            yield return new WaitForSeconds(attackInterval);
        }

        Die();
    }

    void FacePlayer()
    {
        Vector3 playerDire = player.position - transform.position;
        playerDire.y = 0f;

        if (playerDire.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(playerDire.normalized);
        }
    }

    [System.Obsolete]
    void FireHydroSnipe()
    {
        if (cloneHydroSnipePrefab == null || player == null || attackSpawnPoint == null) { return; }

        GameObject proj = Instantiate(cloneHydroSnipePrefab, attackSpawnPoint.position, attackSpawnPoint.rotation);
        HydroSnipe hsp = proj.GetComponent<HydroSnipe>();

        if (hsp != null)
        {
            hsp.Init(player.position, projectileSpeed, projectileLifeTime, projectileDamage);
        }
    }

    void MoveToNewSpot()
    {
        if (agent == null || player == null) { return; }

        Vector3 randOffset = Random.insideUnitSphere * roamRadius;
        randOffset.y = 0f;

        Vector3 candPos = player.position + randOffset;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(candPos, out hit, roamRadius, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        if (hpRoot != null)
        {
            hpRoot.SetActive(true);
        }

        hp -= damage;
        UpdateHPUI();

        if (hp < 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (moveSet != null)
        {
            moveSet.RemoveMirageClone(this);
        }

        Destroy(gameObject);
    }

    public void UpdateHPUI()
    {
        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = (float)hp / hpOrig;
        }
    }
}
