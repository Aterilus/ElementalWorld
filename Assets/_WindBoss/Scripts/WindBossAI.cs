using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WindBossAI : MonoBehaviour, IDamage, IHealthUI, ICutscene
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

        StartCoroutine(StartCutscene());
    }

    void Update()
    {
        if (health <= 0 && !bossDied)
        {
            bossDied = true;
            state = AttackState.Death;
            StopAllCoroutines();
            //Play death animation
            StartCoroutine(EndCutscene());
            return;
        }
    }

    /// <summary>
    /// Coroutine that handles the boss's introductory cutscene. The boss descends into the arena, then the cutscene camera is activated while the player's camera and UI are disabled. The boss delivers a series of dialogue lines, and after the dialogue is finished, control is returned to the player and the main camera and UI are reactivated. Finally, the boss's attack loop begins.
    /// </summary>
    /// <returns>An enumerator that performs the introductory cutscene when executed in a coroutine.</returns>
    public IEnumerator StartCutscene()
    {
        SceneFlowManager.instance.cutSceneCamera = cutsceneCamera;

        while (!introStarted)
        {
            yield return StartCoroutine(DescendIntro());

            UIManager.instance.bossHPUI.SetActive(false);
            SceneFlowManager.instance.playerCamera.gameObject.SetActive(false);
            SceneFlowManager.instance.cutSceneCamera.gameObject.SetActive(true);
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

            SceneFlowManager.instance.cutSceneCamera.gameObject.SetActive(false);
            SceneFlowManager.instance.playerCamera.gameObject.SetActive(true);
            UIManager.instance.playerHealthUI.gameObject.SetActive(true);
            UIManager.instance.playerSprintUI.gameObject.SetActive(true);
            UIManager.instance.bossHPUI.SetActive(true);

            yield return new WaitForSeconds(1f);
        }

        agent.enabled = true;

        StartCoroutine(AttackLoop());
    }

    /// <summary>
    /// Coroutine that handles the boss's death sequence. When the boss's health reaches zero, this sequence is triggered. It involves switching to a cutscene camera, disabling player controls and UI, displaying dialogue lines from the boss, and then restoring the player's control and UI after the dialogue is finished. Finally, it switches back to the main camera and ends the cutscene.
    /// </summary>
    /// <returns>An enumerator that performs the death sequence when executed in a coroutine.</returns>
    public IEnumerator EndCutscene()
    {
        SceneFlowManager.instance.cutSceneCamera = cutsceneCamera;

        UIManager.instance.bossHPUI.SetActive(false);
        SceneFlowManager.instance.playerCamera.gameObject.SetActive(false);
        SceneFlowManager.instance.cutSceneCamera.gameObject.SetActive(true);
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

        SceneFlowManager.instance.cutSceneCamera.gameObject.SetActive(false);
        SceneFlowManager.instance.playerCamera.gameObject.SetActive(true);
        UIManager.instance.playerHealthUI.gameObject.SetActive(true);
        UIManager.instance.playerSprintUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);
    }

    /// <summary>
    /// Coroutine that handles the boss's introductory descent into the arena. The boss starts at a specified height above its landing position and smoothly descends down to the landing position over a duration defined by descendSpeed. The method uses linear interpolation (Lerp) to calculate the boss's position at each frame, creating a smooth descending motion. Once the descent is complete, the boss's position is set to the landing position to ensure it ends up exactly where intended.
    /// </summary>
    /// <returns>An enumerator that performs the descent when executed in a coroutine.</returns>
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

    /// <summary>
    /// Main loop for the boss's attack behavior. The boss will continuously check its state and perform actions accordingly. If the boss is idle, it will face the player, wait for a wind-up time, choose a move, execute that move, and then return to idle after a cooldown period. If the boss is attacking or in any other state, it will simply wait briefly before checking again. This loop will continue until the boss's state changes to Death.
    /// </summary>
    /// <returns>An enumerator that performs the attack loop when executed in a coroutine.</returns>
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

    /// <summary>
    /// Randomly selects the next move for the boss to perform, ensuring that it does not repeat the same move consecutively. The method generates a random move from the ChosenMove enum, checks if it's the same as the last move, and if so, selects the next move in the enum (wrapping around if necessary). Finally, it updates the lastMove variable and returns the chosen move.
    /// </summary>
    /// <returns>The next move for the boss to perform.</returns>
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

    /// <summary>
    /// Updates the boss's HP bar UI based on current health. Should be called whenever the boss takes damage or is healed.
    /// </summary>
    public void UpdateHPUI()
    {
        UIManager.instance.bossHPBar.fillAmount = (float)health / hpOrig;
    }
}
