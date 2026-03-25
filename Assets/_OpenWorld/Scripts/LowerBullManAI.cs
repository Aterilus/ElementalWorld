using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LowerBullManAI : MonoBehaviour, IDamage
{
    [SerializeField] int hp;
    [SerializeField] int slashDamageAmount;
    [SerializeField] int slashRadius;
    [SerializeField] int meleeDamageAmount;
    [SerializeField] int meleeDashRadius;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDistance;
    [SerializeField] float attackMaxCooldown;
    [SerializeField] float slashAttackMaxCooldown;
    [SerializeField] float dashAttackMaxCooldown;
    [SerializeField] float chaseRange;

    [SerializeField] GameObject player;
    [SerializeField] NavMeshAgent agent;

    public GameObject hpRoot;
    public Image hpFill;

    BullManHorde hordeManager;

    int hpOrig;

    float attackCooldown;
    float slashAttackCooldown;
    float dashAttackCooldown;
    float roamTimer;

    Vector3 playerDire;
    Vector3 startingPos;

    bool isAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        hpOrig = hp;

        attackCooldown = attackMaxCooldown;
        slashAttackCooldown = slashDamageAmount;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (isAttacking)
        {
            return;
        }

        playerDire = player.transform.position - transform.position;

        attackCooldown -= Time.deltaTime;
        slashAttackCooldown -= Time.deltaTime;
        dashAttackCooldown -= Time.deltaTime;

        if (attackCooldown > 0)
        {
            CheckRoam();
            return;
        }

        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }

        float disToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (disToPlayer <= slashRadius && slashAttackCooldown <= 0)
        {
            StartCoroutine(SlashAttack());
        }
        else if (disToPlayer <= meleeDashRadius && dashAttackCooldown <= 0)
        {
            StartCoroutine(RushMeleeAttack());
        }
        else if (disToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            CheckRoam();
        }
    }

    void CheckRoam()
    {
        if (agent.remainingDistance < 0.01f && roamTimer >= roamPauseTime)
        {
            Roam();
        }
    }

    void Roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 randPos = Random.insideUnitSphere * roamDistance;
        randPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randPos, out hit, roamDistance, 1);
        agent.SetDestination(hit.position);
    }

    IEnumerator SlashAttack()
    {
        isAttacking = true;
        FacePlayer();
        yield return new WaitForSeconds(4f);

        float disToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (disToPlayer <= slashRadius)
        {
            IDamage dmg = player.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(slashDamageAmount);
            }
        }

        yield return new WaitForSeconds(5f);
        attackCooldown = attackMaxCooldown;
        slashAttackCooldown = slashAttackMaxCooldown;
        isAttacking = false;
    }

    IEnumerator RushMeleeAttack()
    {
        isAttacking = true;
        FacePlayer();

        agent.stoppingDistance = 1.5f;
        agent.speed = 8;
        agent.SetDestination(player.transform.position);

        float rushTimer = 0f;
        float maxRushTimer = 1.25f;
        bool hasDamagedPlayer = false;

        while (rushTimer < maxRushTimer)
        {
            agent.SetDestination(player.transform.position);

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > 2f && !hasDamagedPlayer)
            {
                IDamage dmg = player.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.TakeDamage(slashDamageAmount);
                }

                hasDamagedPlayer = true;
                break;
            }

            rushTimer += Time.deltaTime;
            yield return null;
        }

        agent.ResetPath();
        yield return new WaitForSeconds(1f);

        attackCooldown = attackMaxCooldown;
        dashAttackCooldown = dashAttackMaxCooldown;
        isAttacking = false;
    }
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.stoppingDistance = meleeDashRadius;
        agent.SetDestination(player.transform.position);
    }

    void FacePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDire);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * facePlayerSpeed);
    }

    public void SetHordeManager(BullManHorde manager)
    {
        hordeManager = manager;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        hpRoot.gameObject.SetActive(true);
        hpFill.fillAmount = (float)hp / hpOrig;

        if (hp <= 0)
        {
            if (hordeManager != null)
            {
                hordeManager.OnEnemyDefeated();
            }

            Destroy(gameObject);
        }
    }
}
