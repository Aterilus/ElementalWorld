using UnityEngine;

public class PlayerEVSystem : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    [SerializeField] int currentEVPoints;

    [SerializeField] int healthEV;
    [SerializeField] int attackEV;
    [SerializeField] int defenseEV;
    [SerializeField] int speedEV;
    [SerializeField] int staminaEV;

    [SerializeField] TMPro.TextMeshProUGUI evPointsText;
    [SerializeField] TMPro.TextMeshProUGUI healthEVText;
    [SerializeField] TMPro.TextMeshProUGUI attackEVText;
    [SerializeField] TMPro.TextMeshProUGUI defenseEVText;
    [SerializeField] TMPro.TextMeshProUGUI speedEVText;
    [SerializeField] TMPro.TextMeshProUGUI staminaEVText;

    int healthIncreasePerPoint;
    int attackIncreasePerPoint;
    int defenseIncreasePerPoint;
    int speedIncreasePerPoint;
    int staminaIncreasePerPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        UpdateEVUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveEvPoints(int amount)
    {
        currentEVPoints += amount;
        UpdateEVUI();
    }

    public bool CanSpendEV()
    {
        if (currentEVPoints > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpendEVOnHealth()
    {
        if (!CanSpendEV()) { return; }

        currentEVPoints -= 1;
        healthEV += 1;

        playerController.hp += healthIncreasePerPoint;
        playerController.UpdatePlayerHPUI();
        UpdateEVUI();
    }

    public void SpendEVOnAttack()
    {
        if(!CanSpendEV()) { return; }

        currentEVPoints -= 1;
        attackEV += 1;

        playerController.attackDamage += attackIncreasePerPoint;
        UpdateEVUI();
    }

    public void SpendEVOnDefense()
    {
        if (!CanSpendEV()) { return; }

        currentEVPoints -= 1;
        defenseEV += 1;

        playerController.defense += defenseIncreasePerPoint;
        UpdateEVUI();
    }

    public void SpendEVOnSpeed()
    {
        if (!CanSpendEV()) { return; }

        currentEVPoints -= 1;
        speedEV += 1;

        playerController.speed += speedIncreasePerPoint;
        UpdateEVUI();
    }

    public void SpendEVOnStamina()
    {
        if (!CanSpendEV()) { return; }

        currentEVPoints -= 1;
        staminaEV += 1;

        playerController.maxStamina += staminaIncreasePerPoint;
        UpdateEVUI();
    }

    public void UpdateEVUI()
    {
        if (evPointsText != null)
        {
            evPointsText.text = "Current: " + currentEVPoints;
        }
        if (healthEVText != null)
        {
            healthEVText.text = "Health: " + healthEV;
        }
        if (attackEVText != null)
        {
            attackEVText.text = "Attack: " + attackEV;
        }
        if (defenseEVText != null)
        {
            defenseEVText.text = "Defense: " + defenseEV;
        }
        if (speedEVText != null)
        {
            speedEVText.text = "Speed: " + speedEV;
        }
        if (staminaEVText != null)
        {
            staminaEVText.text = "Stamina: " + staminaEV;
        }
    }
}
