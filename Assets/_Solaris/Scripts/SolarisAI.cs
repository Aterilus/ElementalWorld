using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SolarisAI : MonoBehaviour, IDamage, IHeal, IHealthUI, ICutscene
{
    [SerializeField] int hp;
    [SerializeField] int solarisEVRewards;

    [Header("----------References----------")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject arenaCenter;
    [SerializeField] Camera solarisCutSceneCamera;

    [Header("----------Movement----------")]
    public Transform[] teleportPoints;
    public float teleportTimer;
    public float teleportMaxTimer;

    [Header("----------Flare Components----------")]
    [SerializeField] SolarisFlareTelegraph flareTelegraphPrefab;
    [SerializeField] float flareAttackMaxCooldown;
    [SerializeField] float flareAttackDelayMaxTimer;

    [Header("----------Dagger Components----------")]
    [SerializeField] DaggerProjectile daggerPrefab;
    [SerializeField] Transform daggerSpawnPoint;
    [SerializeField] float daggersMaxCooldown;
    [SerializeField] float daggerDelayMaxTimer;

    [Header("----------Exploding Light Components----------")]
    [SerializeField] ExplodingLight explodingLightprefab;
    [SerializeField] float explodingMaxCooldown;
    [SerializeField] float explodingLightDelayMaxTimer;
    [SerializeField] float exploadingLightRange;
    [SerializeField] float floatingHeight;
    [SerializeField] int minOrbCount;
    [SerializeField] int maxOrbCount;

    [Header("----------Light Beam Components----------")]
    [SerializeField] LineRenderer beamLine;
    [SerializeField] Transform beamSpawnPoint;
    [SerializeField] float beamRange;
    [SerializeField] float beamDuration;
    [SerializeField] float beamDelayMaxTimer;
    [SerializeField] float beamDamage;
    [SerializeField] float beamTickRate;
    [SerializeField] float beamTurnSpeed;
    [SerializeField] float beamMaxCooldown;

    [Header("----------Light Pulse Components----------")]
    [SerializeField] GameObject lightPulseVisuals;
    [SerializeField] float lightPulseRadius;
    [SerializeField] float lightPulseDelay;
    [SerializeField] float lightPulseMaxCooldown;
    [SerializeField] float lightPulseDelayMaxTimer;
    [SerializeField] int lightPulseDamage;

    [Header("----------Blinding Light Components----------")]
    [SerializeField] float blindFlashRange;
    [SerializeField] float blindFlashDuration;
    [SerializeField] float blindFlashMaxCooldown;
    [SerializeField] float blindFlashDelayMaxTimer;
    [SerializeField] float blindFlashSlowMultiplier;

    [Header("----------Solar Rain Components----------")]
    [SerializeField] GameObject solarRainStrikePrefab;
    [SerializeField] int solarRainDropCount;
    [SerializeField] float solarRainRaidusAroundPlayer;
    [SerializeField] float solarRainTimeBetweenDrops;
    [SerializeField] float solarRainMaxCooldown;
    [SerializeField] float solarRainDelayMaxTimer;

    [Header("----------Wall Of Light Components----------")]
    [SerializeField] WallOfLight wallOfLight;
    [SerializeField] float wallOfLightDuration;
    [SerializeField] float wallOfLightHealPerSecond;
    [SerializeField] float wallOfLightShieldHP;
    [Range(0f, 1f)] [SerializeField] float wallDamagePassThroughMultiplier;
    [Range(0f, 1f)] [SerializeField] float wallHealCapPercent;
    
    
    [Header("----------Phase Management----------")]
    [SerializeField] SolarisPhases currentPhase = SolarisPhases.Phase1;
    [Range(0f, 1f)] [SerializeField] float phase2HealthThreshold;
    public enum SolarisPhases
    {
        Phase1,
        Phase2,
        Phase3
    }
    public float phase3DamageMultiplier = 1.5f;

    Color origColor;

    int hpOrig;

    float flareAttackCooldown;
    float daggerAttackCooldown;
    float explodingLightCooldown;
    float beamCooldown;
    float lightPulseCooldown;
    float blindFlashCooldown;
    float solarRainCooldown;

    bool isUsingBeam;
    bool isUsingLightPulse;
    bool isUsingBlindingFlash;
    bool isUsingSolarRain;
    bool wallOfLightActive;
    bool wallPermanentlyDisabled;
    bool fightActive = true;
    bool enterPhase2 = false;

    Coroutine phaseRoutine;

    private void Awake()
    {
        UIManager.instance.bossHPUI.gameObject.SetActive(true);
        if (wallOfLight != null) { wallOfLight.OnShieldBroken += OnWallBroken; }
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player"); }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpOrig = hp;
        UpdateHPUI();

        DeactivateWallOfLight();
        StartPhaseLoop(currentPhase);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fightActive)
        {
            return;
        }

        teleportTimer -= Time.deltaTime;
        flareAttackCooldown -= Time.deltaTime;
        daggerAttackCooldown -= Time.deltaTime;
        explodingLightCooldown -= Time.deltaTime;
        beamCooldown -= Time.deltaTime;
        lightPulseCooldown -= Time.deltaTime;
        blindFlashCooldown -= Time.deltaTime;
        solarRainCooldown -= Time.deltaTime;

        FacePlayer();
        TickTeleport();

        if (hp <= 0)
        {
            OnSolarisDefeated();
            return;
        }

        CheckPhaseTransitions();
    }

    /// =================================
    //  Movement Methods
    /// =================================

    /// <summary>
    /// Rotates the Solaris to face the player's position on the horizontal plane.
    /// </summary>
    /// <remarks>The rotation is applied only if the player is not directly above or below Solaris.
    /// Vertical alignment is ignored; Solaris will face the player based on their relative position in the XZ
    /// plane.</remarks>
    void FacePlayer()
    {
        Vector3 playerDir = player.transform.position - transform.position;
        playerDir.y = 0f;

        if (playerDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(playerDir);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void TickTeleport()
    {
        if (teleportTimer <= 0)
        {
            TeleportToRandomPoint();
            teleportTimer = teleportMaxTimer;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void TeleportToRandomPoint()
    {
        int index = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[index].position;
    }

    /// =================================
    //  HP Methods
    /// =================================
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (currentPhase == SolarisPhases.Phase2 && wallOfLightActive && wallOfLight != null)
        {
            wallOfLight.TakeShieldDamage(damage);
            ApplyChipDamageDuringWall(damage);
            return;
        }

        hp -= damage;
        hp = Mathf.Max(hp, 0);
        UpdateHPUI();

        if (hp < 0)
        {
            OnSolarisDefeated();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="healAmount"></param>
    public void Heal(int healAmount)
    {
        hp = Mathf.Min(hp, hpOrig);
        UpdateHPUI();

        if (hp > hpOrig)
        {
            hp = hpOrig;
        }
    }
    /// =================================
    //  Move Methods
    /// =================================
    
    /// <summary>
    /// 
    /// </summary>
    void ShootFlareAttack()
    {
        SolarisFlareTelegraph flareInstance = Instantiate(flareTelegraphPrefab, player.transform.position, Quaternion.identity);
        flareInstance.TriggerFlare(player.transform);
        flareAttackCooldown = flareAttackMaxCooldown;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    void FireDagger(Transform player)
    {
        DaggerProjectile daggerInstance = Instantiate(daggerPrefab, daggerSpawnPoint.position, daggerSpawnPoint.rotation);

        DaggerProjectile daggerProjectile = daggerInstance.GetComponent<DaggerProjectile>();
        if (daggerInstance != null)
        {
            daggerProjectile.SetTarget(player);
            daggerAttackCooldown = daggersMaxCooldown;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator UseLightPulse()
    {
        isUsingLightPulse = true;
        lightPulseVisuals.SetActive(true);

        float timer = 0f;
        float dur = lightPulseDelay;

        Vector3 startScale = new Vector3(0.1f, 0.025f, 0.1f);
        Vector3 endScale = new Vector3(lightPulseRadius, 0.025f, lightPulseRadius);

        while (timer < dur)
        {
            lightPulseVisuals.transform.localScale = Vector3.Lerp(startScale, endScale, timer / dur);

            timer += Time.deltaTime;
            yield return null;
        }

        lightPulseVisuals.transform.localScale = endScale;

        Collider[] hits = Physics.OverlapSphere(transform.position, lightPulseRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                IDamage dmg = hit.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.TakeDamage(lightPulseDamage);
                }
            }
        }

        yield return new WaitForSeconds(0.25f);

        lightPulseVisuals.SetActive(false);
        lightPulseVisuals.transform.localScale = Vector3.zero;

        lightPulseCooldown = lightPulseMaxCooldown;
        isUsingLightPulse = false;
    }

    /// <summary>
    /// 
    /// </summary>
    void UseExplodingLight()
    {
        int orbCount = Random.Range(minOrbCount, maxOrbCount);

        for (int i = 0; i < orbCount; i++)
        {
            float randX = Random.Range(-exploadingLightRange, exploadingLightRange);
            float randZ = Random.Range(-exploadingLightRange, exploadingLightRange);

            Vector3 offset = new Vector3(randX, 0f, randZ);

            Vector3 spawnPos = player.transform.position + offset;

            RaycastHit hit;
            if (Physics.Raycast(spawnPos + Vector3.up * 5f, Vector3.down, out hit, 10f))
            {
                spawnPos = hit.point;
                spawnPos.y = floatingHeight;
            }

            Instantiate(explodingLightprefab, spawnPos, Quaternion.identity);
        }

        explodingLightCooldown = explodingMaxCooldown;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator UseLightBeam()
    {
        float timer = 0;
        float damageTimer = 0;

        isUsingBeam = true;
        beamLine.enabled = true;

        while (timer < beamDuration)
        {
            Vector3 dire = (player.transform.position - beamSpawnPoint.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dire);

            beamSpawnPoint.rotation = Quaternion.Lerp(beamSpawnPoint.rotation, targetRot, beamTurnSpeed * Time.deltaTime);

            RaycastHit hit;
            Vector3 endPoint;

            if (Physics.Raycast(beamSpawnPoint.position, beamSpawnPoint.forward, out hit, beamRange))
            {
                endPoint = hit.point;

                damageTimer += Time.deltaTime;

                if ( damageTimer >= beamTickRate)
                {
                    IDamage dmg = hit.collider.GetComponentInParent<IDamage>();
                    if (dmg != null)
                    {
                        dmg.TakeDamage((int)beamDamage);
                    }

                    damageTimer = 0;
                }
            }
            else
            {
                endPoint = beamSpawnPoint.position + beamSpawnPoint.forward * beamRange;
            }

            beamLine.SetPosition(0, beamSpawnPoint.position);
            beamLine.SetPosition(1, endPoint);

            timer += Time.deltaTime;
            yield return null;
        }

        beamCooldown = beamMaxCooldown;
        beamLine.enabled = false;
        isUsingBeam = false;
    }

    /// <summary>
    /// 
    /// </summary>
    void UseBlindingLight()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist <= blindFlashRange)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ApplyBlindFlash(blindFlashDuration, blindFlashSlowMultiplier);
            }
        }

        blindFlashCooldown = blindFlashMaxCooldown;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator UseSolarRain()
    {
        isUsingSolarRain = true;

        for (int i = 0; i < solarRainDropCount; i++)
        {
            Vector3 randOffset = new Vector3(Random.Range(-solarRainRaidusAroundPlayer, solarRainRaidusAroundPlayer), 0f, Random.Range(-solarRainRaidusAroundPlayer, solarRainRaidusAroundPlayer));

            Vector3 spawnPos = player.transform.position + randOffset;

            RaycastHit hit;
            if (Physics.Raycast(spawnPos + Vector3.up * 5f, Vector3.down, out hit, 10f))
            {
                spawnPos = hit.point;
            }

            Instantiate(solarRainStrikePrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(solarRainTimeBetweenDrops);
        }

        solarRainCooldown = solarRainMaxCooldown;
        isUsingSolarRain = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator TeleportToCenterThenUseLightPulse()
    {
        transform.position = arenaCenter.transform.position;

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(UseLightPulse());

        yield return new WaitForSeconds(0.5f);
    }
    /// =================================
    //  Wall of Light Methods
    /// =================================


    /// <summary>
    /// 
    /// </summary>
    void ActivateWallOfLight()
    {
        if (wallPermanentlyDisabled) { return; }

        wallOfLightActive = true;

        if (wallOfLight != null)
        {
            wallOfLight.Instialize(this);
            wallOfLight.maxShieldHP = wallOfLightShieldHP;
            wallOfLight.Active(wallOfLightShieldHP);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void DeactivateWallOfLight()
    {
        wallOfLightActive = false;

        if (wallOfLight != null)
        {
            wallOfLight.Deactivate();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnWallBroken()
    {
        EnterPhase3();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="incomingDamage"></param>
    public void ApplyChipDamageDuringWall(float incomingDamage)
    {
        float chipDamage = incomingDamage * wallDamagePassThroughMultiplier;
        if (chipDamage <= 0f) { return; }

        hp -= Mathf.RoundToInt(chipDamage);
        hp = Mathf.Max(hp, 0);
        UpdateHPUI();

        if (hp < 0f)
        {
            OnSolarisDefeated();
        }
    }
    /// =================================
    //  Misc. Methods
    /// =================================

    /// <summary>
    /// 
    /// </summary>
    void OnSolarisDefeated()
    {
        fightActive = false;

        if (phaseRoutine != null)
        {
            StopCoroutine(phaseRoutine);
            phaseRoutine = null;
        }

        PlayerEVSystem playerEV = player.gameObject.GetComponent<PlayerEVSystem>();
        if (playerEV != null)
        {
            playerEV.GiveEvPoints(solarisEVRewards);
        }
        StopAllCoroutines();
        StartCoroutine(EndCutscene());
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdateHPUI()
    {
        UIManager.instance.bossHPBar.fillAmount = (float)hp / hpOrig;
    }

    public IEnumerator StartCutscene()
    {
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndCutscene()
    {
        SceneFlowManager.instance.cutSceneCamera = solarisCutSceneCamera;

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

            UIManager.instance.dialogue.text = "You have proven yourself worthy";

            yield return new WaitForSeconds(4f);

            UIManager.instance.dialogue.text = "Follow me to the safety amidst the darkness";

            yield return new WaitForSeconds(3f);

            UIManager.instance.dialogue.text = "";
            UIManager.instance.dialogue.gameObject.SetActive(false);
            playerControls.enabled = true;
        }

        SceneFlowManager.instance.cutSceneCamera.gameObject.SetActive(false);
        SceneFlowManager.instance.playerCamera.gameObject.SetActive(true);
        UIManager.instance.playerHealthUI.gameObject.SetActive(true);
        UIManager.instance.playerSprintUI.gameObject.SetActive(true);
        SceneFlowManager.instance.LoadNextScene("RefugeTown");
    }
    /// =================================
    //  Phase Methods
    /// =================================
    
    /// <summary>
    /// 
    /// </summary>
    void CheckPhaseTransitions()
    {
        float phase2HP = hpOrig * phase2HealthThreshold;

        if (currentPhase == SolarisPhases.Phase1 && hp <= phase2HP)
        {
            EnterPhase2();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void EnterPhase2()
    {
        enterPhase2 = true;
        currentPhase = SolarisPhases.Phase2;

        StartPhaseLoop(SolarisPhases.Phase2);
    }

    /// <summary>
    /// 
    /// </summary>
    void EnterPhase3()
    {
        currentPhase = SolarisPhases.Phase3;

        wallPermanentlyDisabled = true;
        DeactivateWallOfLight();

        StartPhaseLoop(SolarisPhases.Phase3);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phase"></param>
    void StartPhaseLoop(SolarisPhases phase)
    {
        if (phaseRoutine != null)
        {
            StopCoroutine(phaseRoutine);
        }
        phaseRoutine = StartCoroutine(PhaseLoop(phase));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phase"></param>
    /// <returns></returns>
    IEnumerator PhaseLoop(SolarisPhases phase)
    {
        switch (phase)
        {
            case SolarisPhases.Phase1:
                yield return StartCoroutine(Phase1Loop());
                break;
            case SolarisPhases.Phase2:
                yield return StartCoroutine(Phase2Loop());
                break;
            case SolarisPhases.Phase3:
                yield return StartCoroutine(Phase3Loop());
                break;
            default:
                break;

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator Phase1Loop()
    {
        while (fightActive && currentPhase == SolarisPhases.Phase1)
        {
            yield return new WaitForSeconds(5f);

            if (flareTelegraphPrefab != null && flareAttackCooldown <= 0)
            {
                ShootFlareAttack();
            }

            yield return new WaitForSeconds(flareAttackDelayMaxTimer);

            if (daggerPrefab != null && daggerAttackCooldown <= 0)
            {
                FireDagger(player.transform);
            }

            yield return new WaitForSeconds(daggerDelayMaxTimer);

            if (!isUsingLightPulse && lightPulseCooldown <= 0)
            {
                StartCoroutine(TeleportToCenterThenUseLightPulse());
            }

            yield return new WaitForSeconds(lightPulseDelayMaxTimer);
        }
    }

    /// <summary>
    /// In Phase 2, Solaris summons a protective Wall of Light that absorbs damage and heals him over time. He also starts using the Exploding Light and Light Beam attacks more frequently, while still occasionally using the Blinding Flash to disorient the player. The Wall of Light adds a new layer of strategy for the player, as they must decide when to focus on damaging Solaris directly and when to try to break through the wall's defenses.
    /// </summary>
    /// <returns></returns>
    IEnumerator Phase2Loop()
    {
        ActivateWallOfLight();

        while (fightActive && currentPhase == SolarisPhases.Phase2)
        {
            if (!wallOfLightActive) { yield break; }
            if (explodingLightprefab != null && explodingLightCooldown <= 0)
            {
                UseExplodingLight();
            }

            yield return new WaitForSeconds(explodingLightDelayMaxTimer);

            if (beamLine != null && beamCooldown <= 0 && !isUsingBeam)
            {
                StartCoroutine(UseLightBeam());
            }

            yield return new WaitForSeconds(beamDelayMaxTimer);

            if (blindFlashCooldown <= 0 && !isUsingBlindingFlash)
            {
                UseBlindingLight();
            }

            yield return new WaitForSeconds(blindFlashDelayMaxTimer);
        }
    }

    /// <summary>
    /// In Phase 3, Solaris becomes more aggressive and uses all of his abilities without the protection of the Wall of Light.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator Phase3Loop()
    {
        while (fightActive && currentPhase == SolarisPhases.Phase3)
        {
            if (blindFlashCooldown <= 0 && !isUsingBlindingFlash)
            {
                UseBlindingLight();
            }

            yield return new WaitForSeconds(blindFlashDelayMaxTimer);

            if (solarRainStrikePrefab != null && solarRainCooldown <= 0 && !isUsingSolarRain)
            {
                StartCoroutine(UseSolarRain());
            }

            yield return new WaitForSeconds(solarRainDelayMaxTimer);

            if (beamLine != null && beamCooldown <= 0 && !isUsingBeam)
            {
                StartCoroutine(UseLightBeam());
            }

            yield return new WaitForSeconds(beamDelayMaxTimer);

            if (!isUsingLightPulse && lightPulseCooldown <= 0)
            {
                StartCoroutine(TeleportToCenterThenUseLightPulse());
            }

            yield return new WaitForSeconds(lightPulseDelayMaxTimer);

            if (explodingLightprefab != null && explodingLightCooldown <= 0)
            {
                UseExplodingLight();
            }

            yield return new WaitForSeconds(explodingLightDelayMaxTimer);
        }
    }
}
