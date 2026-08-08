using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IceAbilityPack : MonoBehaviour
{
    [Header("-----Ice Spikes Components-----")]
    [SerializeField] GameObject iceSpikePrefab;
    [SerializeField] Transform iceSpikesSpawnPoint;
    [SerializeField] float spikesSpacing;
    [SerializeField] float spikeFreezeDuration;
    [SerializeField] float spikeMoveSpeed;
    [SerializeField] float spikeSpreadSpeed;
    [SerializeField] float spikeLifetime;
    [SerializeField] float iceSpikesDelay;
    [SerializeField] int spikesDamage;

    [Header("-----Frost Nova Components-----")]
    [SerializeField] GameObject frostNovaPrefab;
    [SerializeField] int frostNovaDamage;
    [SerializeField] float frostNovaFreezeDuration;
    [SerializeField] float frostNovaExpandTime;
    [SerializeField] float frostNovaMaxScale;
    [SerializeField] float frostNovaJumpClearance;
    [SerializeField] float frostNovaDelay;

    [Header("-----Frost Lance Components-----")]
    [SerializeField] GameObject frostLancePrefab;
    [SerializeField] int frostLanceDamage;
    [SerializeField] int frostLanceCount;
    [SerializeField] float frostLanceFreezeDuration;
    [SerializeField] float frostLanceSpeed;
    [SerializeField] float frostLanceLifeTime;
    [SerializeField] float frostLanceTrackingStrength;
    [SerializeField] float frostLanceBackOffset;
    [SerializeField] float frostLanceHeightOffset;
    [SerializeField] float frostLanceSpacing;
    [SerializeField] float frostLanceChargeTime;
    [SerializeField] float frostLanceDelayBetweenShots;

    [Header("-----Ice Prison Components-----")]
    [SerializeField] GameObject icePrisonPrefab;
    [SerializeField] int icePrisonHP;
    [SerializeField] float icePrisonDelay;

    [Header("-----Blizzard Components-----")]
    [SerializeField] GameObject blizzardPrefab;
    [SerializeField] GameObject[] teleportPoints;
    [SerializeField] GameObject arenaCenter;
    [SerializeField] float blizzardDelay;
    [SerializeField] float blizzardLifeTime;
    [SerializeField] float blizzardRotateSpeed;
    [SerializeField] float blizzardExpandSpeed;
    [SerializeField] float blizzardFreezeDuration;
    [SerializeField] float blizzardTickRate;
    [SerializeField] float blizzardDamageTimer;
    [SerializeField] int blizzardDamage;

    [Header("-----Rain of Icicles Components-----")]
    [SerializeField] GameObject rainOfIciclesPrefab;
    [SerializeField] GameObject icicleWarningPrefab;
    [SerializeField] float icicleSpawnRadius;
    [SerializeField] float icicleWarningDuration;
    [SerializeField] float icicleWarningLifeTime;
    [SerializeField] float icicleFreezeDuration;
    [SerializeField] float icicleFallSpeed;
    [SerializeField] float icicleLifeTime;
    [SerializeField] int icicleSpawnHeight;
    [SerializeField] int icicleCount;
    [SerializeField] int icicleDamage;

    [Header("-----Glacial Crash Components-----")]
    [SerializeField] GameObject glacialCrashPrefab;
    [SerializeField] GameObject glacialCrashChargePrefab;
    [SerializeField] Transform glacialCrashSpawnPoint;
    [SerializeField] int glacialCrashIcicleCount;
    [SerializeField] int glacialCrashDamage;
    [SerializeField] float glacialCrashRiseHeight;
    [SerializeField] float glacialCrashRiseSpeed;
    [SerializeField] float glacialCrashChargeTime;
    [SerializeField] float glacialCrashSpreadRadius;
    [SerializeField] float glacialCrashFreezeDuration;
    [SerializeField] float glacialCrashIcicleSpeed;
    [SerializeField] float glacialCrashIcicleLifeTime;
    [SerializeField] float glacialCrashReleaseTime;    

    [Header("-----Frozen Ground Components-----")]
    [SerializeField] GameObject frozenGroundPrefab;
    [SerializeField] float frozenGroundExpandSpeed;
    [SerializeField] float frozenGroundMaxScale;
    [SerializeField] float frozenGroundLifeTime;
    [SerializeField] float frozenGroundFreezeDuration;


    bool hasInitialized;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CastIceSpikes(Transform caster, Transform target)
    {
        yield return new WaitForSeconds(iceSpikesDelay);

        Vector3 dire = target.position - caster.position;
        dire.y = 0;
        dire.Normalize();

        FacePlayer(caster, target);

        Vector3 bossForward = iceSpikesSpawnPoint.position;
        Vector3 bossRight = bossForward + caster.right * spikesSpacing;
        Vector3 bossLeft = bossForward - caster.right * spikesSpacing;

        GameObject leftSpike = Instantiate(iceSpikePrefab, bossLeft, caster.rotation);
        GameObject centerSpike = Instantiate(iceSpikePrefab, bossForward, caster.rotation);
        GameObject rightSpike = Instantiate(iceSpikePrefab, bossRight, caster.rotation);

        IceSpikeWave ls = leftSpike.GetComponent<IceSpikeWave>();
        if (ls != null)
        {
            ls.Initialize(spikesDamage, spikeLifetime, spikeMoveSpeed, spikeSpreadSpeed, spikeFreezeDuration, dire, -caster.right);
        }
        IceSpikeWave cs = centerSpike.GetComponent<IceSpikeWave>();
        if (cs != null)
        {
            cs.Initialize(spikesDamage, spikeLifetime, spikeMoveSpeed, spikeSpreadSpeed, spikeFreezeDuration, dire, Vector3.zero);
        }
        IceSpikeWave rs = rightSpike.GetComponent<IceSpikeWave>();
        if (rs != null)
        {
            rs.Initialize(spikesDamage, spikeLifetime, spikeMoveSpeed, spikeSpreadSpeed, spikeFreezeDuration, dire, caster.right);
        }
    }

    public IEnumerator CastFrostNova(Transform caster)
    {
        yield return new WaitForSeconds(frostNovaDelay);
        GameObject nova = Instantiate(frostNovaPrefab, caster.position, Quaternion.identity);
        FrostNova fn = nova.GetComponent<FrostNova>();
        if (fn != null)
        {
            fn.Initialize(frostNovaDamage, frostNovaFreezeDuration, frostNovaExpandTime, frostNovaMaxScale, frostNovaJumpClearance);
        }
    }

    public IEnumerator CastFrostLance(Transform caster, Transform target)
    {
        FacePlayer(caster, target);

        FrostLance[] lances = new FrostLance[frostLanceCount];

        float startOffset = -(frostLanceCount - 1) / 2f;

        for (int i = 0; i < frostLanceCount; i++)
        {
            float sideOffset = startOffset + i;

            Vector3 spawnPos = caster.position;
            spawnPos -= caster.forward * frostLanceBackOffset;
            spawnPos += caster.right * sideOffset * frostLanceSpacing;
            spawnPos += Vector3.up * frostLanceHeightOffset;

            GameObject lanceOnj = Instantiate(frostLancePrefab, spawnPos, caster.rotation);

            FrostLance lance = lanceOnj.GetComponent<FrostLance>();
            if (lance != null)
            {
                lance.Initialize(frostLanceDamage, frostLanceFreezeDuration, frostLanceSpeed, frostLanceLifeTime, frostLanceTrackingStrength, target);

                lances[i] = lance;
            }
        }

        yield return new WaitForSeconds(frostLanceChargeTime);

        for (int i = 0; i < lances.Length; i++)
        {
            if (lances[i] != null)
            {
                lances[i].Fire();
            }

            yield return new WaitForSeconds(frostLanceDelayBetweenShots);
        }
    }

    public IEnumerator CastIcePrison(Transform target)
    {
        yield return new WaitForSeconds(icePrisonDelay);

        GameObject icePrison = Instantiate(icePrisonPrefab, target.position, Quaternion.identity);
        IcePrison ip = icePrison.GetComponent<IcePrison>();
        if (ip != null)
        {
            ip.Initialize(icePrisonHP, target);
        }
    }

    public IEnumerator CastBlizzard(Transform target)
    {
        GameObject blizzard = Instantiate(blizzardPrefab, arenaCenter.transform.position, Quaternion.identity);
        Blizzard b = blizzard.GetComponent<Blizzard>();
        if (b != null)
        {
            b.Initialize(blizzardLifeTime, blizzardRotateSpeed, blizzardExpandSpeed, blizzardFreezeDuration, blizzardTickRate, blizzardDamageTimer, blizzardDamage);
        }

        yield return new WaitForSeconds(blizzardDelay);

        int randomIndex = Random.Range(0, teleportPoints.Length);

        GameObject selectedPoint = teleportPoints[randomIndex];

        target.position = selectedPoint.transform.position;
    }

    public IEnumerator CastIcyRain(Transform target)
    {
        List<Vector3> icicleSpawnPoints = new List<Vector3>();

        for (int i = 0; i < icicleCount; i++)
        {
            Vector3 randomPoint = Random.insideUnitSphere * icicleSpawnRadius;
            randomPoint.y = 0f;

            Vector3 groundPosition = target.position + randomPoint;

            icicleSpawnPoints.Add(groundPosition);

            GameObject icyWarning = Instantiate(icicleWarningPrefab, groundPosition, Quaternion.identity);
            IcicleWarning warning = icyWarning.GetComponent<IcicleWarning>();
            if (warning != null)
            {
                warning.Initialize(icicleWarningLifeTime);
            }
        }

        yield return new WaitForSeconds(icicleWarningDuration);

        foreach (Vector3 point in icicleSpawnPoints)
        {
            Vector3 spawnPosition = point;
            spawnPosition.y += icicleSpawnHeight;

            GameObject icicleObj = Instantiate(rainOfIciclesPrefab, spawnPosition, Quaternion.identity);
            IcyRain rainPrefab = icicleObj.GetComponent<IcyRain>();
            if (rainPrefab != null)
            {
                rainPrefab.Initialize(icicleDamage, icicleFreezeDuration, icicleFallSpeed, icicleLifeTime, frozenGroundExpandSpeed, frozenGroundMaxScale, 
                    frozenGroundLifeTime, frozenGroundFreezeDuration, frozenGroundPrefab, point);
            }
        }
    }

    public IEnumerator CastGlacialCrash(Transform caster, Transform target)
    {
        Vector3 bossStartPos = caster.position;
        Vector3 airPos = bossStartPos + Vector3.up * glacialCrashRiseHeight;

        while (Vector3.Distance(caster.position, airPos) > 0.01f)
        {
            caster.position = Vector3.MoveTowards(caster.position, airPos, glacialCrashRiseSpeed * Time.deltaTime);
            yield return null;
        }

        FacePlayer(caster, target);

        yield return new WaitForSeconds(glacialCrashChargeTime);

        for (int i = 0; i < glacialCrashIcicleCount; i++)
        {
            Vector3 randPos = Random.insideUnitSphere * glacialCrashSpreadRadius;
            randPos.y = 0f;

            Vector3 impactPos = target.position + randPos;

            GameObject glacialCrashIcicles = Instantiate(glacialCrashPrefab, glacialCrashSpawnPoint.position, Quaternion.identity);
            GlacialCrash gc = glacialCrashIcicles.GetComponent<GlacialCrash>();
            if (gc != null)
            {
                gc.Initialize(glacialCrashDamage, glacialCrashFreezeDuration, glacialCrashIcicleSpeed, glacialCrashIcicleLifeTime, frozenGroundPrefab, impactPos,
                    frozenGroundExpandSpeed, frozenGroundMaxScale, frozenGroundLifeTime, frozenGroundFreezeDuration);
            }

            yield return new WaitForSeconds(3f);
        }

        yield return new WaitForSeconds(glacialCrashReleaseTime);

        while (Vector3.Distance(caster.position, bossStartPos) > 0.01f)
        {
            caster.position = Vector3.MoveTowards(caster.position, bossStartPos, glacialCrashRiseSpeed * Time.deltaTime);
            yield return null;
        }

        caster.position = bossStartPos;
    }

    /// <summary>
    /// Rotates the caster to face the target on the horizontal plane, used at the end of the spinning phase of the Water Bubble Shield ability to orient the boss towards the player before launching the bubbles outward. The method calculates the direction from the caster to the target, ignores any vertical difference, and sets the caster's rotation to look in that direction if it is significant enough. This ensures that when the bubbles are launched outward, they will be aimed towards the player's current position for a more threatening and engaging attack pattern.
    /// </summary>
    /// <param name="caster">The transform of the boss.</param>
    /// <param name="target">The transform of the player.</param>
    public void FacePlayer(Transform caster, Transform target)
    {
        if (caster == null || target == null) { return; }

        Vector3 dir = target.position - caster.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            caster.rotation = Quaternion.LookRotation(dir.normalized);
        }
    }
}
