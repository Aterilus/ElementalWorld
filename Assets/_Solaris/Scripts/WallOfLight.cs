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

    public void Instialize(SolarisAI owner)
    {
        solaris = owner;
        solarisHealable = owner as IHeal;
    }

    public SolarisAI GetOwner()
    {
        return solaris;
    }

    public void Active(float newShieldHP = -1f)
    {
        isActive = true;
        visualRoot.SetActive(true);
        
        if (newShieldHP > 0f)
        {
            maxShieldHP = newShieldHP;
        }
        currentShieldHP = maxShieldHP;

        if (GameManager.instance.bossShieldBar != null)
        { 
            GameManager.instance.bossShieldUI.SetActive(true);
        }
        UpdateShialdUI();
    }

    public void Deactivate()
    {
        isActive = false;
        visualRoot.SetActive(false);
    }

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

    public void BreakShield()
    {
        visualRoot.SetActive(false);

        isActive = false;

        if (GameManager.instance.bossShieldUI != null)
        {
            GameManager.instance.bossShieldUI.SetActive(false);
        }

        OnShieldBroken?.Invoke();
    }

    void UpdateShialdUI()
    {
        if (GameManager.instance.bossShieldBar != null)
        {
            GameManager.instance.bossShieldBar.fillAmount = (float)currentShieldHP / maxShieldHP;
        }
    }
}
