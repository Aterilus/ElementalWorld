using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WindBossAI : MonoBehaviour, IDamage
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject arenaCenter;

    [SerializeField] WindAbilityPack movesSet;
    
    [SerializeField] NavMeshAgent agent;

    [SerializeField] Camera cutsceneCamera;

    [SerializeField] int health;

    [SerializeField] float attackTimer;
    [SerializeField] float windUpTime;
    [SerializeField] float introHeight;
    [SerializeField] float descendSpeed;

    int hpOrig;

    Vector3 landingPosition;
    enum AttackState
    {
        None,
        Attacking,
        Death,
        Idle,
        Recovering
    }
    [SerializeField] AttackState state = AttackState.Idle;

    enum ChosenMove
    {
        None,
        AirBlade,
        Cyclone,
        EyeOfTheStorm,
        HurricaneSlam,
        HurricaneStrike,
        PhantomGust,
        SkyLift,
        WindCurrent
    }
    ChosenMove move = ChosenMove.None;
    ChosenMove lastMove = ChosenMove.None;

    bool isAttacking;
    bool introStarted;
    bool bossDied;

    private void Awake()
    {
        hpOrig = health;
        UpdateHPUI();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if (movesSet == null)
        {
            movesSet = GetComponent<WindAbilityPack>();
        }

        landingPosition = transform.position;
        transform.position = landingPosition + Vector3.up * introHeight;

        if (agent != null)
        {
            agent.enabled = false;
        }

        StartCoroutine(StartingCutScene());
    }

    void Update()
    {
        if (health <= 0 && !bossDied)
        {
            bossDied = true;
            state = AttackState.Death;
            StopAllCoroutines();
            //Play death animation
            StartCoroutine(DeathSequence());
            return;
        }
    }

    IEnumerator StartingCutScene()
    {
        GameManager.instance.cutSceneCamera = cutsceneCamera;

        while (!introStarted)
        {
            yield return StartCoroutine(DescendIntro());

            GameManager.instance.bossHPUI.SetActive(false);
            GameManager.instance.playerCamera.gameObject.SetActive(false);
            GameManager.instance.cutSceneCamera.gameObject.SetActive(true);
            GameManager.instance.playerHealthUI.gameObject.SetActive(false);
            GameManager.instance.playerSprintUI.gameObject.SetActive(false);

            PlayerController playerControls = player.GetComponent<PlayerController>();
            if (playerControls != null)
            {
                playerControls.enabled = false;


                GameManager.instance.dialogue.gameObject.SetActive(true);

                GameManager.instance.dialogue.text = "Meire mortal....";

                yield return new WaitForSeconds(3f);

                GameManager.instance.dialogue.text = "You've come to die in the realm of skies...";

                yield return new WaitForSeconds(3f);

                GameManager.instance.dialogue.text = "THEN DIEEEEEEEE!!!!!!!!!!!";

                yield return new WaitForSeconds(3f);

                GameManager.instance.dialogue.gameObject.SetActive(false);
                playerControls.enabled = true;
            }

            GameManager.instance.cutSceneCamera.gameObject.SetActive(false);
            GameManager.instance.playerCamera.gameObject.SetActive(true);
            GameManager.instance.playerHealthUI.gameObject.SetActive(true);
            GameManager.instance.playerSprintUI.gameObject.SetActive(true);
            GameManager.instance.bossHPUI.SetActive(true);

            yield return new WaitForSeconds(1f);
        }

        agent.enabled = true;

        StartCoroutine(AttackLoop());
    }
    IEnumerator DeathSequence()
    {
        GameManager.instance.cutSceneCamera = cutsceneCamera;

        GameManager.instance.bossHPUI.SetActive(false);
        GameManager.instance.playerCamera.gameObject.SetActive(false);
        GameManager.instance.cutSceneCamera.gameObject.SetActive(true);
        GameManager.instance.playerHealthUI.gameObject.SetActive(false);
        GameManager.instance.playerSprintUI.gameObject.SetActive(false);

        PlayerController playerControls = player.GetComponent<PlayerController>();
        if (playerControls != null)
        {
            playerControls.enabled = false;


            GameManager.instance.dialogue.gameObject.SetActive(true);

            GameManager.instance.dialogue.text = "ME!!?!?!?!?!";

            yield return new WaitForSeconds(3f);

            GameManager.instance.dialogue.text = "LOSE TO A MORTAL!?!?!?!??!?!!!";

            yield return new WaitForSeconds(3f);

            GameManager.instance.dialogue.text = "Thanks for freeing me from the mind control....";

            yield return new WaitForSeconds(3f);

            GameManager.instance.dialogue.gameObject.SetActive(false);
            playerControls.enabled = true;
        }

        GameManager.instance.cutSceneCamera.gameObject.SetActive(false);
        GameManager.instance.playerCamera.gameObject.SetActive(true);
        GameManager.instance.playerHealthUI.gameObject.SetActive(true);
        GameManager.instance.playerSprintUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);
    }

    IEnumerator DescendIntro()
    {
        introStarted = true;

        Vector3 startPos = transform.position;
        float timer = 0f;

        while (timer < descendSpeed)
        {
            transform.position = Vector3.Lerp(startPos, landingPosition, timer / descendSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = landingPosition;
    }

    IEnumerator AttackLoop()
    {
        while (state != AttackState.Death)
        {
            if (state == AttackState.Idle)
            {
                state = AttackState.Attacking;
                isAttacking = true;
                FacePlayer();
                yield return new WaitForSeconds(windUpTime);

                move = ChooseNextMove();

                switch (move)
                {
                    case ChosenMove.None:
                        break;
                    case ChosenMove.AirBlade:
                        yield return movesSet.CastAirBlade(transform);
                        break;
                    case ChosenMove.Cyclone:
                        yield return movesSet.CastCyclone(transform, player.transform);
                        break;
                    case ChosenMove.EyeOfTheStorm:
                        yield return movesSet.CastEyeOfTheStorm(transform, player.transform);
                        break;
                    case ChosenMove.HurricaneSlam:
                        yield return movesSet.CastHurricaneSlam(transform, player.transform);
                        break;
                    case ChosenMove.HurricaneStrike:
                        yield return movesSet.CastHurricaneStrike(transform);
                        break;
                    case ChosenMove.PhantomGust:
                        yield return movesSet.CastPhantomGust(transform);
                        break;
                    case ChosenMove.SkyLift:
                        yield return movesSet.CastSkyLift(transform, player.transform);
                        break;
                    case ChosenMove.WindCurrent:
                        yield return movesSet.CastWindCurrent(transform, player.transform);
                        break;
                }

                state = AttackState.Idle;
                yield return new WaitForSeconds(attackTimer);
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    ChosenMove ChooseNextMove()
    {
        
        int moveCount = System.Enum.GetNames(typeof(ChosenMove)).Length;
        move = (ChosenMove)Random.Range(1, moveCount);

        if (move == lastMove)
        {
            move = (ChosenMove)(((int)move % (moveCount - 1)) + 1);
        }

        lastMove = move;
        return move;
    }

    void FacePlayer()
    {
        Vector3 playerDire = player.transform.position - transform.position;
        playerDire.y = 0f;

        if (playerDire.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(playerDire.normalized);
        }

        //Quaternion rot = Quaternion.LookRotation(playerDire);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * facePlayerSpeed);
    }

    //AttackState AttackStateDef()
    //{
    //    switch (state)
    //    {
    //        case AttackState.None:
    //            break;
    //        case AttackState.Attacking:
    //            isAttacking = true;
    //            yield return null;
    //            break;
    //        case AttackState.Death:
    //            isAttacking = false;
    //            StopAllCoroutines();
    //            break;
    //        case AttackState.Idle:
    //            ChooseNextMove(); 
    //            break;
    //        case AttackState.Recovering:
    //            yield return null;
    //            break;
    //    }
    //}

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHPUI();

        if (health <= 0)
        {
            state = AttackState.Death;
            StopAllCoroutines();
            //Play death animation
        }
    }

    public void UpdateHPUI()
    {
        GameManager.instance.bossHPBar.fillAmount = (float)health / hpOrig;
    }
}
