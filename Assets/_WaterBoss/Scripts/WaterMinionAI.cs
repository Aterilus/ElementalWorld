using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaterMinionAI : MonoBehaviour, IDamage, IHealthUI
{
    [SerializeField] NavMeshAgent agent;

    [SerializeField] float roamRadius;
    [SerializeField] float roamWaitTime;
    [SerializeField] float detectionRange;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;

    [SerializeField] int hp;
    [SerializeField] int damage;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDistance;

    [SerializeField] GameObject hpRoot;

    [SerializeField] Image hpFill;

    Transform player;

    float roamTimer;

    int currentHp;

    bool isAttacking;

    Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;

        currentHp = hp;
        UpdateHPUI();

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (player == null)
        {
            player = GameManager.instance.player.transform;
        }
    }

    void Update()
    {
        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        else if (distToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            CheckRoam();
        }
    }

    /// <summary>
    /// Handles the chasing behavior of the minion. If the NavMeshAgent is not null, it will set the agent to be active and set the player's position as the destination for the agent to move towards.
    /// </summary>
    void ChasePlayer()
    {
        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    /// <summary>
    /// Checks if the minion should roam. If the minion is close enough to its current destination and the roam timer has exceeded the roam pause time, it will call the Roam() method to find a new random destination.
    /// </summary>
    void CheckRoam()
    {
        if (agent.remainingDistance < 0.01f && roamTimer >= roamPauseTime)
        {
            Roam();
        }
    }

    /// <summary>
    /// Handles the roaming behavior of the minion. When called, it resets the roam timer, sets the stopping distance to 0, and calculates a random position within a specified radius from the starting position. The minion then sets this random position as its new destination on the NavMesh.
    /// </summary>
    void Roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 randPos = Random.insideUnitSphere * roamDistance;
        randPos += startPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randPos, out hit, roamDistance, 1);
        agent.SetDestination(hit.position);
    }

    /// <summary>
    /// Handles the attack behavior of the minion. The minion will stop moving, face the player, and apply damage to the player if they are within attack range. After attacking, it will wait for the specified cooldown before it can attack again.
    /// </summary>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    IEnumerator Attack()
    {
        isAttacking = true;

        if (agent != null)
        {
            agent.isStopped = true;
        }

        if (player != null)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }

        IDamage dmg = player.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    /// <summary>
    /// Reduces the minion's HP by the specified damage amount and updates the health bar. If HP drops to 0 or below, the minion is destroyed.
    /// </summary>
    /// <param name="damage">The amount of damage to apply to the minion.</param>
    public void TakeDamage(int damage)
    {
        hp -= damage;
        hpRoot.gameObject.SetActive(true);
        UpdateHPUI();

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHPUI()
    {
        hpFill.fillAmount = (float)hp / currentHp;
    }
}
