using UnityEngine;
using System;
using UnityEngine.AI;
using System.Collections;

public class BossBullManAI : MonoBehaviour, IDamage
{
    [SerializeField] GameObject player;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] int hp;
    [SerializeField] int slashDamageAmount;
    [SerializeField] int meleeDamageAmount;
    [SerializeField] int meleeRange;
    [SerializeField] int slashRange;

    [SerializeField] float attackMaxCooldown;
    [SerializeField] float slachAttackMaxCooldown;
    [SerializeField] float facePlayerSpeed;

    BullManHorde hordeManager;

    int hpOrig;

    float attackCooldown;
    float slachAttackCooldown;

    Vector3 playerDire;

    bool isAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpOrig = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (isAttacking) { return; }

        playerDire = player.transform.position - transform.position;
        playerDire.y = 0;

        slachAttackCooldown -= Time.deltaTime;
        attackCooldown -= Time.deltaTime;

        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distToPlayer < meleeRange && attackCooldown <= 0)
        {
            StartCoroutine(MeleeAttack());
        }
        else if (distToPlayer < slashRange && slachAttackCooldown <= 0 && attackCooldown <= 0)
        {
            StartCoroutine(SlashAttack());
        }
        else
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.stoppingDistance = meleeRange;
        agent.SetDestination(player.transform.position);
    }

    void FacePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDire);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * facePlayerSpeed);
    }

    IEnumerator MeleeAttack()
    {
        isAttacking = true;
        agent.isStopped = true;

        FacePlayer();

        yield return new WaitForSeconds(0.5f);

        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distToPlayer < meleeRange)
        {
            IDamage dmg = player.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(meleeDamageAmount);
            }
        }

        yield return new WaitForSeconds(0.5f);

        attackCooldown = attackMaxCooldown;

        agent.isStopped = false;
        isAttacking = false;
    }

    IEnumerator SlashAttack()
    {
        isAttacking = true;
        agent.isStopped = true;

        FacePlayer();

        yield return new WaitForSeconds(1.5f);

        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distToPlayer < slashRange)
        {
            IDamage dmg = player.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(slashDamageAmount);
            }
        }

        yield return new WaitForSeconds(1f);

        attackCooldown = attackMaxCooldown;
        slachAttackCooldown = slachAttackMaxCooldown;

        agent.isStopped = false;
        isAttacking = false;
    }

    public void SetHordeManager(BullManHorde manager)
    {
        hordeManager = manager;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        UpdateHPUI();

        if (hp <= 0)
        {
            GameManager.instance.bossHPUI.gameObject.SetActive(false);

            if (hordeManager != null)
            {
                hordeManager.OnEnemyDefeated();
            }

            Destroy(gameObject);
        }
    }

    public void UpdateHPUI()
    {
        GameManager.instance.bossHPBar.fillAmount = (float)hp / hpOrig;
    }
}
