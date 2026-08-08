using UnityEngine;
using UnityEngine.UI;

public class IcePrison : MonoBehaviour, IDamage, IHealthUI
{
    int prisonMaxHP;

    Transform player;

    bool playerTrapped;
    bool hasInitialized;

    int prisonHP;

    public Image hpFillImage;

    private void Start()
    {
        prisonHP = prisonMaxHP;
        UpdateHPUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasInitialized)
        {
            return;
        }
    }

    public void Initialize(int hp, Transform _player)
    {
        prisonMaxHP = hp;
        player = _player;
        playerTrapped = false;
        hasInitialized = true;
    }

    public void TakeDamage(int damage)
    {
        prisonMaxHP -= damage;
        UpdateHPUI();

        if (prisonMaxHP <= 0)
        {
            BreakPrison();
        }
    }

    public void BreakPrison()
    {
        // Logic for breaking the prison, freeing the player, and applying any necessary effects
        Destroy(gameObject);
    }

    public void UpdateHPUI()
    {
        hpFillImage.fillAmount = (float)prisonMaxHP / prisonMaxHP;
    }
}
