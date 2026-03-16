using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("---------Player Components----------")]
    [SerializeField] CharacterController controller;

    [Header("---------Player Stats----------")]
    [SerializeField] int hp;
    [SerializeField] int speed;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMaxTimes;
    [SerializeField] int gravity;

    [Header("---------Player Sprint & Stamina----------")]
    [SerializeField] int sprintMod;
    [SerializeField] int maxStamina;
    [SerializeField] float staminaDrainRate;
    [SerializeField] float staminaRegenRate;
    [SerializeField] float staminaRegenDelay;

    int hpOrig;
    int jumpCount;
    int speedOrig;
    
    float currentStamina;

    Vector3 moveDir;
    Vector3 playerVel;

    bool isSprinting;

    private Coroutine staminaRegenCoroutine;

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
    }

    /// <summary>
    /// Updates the player's movement based on input, applying directional movement, jumping, and gravity effects.
    /// </summary>
    /// <remarks>This method processes horizontal and vertical input to move the player character, applies
    /// jump mechanics, and updates vertical velocity due to gravity. It should be called once per frame to ensure
    /// responsive movement. The player's grounded state is used to reset jump count and vertical velocity.</remarks>
    void Movement()
    {
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        Jump();
        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;
        if (controller.isGrounded)
        {
            playerVel.y = 0;
            jumpCount = 0;
        }

        Sprint();
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

    public void UpdatePlayerSprintUI()
    {
        GameManager.instance.playerSprintBar.fillAmount = currentStamina / maxStamina;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        UpdatePlayerHPUI();

        if (hp <= 0)
        {
            GameManager.instance.Lose();
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
}
