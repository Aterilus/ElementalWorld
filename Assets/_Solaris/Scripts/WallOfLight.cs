using UnityEngine;
using System.Collections;
using System;

public class WallOfLight : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] float healPerSecond;
    
    [SerializeField] GameObject visualRoot;
    
    [Header("Shield Settings")]
    [SerializeField] public float maxShieldHP;

    SolarisAI solaris;

    float currentShieldHP;

    public Action OnShieldBroken;

    Collider shieldCollider;

    bool isActive;

    IHeal solarisHealable;

    private void Awake()
    {
        solarisHealable = solaris as IHeal;
        currentShieldHP = maxShieldHP;
        solaris = GetComponentInParent<SolarisAI>();
    }

    private void Update()
    {
        if (!isActive) return;

        if (solaris != null)
        {
            solaris.Heal((int)(healPerSecond * Time.deltaTime));
        }
    }

    /// <summary>
    /// Initializes the WallOfLight shield with a reference to the owning SolarisAI instance. This method should be called by the SolarisAI script when setting up the shield, allowing the shield to access necessary properties and methods from its owner.
    /// </summary>
    /// <param name="owner">The SolarisAI instance that owns this shield.</param>
    public void Instialize(SolarisAI owner)
    {
        solaris = owner;
        solarisHealable = owner as IHeal;
    }

    /// <summary>
    /// Returns the SolarisAI instance that owns this WallOfLight shield. This can be used by other scripts to access the boss's properties or methods related to the shield.
    /// </summary>
    /// <returns>The SolarisAI instance that owns this shield.</returns>
    public SolarisAI GetOwner()
    {
        return solaris;
    }

    /// <summary>
    /// Activates the shield, enabling its visuals and allowing it to interact with damage and healing. If a new shield HP value is provided, it will reset the shield HP to that value; otherwise, it will reset to the default maxShieldHP.
    /// </summary>
    /// <param name="newShieldHP">The new shield HP value. If not provided or less than 0, the shield will use the default maxShieldHP.</param>
    public void Active(float newShieldHP = -1f)
    {
        isActive = true;
        visualRoot.SetActive(true);
        
        if (newShieldHP > 0f)
        {
            maxShieldHP = newShieldHP;
        }
        currentShieldHP = maxShieldHP;

        if (UIManager.instance.bossShieldBar != null)
        { 
            UIManager.instance.bossShieldUI.SetActive(true);
        }
        UpdateShialdUI();
    }

    /// <summary>
    /// Deactivates the shield, hiding its visuals and stopping any healing or damage interactions. Should be called when the shield is no longer needed or has been broken.
    /// </summary>
    public void Deactivate()
    {
        isActive = false;
        visualRoot.SetActive(false);
    }

    /// <summary>
    /// Applies damage to the shield. If the damage reduces the shield HP to 0 or below, the shield will break and trigger the appropriate effects and events.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeShieldDamage(float damage)
    {
        currentShieldHP -= damage;
        currentShieldHP = Mathf.Max(currentShieldHP, 0f);

        UpdateShialdUI();

        if (currentShieldHP <= 0f)
        {
            BreakShield();
        }
    }

    /// <summary>
    /// Handles the logic for when the shield is broken, such as playing break effects, disabling the shield visuals, and invoking any relevant events or callbacks.
    /// </summary>
    public void BreakShield()
    {
        visualRoot.SetActive(false);

        isActive = false;

        if (UIManager.instance.bossShieldUI != null)
        {
            UIManager.instance.bossShieldUI.SetActive(false);
        }

        OnShieldBroken?.Invoke();
    }

    /// <summary>
    /// Updates the shield UI to reflect the current shield HP. Should be called whenever the shield HP changes.
    /// </summary>
    void UpdateShialdUI()
    {
        if (UIManager.instance.bossShieldBar != null)
        {
            UIManager.instance.bossShieldBar.fillAmount = (float)currentShieldHP / maxShieldHP;
        }
    }
}
