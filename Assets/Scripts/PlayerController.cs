using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("---------Player Components----------")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [Header("---------Player Stats----------")]
    [SerializeField] public int hp;
    [SerializeField] public int speed;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMaxTimes;
    [SerializeField] int gravity;
    [SerializeField] public int defense;

    [Header("---------Player Sprint & Stamina----------")]
    [SerializeField] int sprintMod;
    [SerializeField] public int maxStamina;
    [SerializeField] float staminaDrainRate;
    [SerializeField] float staminaRegenRate;
    [SerializeField] float staminaRegenDelay;

    [Header("----------Player Attack----------")]
    [SerializeField] public int attackDamage;
    [SerializeField] int attackDistance;
    [SerializeField] float attackRate;
    [SerializeField] float combatDuration;

    [Header("----------Status Effects----------")]
    [SerializeField] float blindFlashSlowAmount;
    [SerializeField] float blindFlashDuration;

    int hpOrig;
    int jumpCount;
    int speedOrig;
    
    float currentStamina;
    float combatTimer;
    float attackTimer;

    Vector3 moveDir;
    Vector3 playerVel;

    bool isSprinting;
    bool isInCombat;
    bool isBlinded;

    private Coroutine staminaRegenCoroutine;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        hpOrig = hp;
        UpdatePlayerHPUI();

        currentStamina = maxStamina;

        speedOrig = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Sprint();
        HandleSprintUI();
        HandleCombatState();
    }

    /// <summary>
    /// Updates the player's movement based on input, applying directional movement, jumping, and gravity effects.
    /// </summary>
    /// <remarks>This method processes horizontal and vertical input to move the player character, applies
    /// jump mechanics, and updates vertical velocity due to gravity. It should be called once per frame to ensure
    /// responsive movement. The player's grounded state is used to reset jump count and vertical velocity.</remarks>
    void Movement()
    {
        attackTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            playerVel.y = 0;
            jumpCount = 0;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speedOrig * Time.deltaTime);

        Jump();
        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Fire1") && attackTimer >= attackRate)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Initiates a jump action for the player if the jump input is pressed and the maximum number of jumps has not been
    /// reached.
    /// </summary>
    /// <remarks>This method increases the player's vertical velocity to perform a jump. It can be called
    /// multiple times if multi-jump is enabled by setting <c>jumpMaxTimes</c> greater than one. The jump will only
    /// occur if the jump input is detected and the current jump count is less than the allowed maximum.</remarks>
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMaxTimes)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }
    }

    /// <summary>
    /// Initiates or updates the player's sprinting state based on input and stamina.
    /// </summary>
    /// <remarks>Sprinting increases the player's movement speed while the sprint button is held and
    /// sufficient stamina is available. Stamina is consumed during sprinting and regenerates after a delay when
    /// sprinting stops or stamina is depleted. Calling this method repeatedly (typically once per frame) ensures the
    /// sprint state and stamina are updated in real time.</remarks>
    void Sprint()
    {
        bool sprintButtonHeld = Input.GetButton("Sprint");

        if (sprintButtonHeld && currentStamina > 0)
        {
            isSprinting = true;
            speedOrig = speed * sprintMod;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            if (staminaRegenCoroutine != null)
            {
                StopCoroutine(staminaRegenCoroutine);
                staminaRegenCoroutine = null;
            }

            if (currentStamina <= 0)
            {
                isSprinting = false;
                speedOrig = speed;

                if (staminaRegenCoroutine == null)
                {
                    staminaRegenCoroutine = StartCoroutine(RechargeStaminaAfterDelay(staminaRegenDelay));
                }
            }
        }
        else
        {
            isSprinting = false;
            speedOrig = speed;

            if (currentStamina < maxStamina && staminaRegenCoroutine == null)
            {
                staminaRegenCoroutine = StartCoroutine(RechargeStaminaAfterDelay(staminaRegenDelay));
            }
        }

        UpdatePlayerSprintUI();
    }

    /// <summary>
    /// Waits for a specified delay, then gradually restores the player's stamina over time until it reaches the maximum
    /// value.
    /// </summary>
    /// <remarks>This method is intended to be used with Unity's coroutine system. After the specified delay,
    /// stamina is regenerated incrementally each frame until fully restored.</remarks>
    /// <param name="delay">The time, in seconds, to wait before starting stamina regeneration. Must be non-negative.</param>
    /// <returns>An enumerator that performs the delayed stamina recharge operation when executed in a coroutine.</returns>
    private IEnumerator RechargeStaminaAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            UpdatePlayerSprintUI();
            yield return null;
        }
        staminaRegenCoroutine = null;
    }

    /// <summary>
    /// Updates the player's sprint UI to reflect the current stamina level.
    /// </summary>
    /// <remarks>This method adjusts the sprint bar's fill amount based on the player's current and maximum
    /// stamina. Call this method after any change to the player's stamina to ensure the UI remains accurate.</remarks>
    public void UpdatePlayerSprintUI()
    {
        GameManager.instance.playerSprintBar.fillAmount = (float)currentStamina / maxStamina;
    }

    void Shoot()
    {
        attackTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, attackDistance, ~ignoreLayer))
        {
            WallOfLight wall = hit.collider.GetComponentInParent<WallOfLight>();
            if (wall != null)
            {
                wall.TakeShieldDamage(attackDamage);

                SolarisAI owner = wall.GetOwner();
                if (owner != null)
                {
                    owner.ApplyChipDamageDuringWall(attackDamage);
                }

                return;
            }

            Debug.Log(hit.collider.name);
            IDamage damage = hit.collider.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.TakeDamage(attackDamage);
            }
        }
    }

    /// <summary>
    /// Reduces the player's health points by the specified damage amount.
    /// </summary>
    /// <remarks>If the resulting health points are less than or equal to zero, the player is considered
    /// defeated and the game loss sequence is triggered.</remarks>
    /// <param name="damage">The amount of damage to subtract from the player's current health points. Must be a non-negative value.</param>
    public void TakeDamage(int damage)
    {
        isInCombat = true;
        combatTimer = combatDuration;

        hp -= damage;
        UpdatePlayerHPUI();

        if (hp <= 0)
        {
            if (SceneManager.GetActiveScene().name == "BullManHorde")
            {
                GameManager.instance.LoadNextScene("OpenWorld");
                GameManager.instance.Lose();
            }
        }
    }

    /// <summary>
    /// Updates the player's health bar UI to reflect the current health value.
    /// </summary>
    /// <remarks>This method synchronizes the visual health bar with the player's current health. Call this
    /// method after modifying the player's health to ensure the UI remains accurate.</remarks>
    public void UpdatePlayerHPUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)hp / hpOrig;
    }

    void HandleCombatState()
    {
        if (isInCombat)
        {
            combatTimer -= Time.deltaTime;

            if (combatTimer <= 0)
            {
                isInCombat= false;
            }
        }

        GameManager.instance.playerHealthUI.SetActive(isInCombat);
    }

    void HandleSprintUI()
    {
        if (isSprinting || currentStamina < maxStamina)
        {
            GameManager.instance.playerSprintUI.SetActive(true);
        }
        else
        {
            GameManager.instance.playerSprintUI.SetActive(false);
        }
    }

    public void ApplyBlindFlash(float duration, float slowMultiplier)
    {
        StartCoroutine(BlindFlashRoutine(duration, slowMultiplier));
    }

    IEnumerator BlindFlashRoutine(float duration, float slowMultiplier)
    {
        if (isBlinded) { yield break; }

        isBlinded = true;

        float originalSpeed = speed;
        speed = Mathf.RoundToInt(speed * slowMultiplier);

        GameManager.instance.blindFlashOverlay.gameObject.SetActive(true);
        Color overlayColor = GameManager.instance.blindFlashOverlay.color;
        overlayColor.a = 0.75f;
        GameManager.instance.blindFlashOverlay.color = overlayColor;

        yield return new WaitForSeconds(duration);

        speed = (int)originalSpeed;

        overlayColor.a = 0f;
        GameManager.instance.blindFlashOverlay.color = overlayColor;
        GameManager.instance.blindFlashOverlay.gameObject.SetActive(false);

        isBlinded = false;
    }
}
