using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LightningAbilityPack : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject markVisualPrefab;

    [SerializeField] float markDuration;

    [Header("-----Arc Spear Components-----")]
    [SerializeField] GameObject arcSpearPrefab;
    [SerializeField] GameObject arcSpearSpawnPoint;
    [SerializeField] float arcSpearMaxCooldown;
    [SerializeField] float arcSpearTelegraph;
    [SerializeField] float arcSpearLifeTime;
    [SerializeField] int arcSpearSpeed;
    [SerializeField] int arcSpearDamage;

    [Header("-----Voltage Lightning Mine Components-----")]
    [SerializeField] GameObject voltMinePrefab;
    [SerializeField] GameObject arenaCenter;
    [SerializeField] float voltMineExplosionRadius;
    [SerializeField] float voltMineArmDelay;
    [SerializeField] float voltMineMaxCooldown;
    [SerializeField] float timeBetweenMineSpawns;
    [SerializeField] int voltMineSpawnCount;
    [SerializeField] int maxActiveMines;
    [SerializeField] int voltMineDamage;
    [SerializeField] int mineSpawnRangeX;
    [SerializeField] int mineSpawnRangeY;
    [SerializeField] int mineSpawnRangeZ;

    [Header("-----Thunder Strike Components-----")]
    [SerializeField] GameObject thunderStrikePrefab;
    [SerializeField] GameObject thunderWarningPrefab;
    [SerializeField] float thunderStrikeMaxCooldown;
    [SerializeField] float thunderStrikeTelegraph;
    [SerializeField] float thunderStrikeRadius;
    [SerializeField] float thunderStrikeLifeTime;
    [SerializeField] float timeBetweenThunderStrikes;
    [SerializeField] int thunderStrikeDamage;
    [SerializeField] int thunderStrikeCount;
    [SerializeField] int thunderSpawnRangeX;
    [SerializeField] int thunderSpawnRangeY;
    [SerializeField] int thunderSpawnRangeZ;

    [Header("-----Magnetic Pull Components-----")]
    [SerializeField] GameObject magneticPullPrefab;
    [SerializeField] float pullRadius;
    [SerializeField] float pullDuration;
    [SerializeField] float pullMaxCooldown;
    [SerializeField] int pullStrength;

    [Header("-----Storm Surge Components-----")]
    [SerializeField] float stormSurgeMaxCooldown;
    [SerializeField] float stormSurgeTelegraph;
    [SerializeField] float timeBetweenMineDetonations;

    [Header("-----Ion Crash Components-----")]
    [SerializeField] GameObject ionCrashPrefab;
    [SerializeField] GameObject ionCrashWarningPrefab;
    [SerializeField] float ionCrashMaxCooldown;
    [SerializeField] float ionCrashImpactRadius;
    [SerializeField] float ionCrashNearbyMineDetectionRadius;
    [SerializeField] float ionCrashTelegraph;
    [SerializeField] float ionCrashLifeTime;
    [SerializeField] float ionCrashFlyUpTime;
    [SerializeField] float ionCrashSlamTime;
    [SerializeField] int ionCrashFlyUpHeight;
    [SerializeField] int ionCrashDamage;

    [Header("-----EMP Pulse Components-----")]
    [SerializeField] GameObject empPulsePrefab;
    [SerializeField] float empPulseMaxCooldown;
    [SerializeField] float empPulseTelegraph;
    [SerializeField] float empPulseLifeTime;
    [SerializeField] float empPulseRadius;
    [SerializeField] int empPulseDamage;

    [Header("-----Static Detonation Components-----")]
    [SerializeField] GameObject staticDetonationPrefab;
    [SerializeField] GameObject staticDetonationWarningPrefab;
    [SerializeField] int staticDetonationDamage;
    [SerializeField] float staticDetonationRadius;
    [SerializeField] float staticDetonationTelegraph;
    [SerializeField] float staticDetonationLifeTime;
    [SerializeField] float staticDetonationMaxCooldown;

    

    float currentMarkTimer;
    float arcSpearCoolDown;
    float voltMineCooldown;
    float thunderStrikeCooldown;
    float pullCooldown;
    float stormSurgeCooldown;
    float ionCrashCooldown;
    float empPulseCooldown;
    float staticDetonationCooldown;

    List<VoltageMine> activeMines = new List<VoltageMine>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            player = GameManager.instance.player;
        }

        arcSpearCoolDown = 0f;
        voltMineCooldown = 0f;
        thunderStrikeCooldown = 0f;
        pullCooldown = 0f;
        stormSurgeCooldown = 0f;
        ionCrashCooldown = 0f;
        empPulseCooldown = 0f;
        staticDetonationCooldown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.playerIsMarked)
        {
            currentMarkTimer -= Time.deltaTime;

            if (currentMarkTimer <= 0)
            {
                RemoveStaticMark();
            }
        }

        if (arcSpearCoolDown > 0)
        {
            arcSpearCoolDown -= Time.deltaTime;
        }
        if (voltMineCooldown > 0)
        {
            voltMineCooldown -= Time.deltaTime;
        }
        if (thunderStrikeCooldown > 0)
        {
            thunderStrikeCooldown -= Time.deltaTime;
        }
        if (pullCooldown > 0)
        {
            pullCooldown -= Time.deltaTime;
        }
        if (stormSurgeCooldown > 0)
        {
            stormSurgeCooldown -= Time.deltaTime;
        }
        if (ionCrashCooldown > 0)
        {
            ionCrashCooldown -= Time.deltaTime;
        }
        if (empPulseCooldown > 0)
        {
            empPulseCooldown -= Time.deltaTime;
        }
        if (staticDetonationCooldown > 0)
        {
            staticDetonationCooldown -= Time.deltaTime;
        }
    }

    public IEnumerator CastArcSpear(Transform caster, Transform target)
    {
        if (arcSpearCoolDown > 0) { yield break; }

        arcSpearCoolDown = arcSpearMaxCooldown;

        yield return new WaitForSeconds(arcSpearTelegraph);

        Transform spawnPos = arcSpearSpawnPoint != null ? arcSpearSpawnPoint.transform : caster;

        if (spawnPos != null && target != null)
        {
            Vector3 distToPlayer = (target.position - spawnPos.position).normalized;

            GameObject spearProj = Instantiate(arcSpearPrefab, spawnPos.position, Quaternion.identity);
            ArcSpear arcSp = spearProj.GetComponent<ArcSpear>();
            if (arcSp != null)
            {
                arcSp.Initialize(arcSpearSpeed, arcSpearDamage, arcSpearLifeTime, distToPlayer, this);
            }
        }
    }

    public IEnumerator CastThunderStrike()
    {
        if (thunderStrikeCooldown > 0) { yield break; }

        thunderStrikeCooldown = thunderStrikeMaxCooldown;

        List<GameObject> warningList = new List<GameObject>();
        List<Vector3> thunderStrikePos = new List<Vector3>();

        for (int i = 0; i < thunderStrikeCount; i++)
        {
            float randomX = Random.Range(-thunderSpawnRangeX, thunderSpawnRangeX);
            float randomZ = Random.Range(-thunderSpawnRangeZ, thunderSpawnRangeZ);

            Vector3 strikePos = arenaCenter.transform.position + new Vector3(randomX, thunderSpawnRangeY, randomZ);
            thunderStrikePos.Add(strikePos);

            GameObject thunderWarning = Instantiate(thunderWarningPrefab, strikePos, Quaternion.identity);
            warningList.Add(thunderWarning);
        }

        yield return new WaitForSeconds(thunderStrikeTelegraph);

        foreach (GameObject thunderWarning in warningList)
        {
            Destroy(thunderWarning);
        }

        foreach (Vector3 pos in thunderStrikePos)
        {
            GameObject thunderPrefab = Instantiate(thunderStrikePrefab, pos, Quaternion.identity);
            ThunderStrike ts = thunderPrefab.GetComponent<ThunderStrike>();
            if (ts != null)
            {
                ts.Initialize(thunderStrikeDamage, thunderStrikeRadius, thunderStrikeLifeTime, this);
            }

            yield return new WaitForSeconds(timeBetweenMineSpawns);
        }

        DetonateAllMines();
    }

    public IEnumerator CastStormSurge()
    {
        if (stormSurgeCooldown > 0)
        {
            yield break;
        }

        stormSurgeCooldown = stormSurgeMaxCooldown;

        if (activeMines.Count == 0) { yield break; }

        foreach (VoltageMine mine in activeMines)
        {
            //Spawn warning/pulse visual on mine later
        }

        yield return new WaitForSeconds(stormSurgeTelegraph);

        foreach (VoltageMine mine in activeMines)
        {
            if (mine != null)
            {
                mine.Explode();
            }

            yield return new WaitForSeconds(timeBetweenMineDetonations);
        }

        activeMines.Clear();
    }

    public IEnumerator CastIonCrash(Transform caster, Transform target)
    {
        if (ionCrashCooldown > 0) { yield break; }

        ionCrashCooldown = ionCrashMaxCooldown;

        Vector3 casterOriPos = caster.position;
        Vector3 targetPos = target.position;

        GameObject ionCWPrefab = Instantiate(ionCrashWarningPrefab, targetPos, Quaternion.identity);

        Vector3 flyUpPos = casterOriPos + Vector3.up;

        caster.transform.position = flyUpPos;

        yield return new WaitForSeconds(ionCrashTelegraph);

        caster.transform.position -= flyUpPos * Time.deltaTime;

        Destroy(ionCWPrefab);

        caster.position = target.position;

        GameObject ionCIPrefab = Instantiate(ionCrashPrefab, target.position, Quaternion.identity);
        IonCrashImpact ici = ionCIPrefab.GetComponent<IonCrashImpact>();
        if (ici != null)
        {
            ici.Initialize(ionCrashDamage, ionCrashImpactRadius, ionCrashLifeTime, ionCrashNearbyMineDetectionRadius, this);
        }
    }

    public IEnumerator CastEMPPulse(Transform caster)
    {
        if (ionCrashCooldown > 0) { yield break; }

        empPulseCooldown = empPulseMaxCooldown;

        // Spawn EMP warning visual on caster later

        yield return new WaitForSeconds(empPulseTelegraph);

        GameObject empPrefab = Instantiate(empPulsePrefab, caster.position, Quaternion.identity);
        EMPPulse emp = empPrefab.GetComponent<EMPPulse>();
        if (emp != null)
        {
            emp.Initialize(empPulseDamage, empPulseRadius, empPulseLifeTime, this);
        }
    }

    public IEnumerator CastStaticDetonation(Transform target)
    {
        if (staticDetonationCooldown > 0) { yield break; }

        if (!GameManager.instance.playerIsMarked) { yield break; }

        staticDetonationCooldown = staticDetonationMaxCooldown;

        Vector3 targetPos = target.position;

        GameObject warning = Instantiate(staticDetonationWarningPrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(staticDetonationTelegraph);

        Destroy(warning);

        GameObject sdp = Instantiate(staticDetonationPrefab, targetPos, Quaternion.identity);
        StaticDetonation sd = sdp.GetComponent<StaticDetonation>();
        if (sd != null)
        {
            sd.Initialize(staticDetonationDamage, staticDetonationRadius, staticDetonationLifeTime, this);
        }

        RemoveStaticMark();
    }

    public IEnumerator SpawnVoltageMine()
    {
        if (voltMineCooldown > 0) { yield break; }

        voltMineCooldown = voltMineMaxCooldown;

        for (int i = 0; i < voltMineSpawnCount; i++)
        {
            if (activeMines.Count >= maxActiveMines)
            {
                yield break;
            }

            float randomX = Random.Range(-mineSpawnRangeX, mineSpawnRangeX);
            float randomZ = Random.Range(-mineSpawnRangeZ, mineSpawnRangeZ);

            Vector3 spawnPos = arenaCenter.transform.position + new Vector3(randomX, 0, randomZ);
            spawnPos.y = mineSpawnRangeY;

            GameObject voltageMinePrefab = Instantiate(voltMinePrefab, spawnPos, Quaternion.identity);
            VoltageMine voltMine = voltageMinePrefab.GetComponent<VoltageMine>();
            if (voltMine != null)
            {
                voltMine.Initialize(voltMineDamage, voltMineExplosionRadius, voltMineArmDelay, this);
                activeMines.Add(voltMine);
            }

            yield return new WaitForSeconds(timeBetweenMineSpawns);
        }
    }

    public IEnumerator MagneticPull(Transform caster)
    {
        if (pullCooldown > 0)
        {
            yield break;
        }

        pullCooldown = pullMaxCooldown;

        activeMines.RemoveAll(mine => mine == null);

        Vector3 pullPosition;

        if (activeMines.Count > 0)
        {
            int randIndex = Random.RandomRange(0, activeMines.Count);
            pullPosition = activeMines[randIndex]. transform.position;
        }
        else
        {
            pullPosition = caster.position;
        }

        GameObject mageneticPrefab = Instantiate(magneticPullPrefab, caster.position, Quaternion.identity);
        MagneticPull mp = magneticPullPrefab.GetComponent<MagneticPull>();
        if (mp != null)
        {
            mp.Initialize(pullStrength, pullRadius, pullDuration);
        }
    }

    public void ApplyStaticMark()
    {
        if (GameManager.instance.playerIsMarked)
        {
            RefreshStaticMark();
            return;
        }

        GameManager.instance.playerIsMarked = true;
        RefreshStaticMark();

        //Set Visual Mark in version 1.5

        //Save spawned visual as currentMarkVisual in version 1.5
    }

    void RefreshStaticMark()
    {
        currentMarkTimer = markDuration;
    }

    void RemoveStaticMark()
    {
        GameManager.instance.playerIsMarked = false;
        currentMarkTimer = 0;
    }

    public void DetonateNearbyMines(Vector3 position, float radius)
    {

        activeMines.RemoveAll(mine => mine == null);

        List<VoltageMine> minesToRemove = new List<VoltageMine>();

        foreach (VoltageMine mine in activeMines)
        {
            if (mine != null)
            {
                float minePos = Vector3.Distance(mine.transform.position, position);
                if (minePos <= radius)
                {
                    mine.Explode();
                    minesToRemove.Add(mine);
                }
            }
        }

        foreach (VoltageMine mine in minesToRemove)
        {
            activeMines.Remove(mine);
        }
    }

    public void DetonateAllMines()
    {
        foreach (VoltageMine mine in activeMines)
        {
            if (mine != null)
            {
                mine.Explode();
            }
        }

        activeMines.Clear();
    }
}
