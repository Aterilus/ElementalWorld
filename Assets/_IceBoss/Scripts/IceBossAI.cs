using System.Collections;
using UnityEngine;

public class IceBossAI : MonoBehaviour, IDamage, IHealthUI, ICutscene
{
    [SerializeField] Camera cutsceneCamera;
    [SerializeField] GameObject player;
    [SerializeField] IceAbilityPack moveSet;

    [Header("-----Absolute Zero Components-----")]
    [SerializeField] int absoluteZeroHealthThreashold;
    [SerializeField] float absoluteZeroChargeTime;
    [SerializeField] float absoluteZeroDelayBetweenMoves;
    [SerializeField] float absoluteZeroDelayBetweenCycles;

    bool isAttacking;
    bool isAbsoluteZeroActive;

    Coroutine attackAIRoutine;
    Coroutine currentAttackCoroutine;
    Coroutine absoluteZeroRoutine;

    [SerializeField] int hp;

    int hpOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if (player == null)
       {
           player = GameManager.instance.player;
       }
       if (cutsceneCamera != null)
       {
           SceneFlowManager.instance.cutsceneCamera = cutsceneCamera;
       }

        hpOrig = hp;

        //StartCoroutine(AttackAI());
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0 && !isAttacking)
        {
            StartCoroutine(AttackAI());
        }

        if (hp <= 0)
        {
            return;
        }

        CheckAbsoluteZero();

        if (isAbsoluteZeroActive)
        {
            return;
        }

        if (!isAttacking && attackAIRoutine == null)
        {
            attackAIRoutine = StartCoroutine(AttackAI());
        }
    }

    IEnumerator AttackAI()
    {
        isAttacking = true;
        currentAttackCoroutine = StartCoroutine(moveSet.CastGlacialCrash(transform, player.transform));
        yield return currentAttackCoroutine;
        currentAttackCoroutine = null;
        yield return new WaitForSeconds(25f);
        isAttacking = false;
        attackAIRoutine = null;
    }

    public IEnumerator EndCutscene()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator StartCutscene()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            hp = 0;
            isAttacking = false;

        }

        CheckAbsoluteZero();
    }

    public void UpdateHPUI()
    {
        throw new System.NotImplementedException();
    }

    void CheckAbsoluteZero()
    {
        Debug.Log($"Absolute Zero Check: HP = {hp}, Threshold = {absoluteZeroHealthThreashold}, IsActive = {isAbsoluteZeroActive}");
        if (hp <= absoluteZeroHealthThreashold && !isAbsoluteZeroActive)
        {
            if (currentAttackCoroutine != null)
            {
                StopCoroutine(currentAttackCoroutine);
                currentAttackCoroutine = null;
            }

            if (attackAIRoutine != null)
            {
                StopCoroutine(attackAIRoutine);
                attackAIRoutine = null;
            }

            isAttacking = true;
            isAbsoluteZeroActive = true;

            Debug.Log("Activating Absolute Zero!");
            currentAttackCoroutine = StartCoroutine(AbsoluteZero());
        }
    }

    IEnumerator AbsoluteZero()
    {
        Debug.Log("Absolute Zero routine started.");
        moveSet.FacePlayer(transform, player.transform);

        yield return new WaitForSeconds(absoluteZeroChargeTime);

        while (hp > 0 && isAbsoluteZeroActive)

        { 
            Debug.Log("Absolute Zero cycle started.");
            yield return StartCoroutine(moveSet.CastIcyRain(player.transform));
            yield return new WaitForSeconds(absoluteZeroDelayBetweenMoves);
            if (hp <= 0)
            {
                isAbsoluteZeroActive = false;
                yield break;
            }

            yield return StartCoroutine(moveSet.CastFrostLance(transform, player.transform));
            yield return new WaitForSeconds(absoluteZeroDelayBetweenMoves);
            if (hp <= 0)
            {
                isAbsoluteZeroActive = false;
                yield break;
            }

            yield return StartCoroutine(moveSet.CastBlizzard(player.transform));
            yield return new WaitForSeconds(absoluteZeroDelayBetweenMoves);
            if (hp <= 0)
            {
                isAbsoluteZeroActive = false;
                yield break;
            }

            yield return StartCoroutine(moveSet.CastFrostNova(transform));
            yield return new WaitForSeconds(absoluteZeroDelayBetweenMoves);

            yield return new WaitForSeconds(absoluteZeroDelayBetweenCycles);
        }
    }
}
