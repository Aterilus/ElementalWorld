using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaterBossAI : MonoBehaviour, IDamage, IHealthUI, ICutscene
{
    [SerializeField] NavMeshAgent agent;

    [SerializeField] WaterAbilityPack moveSet;

    [SerializeField] Camera cutsceneCamera;

    [SerializeField] GameObject player;
    [SerializeField] GameObject hpRoot;
    [SerializeField] GameObject bossVisual;
    [SerializeField] GameObject exitGatePrefab;

    [SerializeField] Image hpFillImage;

    [SerializeField] Transform exitGateSpawnPoint;

    [SerializeField] float timerBetweenAttacks;

    [Header("Boss Repositioning")]
    [SerializeField] float repositionRadius;
    [SerializeField] float minDistanceFromPlayer;
    [SerializeField] float repositionTimeout;
    [SerializeField] float timeBetweenAttacks;

    [SerializeField] int hp;
    [SerializeField] int hpMirageSplit;

    enum ChooseAttack
    {
        NONE,
        AcidWave,
        DepthCharge,
        HydroSnipe,
        MirageSplit,
        PhaseSwim,
        TidalPull,
        WaterBubbleShield,
        WaterPrison
    }
    ChooseAttack attackChoice = ChooseAttack.NONE;

    int hpOrig;
    int hpMS;

    bool isAttacking;
    bool mirageSplitActive;
    bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        hpOrig = hp;
        UpdateHPUI();

        hpMS = hpMirageSplit;
        UpdateMirageSplitHPUI();

        if (hpRoot != null)
        {
            hpRoot.SetActive(false);
        }

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
        
    }

    /// <summary>
    /// Main combat loop for the boss. The boss will wait for a short delay before starting the combat, then continuously check if it can attack the player. If the boss is not currently attacking and the player is present, it will set the isAttacking flag to true, reposition itself around the player, choose an attack to perform, wait for a specified time between attacks, and then set isAttacking back to false. This loop will continue until the boss's HP reaches 0 or below, at which point it will stop all coroutines and end its behavior.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    [System.Obsolete]
    IEnumerator CombatLoop()
    {
        yield return new WaitForSeconds(1.5f);
        
        while (hp > 0)
        {
            if (!isAttacking && player != null)
            {
                isAttacking = true;
                yield return StartCoroutine(RepositionBoss());
                yield return StartCoroutine(ChooseMove());
                yield return new WaitForSeconds(timerBetweenAttacks);
                isAttacking = false;
            }

            yield return null;
        }
    }

    public IEnumerator StartCutscene()
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

            UIManager.instance.dialogue.text = "Welcome Human";

            yield return new WaitForSeconds(3f);

            UIManager.instance.dialogue.text = "The tides will consume you...";

            yield return new WaitForSeconds(3f);

            UIManager.instance.dialogue.text = "NOW DIE MORTAL!!!!!!!";

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


        agent.enabled = true;

        StartCoroutine(CombatLoop());
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

            UIManager.instance.dialogue.text = "A MORTAL...";

            yield return new WaitForSeconds(3f);

            UIManager.instance.dialogue.text = "WASHED AWAY MY WAVES!?!?!?!";

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
    
    [System.Obsolete]
    IEnumerator ChooseMove()
    {
        attackChoice = ChooseNextMove();

        switch (Random.Range(0, 8))
        {
            case 0:
                attackChoice = ChooseAttack.AcidWave;
                yield return StartCoroutine(moveSet.CastAcidWave(transform, player.transform));
                break;
            case 1:
                attackChoice = ChooseAttack.DepthCharge;
                yield return StartCoroutine(moveSet.CastDepthCharge(player.transform));
                break;
            case 2:
                attackChoice = ChooseAttack.HydroSnipe;
                yield return StartCoroutine(moveSet.CastHydroSnipe(transform, player.transform));
                break;
            case 3:
                attackChoice = ChooseAttack.MirageSplit;
                ActivateMirageSplitHPUI();
                yield return StartCoroutine(moveSet.CastMirageSplit(transform, player.transform));
                break;
            case 4:
                attackChoice = ChooseAttack.PhaseSwim;
                yield return StartCoroutine(moveSet.CastPhaseSwim(transform, player.transform));
                break;
            case 5:
                attackChoice = ChooseAttack.TidalPull;
                yield return StartCoroutine(moveSet.CastTidalPull(transform, player.transform));
                break;
            case 6:
                attackChoice = ChooseAttack.WaterBubbleShield;
                yield return StartCoroutine(moveSet.CastWaterBubbleShield(transform, player.transform));
                break;
            case 7:
                attackChoice = ChooseAttack.WaterPrison;
                yield return StartCoroutine(moveSet.CastWaterPrison(transform, player.transform));
                break;
        }
    }

    /// <summary>
    /// Randomly selects the next attack for the boss to perform. It calculates the total number of available attacks based on the ChooseAttack enum, then randomly selects one of the attacks (excluding NONE) and returns it as the next move for the boss to execute.
    /// </summary>
    /// <returns>The next attack for the boss to perform.</returns>
    ChooseAttack ChooseNextMove()
    {

        int moveCount = System.Enum.GetNames(typeof(ChooseAttack)).Length;
        attackChoice = (ChooseAttack)Random.Range(1, moveCount);

        return attackChoice;
    }

    /// <summary>
    /// Repositions the boss to a new location around the player before each attack. This method attempts to find a valid position on the NavMesh within a specified radius around the player, ensuring that the new position is not too close to the player. If a valid position is found, the boss will move to that position and face the player. The method includes a timeout to prevent the boss from getting stuck if it cannot reach the new position within a reasonable time frame.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator RepositionBoss()
    {
        if (agent == null || player == null)
        {
            yield break;
        }

        Vector3 newPosition = transform.position;
        bool foundPosition = false;

        for (int i = 0; i < 10; i++)
        {
            Vector3 randOffset = Random.insideUnitSphere * repositionRadius;
            randOffset.y = 0f;

            Vector3 candidatePos = player.transform.position + randOffset;

            float distanceToPlayer = Vector3.Distance(candidatePos, player.transform.position);

            if (distanceToPlayer < minDistanceFromPlayer)
            {
                continue;
            }

            NavMeshHit hit;
            if (NavMesh.SamplePosition(candidatePos, out hit, repositionRadius, NavMesh.AllAreas))
            {
                newPosition = hit.position;
                foundPosition = true;
                break;
            }
        }

        if (!foundPosition)
        {
            yield break;
        }

        agent.isStopped = false;
        agent.SetDestination(newPosition);

        float elapsedTime = 0f;

        while (elapsedTime < repositionTimeout)
        {
            if (agent.remainingDistance <= agent.stoppingDistance + 0.5f && !agent.pathPending)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = true;
        FacePlayer();
    }

    /// <summary>
    /// Handles the boss taking damage. If the Mirage Split is active, it reduces the hpMS variable and updates the Mirage Split HP UI. If the hpMS reaches 0, it ends the Mirage Split HP UI and calls EndMirageSplitEarly() on the moveSet to ensure any ongoing Mirage Split behavior is properly terminated. If the Mirage Split is not active, it reduces the main hp variable and updates the main boss HP UI. If the main hp reaches 0, it calls EndMirageSplitEarly() on the moveSet to ensure any ongoing Mirage Split behavior is properly terminated, and stops all coroutines to end the boss's behavior.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        if (mirageSplitActive)
        {
            hpMS -= damage;
            if (hpMS < 0)
            {
                hpMS = 0;
            }

            Debug.Log($"Mirage Split HP: {hpMS}/{hpMirageSplit}");

            UpdateMirageSplitHPUI();

            if (hpMS <= 0)
            {
                EndMirageSplitHPUI();

                if (moveSet != null)
                {
                    moveSet.EndMirageSplitEarly();
                }
            }

            return;
        }

        hp -= damage;

        if (hp < 0)
        {
            hp = 0;
        }
        UpdateHPUI();

        if (hp <= 0)
        {
            if (moveSet != null)
            {
                moveSet.EndMirageSplitEarly();
            }

            Die();
        }
    }

    /// <summary>
    /// Activates the Mirage Split HP UI and deactivates the main boss HP UI. This method is called when the boss uses the Mirage Split ability, allowing the player to see the HP of the Mirage Split instead of the main boss. It sets the mirageSplitActive flag to true, updates the hpMS variable to match the hpMirageSplit value, shows the Mirage Split HP UI, and hides the main boss HP UI. It also calls UpdateMirageSplitHPUI() to ensure the Mirage Split HP bar is updated to reflect the current HP of the Mirage Split.
    /// </summary>
    void ActivateMirageSplitHPUI()
    {
        mirageSplitActive = true;
        hpMS = hpMirageSplit;

        if (hpRoot != null)
        {
            hpRoot.SetActive(true);
        }

        if (UIManager.instance.bossHPUI != null)
        {
            UIManager.instance.bossHPUI.SetActive(false);
        }

        UpdateMirageSplitHPUI();
    }

    /// <summary>
    /// Deactivates the Mirage Split HP UI and reactivates the main boss HP UI. This method is called when the Mirage Split is defeated or when the Mirage Split HP reaches 0, ensuring that the 
    /// player can see the main boss HP again after dealing with the Mirage Split. It sets the mirageSplitActive flag to false, hides the Mirage Split HP UI, and shows the main boss HP UI.
    /// </summary>
    void EndMirageSplitHPUI()
    {
        mirageSplitActive = false;

        if (hpRoot != null)
        {
            hpRoot.SetActive(false);
        }

        if (UIManager.instance.bossHPUI != null)
        {
            UIManager.instance.bossHPUI.SetActive(true);
        }
    }

    /// <summary>
    /// Updates the boss HP UI to reflect the current HP of the boss. This method calculates the fill amount for the boss HP bar based on the current HP (hp) and the original HP (hpOrig), and 
    /// updates the fill amount of the bossHPBar accordingly. It is called whenever the boss takes damage to ensure the UI accurately represents the remaining HP of the boss.
    /// </summary>
    public void UpdateHPUI()
    {
        UIManager.instance.bossHPBar.fillAmount = (float)hp / hpOrig;
    }

    /// <summary>
    /// Updates the Mirage Split HP UI to reflect the current HP of the Mirage Split. This method calculates the fill amount for the Mirage Split HP bar based on the current Mirage Split HP 
    /// (hpMS) and the maximum Mirage Split HP (hpMirageSplit), and updates the fill amount of the hpFillImage accordingly. It is called whenever the Mirage Split takes damage to ensure the UI 
    /// accurately represents the remaining HP of the Mirage Split.
    /// </summary>
    void UpdateMirageSplitHPUI()
    {
        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = (float)hpMS / hpMirageSplit;
        }
    }

    /// <summary>
    /// Rotates the boss to face the player. This method calculates the direction from the boss to the player, ignores any vertical difference, and sets the boss's rotation to look in that direction. It is called after repositioning the boss to ensure it is always facing the player during combat.
    /// </summary>
    void FacePlayer()
    {
        if (player == null) { return; }

        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(dir.normalized);
        }
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

        if (moveSet != null)
        {
            moveSet.EndMirageSplitEarly();
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
