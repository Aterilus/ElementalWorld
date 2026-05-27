using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class LightningBossAI : MonoBehaviour, IDamage, IHealthUI, ICutscene
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject bossVisual;
    [SerializeField] GameObject exitGatePrefab;

    [SerializeField] Transform exitGateSpawnPoint;

    [SerializeField] Camera cutsceneCamera;

    [SerializeField] LightningAbilityPack moveSet;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] float timeBeforeNextAttack;

    [SerializeField] int hp;

    enum MovePool
    {
        NONE,
        ArcSpear,
        EMPPulse,
        IonCrash,
        MagneticPull,
        StaticDetonation,
        StormSurge,
        ThunderStrike,
        VoltageMine
    }

    MovePool currentMove = MovePool.NONE;

    int hpOrig;

    bool IsAttacking;
    bool introStarted;
    bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpOrig = hp;
        UpdateHPUI();

        UIManager.instance.bossHPUI.SetActive(true);

        if (player == null)
        {
            player = GameManager.instance.player;
        }

        StartCoroutine(StartCutscene());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (hp <= 0)
        {

            //Play death animation
            Die();
            return;
        }
    }

    IEnumerator CombatAI()
    {
        yield return new WaitForSeconds(1.3f);

        while (hp > 0)
        {
            if (IsAttacking) 
            {
                yield return null;
                continue;
            }

            ChooseNextMove();


            IsAttacking = true;

            switch (currentMove)
            {
                case MovePool.ArcSpear:
                    yield return StartCoroutine(moveSet.CastArcSpear(transform, player.transform));
                    break;
                case MovePool.EMPPulse:
                    yield return StartCoroutine(moveSet.CastEMPPulse(transform));
                    break;
                case MovePool.IonCrash:
                    yield return StartCoroutine(moveSet.CastIonCrash(transform, player.transform));
                    break;
                case MovePool.MagneticPull:
                    yield return StartCoroutine(moveSet.MagneticPull(transform));
                    break;
                case MovePool.StaticDetonation:
                    if (GameManager.instance.playerIsMarked)
                    {
                        yield return StartCoroutine(moveSet.CastStaticDetonation(player.transform));
                    }
                    else
                    {
                        ChooseNextMove();
                    }
                    break;
                case MovePool.StormSurge:
                    yield return StartCoroutine(moveSet.CastStormSurge());
                    break;
                case MovePool.ThunderStrike:
                    yield return StartCoroutine(moveSet.CastThunderStrike());
                    break;
                case MovePool.VoltageMine:
                    yield return StartCoroutine(moveSet.SpawnVoltageMine());
                    break;
            }

            IsAttacking = false;

            yield return new WaitForSeconds(timeBeforeNextAttack);
        }
    }

    MovePool ChooseNextMove()
    {

        int moveCount = System.Enum.GetNames(typeof(MovePool)).Length;
        currentMove = (MovePool)Random.Range(1, moveCount);

        return currentMove;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        hp -= damage;
        UpdateHPUI();

        if (hp < 0)
        {
            hp = 0;

            //Play death animation
            Die();
        }
    }

    public void UpdateHPUI()
    {
        UIManager.instance.bossHPBar.fillAmount = (float)hp / hpOrig;
    }

    public IEnumerator StartCutscene()
    {
        
        SceneFlowManager.instance.cutsceneCamera = cutsceneCamera;

        while (!introStarted)
        {
            introStarted = true;
            UIManager.instance.bossHPUI.SetActive(false);
            SceneFlowManager.instance.playerCamera.gameObject.SetActive(false);
            SceneFlowManager.instance.cutsceneCamera.gameObject.SetActive(true);
            UIManager.instance.playerHealthUI.gameObject.SetActive(false);
            UIManager.instance.playerSprintUI.gameObject.SetActive(false);

            PlayerController playerControls = player.GetComponent<PlayerController>();
            if (playerControls != null)
            {
                playerControls.enabled = false;


                UIManager.instance.dialogue.gameObject.SetActive(true);

                UIManager.instance.dialogue.text = "Meire mortal....";

                yield return new WaitForSeconds(3f);

                UIManager.instance.dialogue.text = "You've come to die in the realm of skies...";

                yield return new WaitForSeconds(3f);

                UIManager.instance.dialogue.text = "THEN DIEEEEEEEE!!!!!!!!!!!";

                yield return new WaitForSeconds(3f);

                UIManager.instance.dialogue.gameObject.SetActive(false);
                playerControls.enabled = true;
            }

            SceneFlowManager.instance.cutsceneCamera.gameObject.SetActive(false);
            SceneFlowManager.instance.playerCamera.gameObject.SetActive(true);
            UIManager.instance.playerHealthUI.gameObject.SetActive(true);
            UIManager.instance.playerSprintUI.gameObject.SetActive(true);
            UIManager.instance.bossHPUI.SetActive(true);

            yield return new WaitForSeconds(1f);
        }

        agent.enabled = true;

        StartCoroutine(CombatAI());
    }

    public IEnumerator EndCutscene()
    {
        SceneFlowManager.instance.cutsceneCamera = cutsceneCamera;

        UIManager.instance.bossHPUI.SetActive(false);
        SceneFlowManager.instance.playerCamera.gameObject.SetActive(false);
        SceneFlowManager.instance.cutsceneCamera.gameObject.SetActive(true);
        UIManager.instance.playerHealthUI.gameObject.SetActive(false);
        UIManager.instance.playerSprintUI.gameObject.SetActive(false);

        PlayerController playerControls = player.GetComponent<PlayerController>();
        if (playerControls != null)
        {
            playerControls.enabled = false;


            UIManager.instance.dialogue.gameObject.SetActive(true);

            UIManager.instance.dialogue.text = "ME!!?!?!?!?!";

            yield return new WaitForSeconds(3f);

            UIManager.instance.dialogue.text = "LOSE TO A MORTAL!?!?!?!??!?!!!";

            yield return new WaitForSeconds(3f);

            UIManager.instance.dialogue.text = "Thanks for freeing me from the mind control....";

            yield return new WaitForSeconds(3f);

            UIManager.instance.dialogue.gameObject.SetActive(false);
            playerControls.enabled = true;
        }

        SceneFlowManager.instance.cutsceneCamera.gameObject.SetActive(false);
        SceneFlowManager.instance.playerCamera.gameObject.SetActive(true);
        UIManager.instance.playerHealthUI.gameObject.SetActive(true);
        UIManager.instance.playerSprintUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        StopAllCoroutines();

        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        yield return StartCoroutine(EndCutscene());

        yield return new WaitForSeconds(1f);

        if (bossVisual != null)
        {
            bossVisual.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (exitGatePrefab != null && exitGateSpawnPoint != null)
        {
            Instantiate(exitGatePrefab, exitGateSpawnPoint.position, exitGateSpawnPoint.rotation);
        }
    }
}
