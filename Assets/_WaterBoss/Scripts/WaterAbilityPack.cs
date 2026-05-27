using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaterAbilityPack : MonoBehaviour
{
    [Header("-----Water Prison Components-----")]
    [SerializeField] GameObject waterPrisonPrefab;
    [SerializeField] GameObject[] waterPrisonMinionPrefabs;
    [SerializeField] float waterPrisonTelegraph;
    [SerializeField] float waterPrisonMaxCooldown;
    [SerializeField] float waterPrisonDuration;
    [SerializeField] int waterPrisonMinionCount;
    [SerializeField] int waterPrisonMinionRadius;

    [Header("-----Acid Wave Components-----")]
    [SerializeField] GameObject acidWavePrefab;
    [SerializeField] float acidWaveMaxCooldown;
    [SerializeField] float acidWaveTelegraph;
    [SerializeField] float acidWaveSpawnDistance;
    [SerializeField] float acidWaveSpeed;
    [SerializeField] float acidWaveLifeTime;
    [SerializeField] float acidWaveTickInterval;
    [SerializeField] float acidWaveDOTDuration;
    [SerializeField] int acidWaveDamage;
    [SerializeField] int acidWaveTickDamage;

    [Header("-----Hydro Snipe Components-----")]
    [SerializeField] GameObject hydroSnipePrefab;
    [SerializeField] Transform hydroSnipeSpawnPoint;
    [SerializeField] float hydroSnipeTelegraph;
    [SerializeField] float hydroSnipeProjectileSpeed;
    [SerializeField] float hydroSnipeMaxCooldown;
    [SerializeField] float hydroSnipeLifeTime;
    [SerializeField] int hydroSnipeDamage;

    [Header("-----Tidal Pull Components-----")]
    [SerializeField] GameObject tidalPullPrefab;
    [SerializeField] float tidalPullMaxCooldown;
    [SerializeField] float tidalPullTelegraph;
    [SerializeField] float tidalPullSpawnDistance;
    [SerializeField] float tidalPullConeAngle;
    [SerializeField] float tidalPullDuration;
    [SerializeField] float tidalPullZoneRadius;
    [SerializeField] float tidalPullCatchRadius;
    [SerializeField] float tidalPullStrength;
    [SerializeField] float tidalPullGroundY;

    [Header("-----Depth Charge Components-----")]
    [SerializeField] GameObject depthChargePrefab;
    [SerializeField] float depthChargeTelegraph;
    [SerializeField] float depthChargeMaxCooldown;
    [SerializeField] float depthChargeDelay;
    [SerializeField] float depthChargeRadius;
    [SerializeField] int depthChargeDamage;
    [SerializeField] int depthChargeCount;

    [Header("-----Water Bubble Shield Components-----")]
    [SerializeField] GameObject waterBubbleShieldPrefab;
    [SerializeField] float waterBubbleShieldMaxCooldown;
    [SerializeField] float waterBubbleShieldTelegraph;
    [SerializeField] float waterBubbleOrbitSpeed;
    [SerializeField] float spinDuration;
    [SerializeField] float shieldHoldDuration;
    [SerializeField] float spinSpeed;
    [SerializeField] float bubbleSpawnInterval;
    [SerializeField] float bubbleLaunchDuration;
    [SerializeField] float bubbleShootSpeed;
    [SerializeField] float bubbleShootLifeTime;
    [SerializeField] float referenceBossScale;
    [SerializeField] float baseShieldCenterHeight;
    [SerializeField] float baseShieldHalfHeight;
    [SerializeField] float baseShieldMaxRadius;
    [SerializeField] int waterBubbleDamage;

    [Header("-----Phase Swim Components-----")]
    [SerializeField] GameObject bossVisualRoot;
    [SerializeField] GameObject tidalWavePrefab;
    [SerializeField] float phaseSwimMaxCooldown;
    [SerializeField] float phaseSwimTelegraph;
    [SerializeField] float diveDuration;
    [SerializeField] float reappearDistanceFromPlayer;
    [SerializeField] float waterLevelY;
    [SerializeField] float tidalWaveSpeed;
    [SerializeField] float tidalWaveLifeTime;
    [SerializeField] float tidalWaveKnockbackForce;
    [SerializeField] float meleeRange;
    [SerializeField] int tidalWaveDamage;
    [SerializeField] int meleeDamage;


    [Header("-----Mirage Split Components-----")]
    [SerializeField] GameObject mirageSplitPrefab;
    [SerializeField] float mirageSplitTelegraph;
    [SerializeField] float mirageSplitMaxCooldown;
    [SerializeField] float cloneSpawnRadius;
    [SerializeField] int mirageCloneCount;

    [Header("-----Mirage Clone Attack Components-----")]
    [SerializeField] GameObject cloneHydroSnipePrefab;
    [SerializeField] float cloneAttackInterval;
    [SerializeField] float cloneProjectileSpeed;
    [SerializeField] float cloneProjectileLifeTime;
    [SerializeField] int cloneProjectileDamage;

    float hydroSnipeCooldown;
    float waterPrisonCooldown;
    float mirageSplitCooldown;
    float waterBubbleShieldCooldown;
    float acidWaveCooldown;
    float tidalPullCooldown;
    float depthChargeCooldown;
    float phaseSwimCooldown;

    List<MirageSplit> activeClones = new List<MirageSplit>();
    List<BubbleShield> activeBubbleShieldBubbles = new List<BubbleShield>();

    bool mirageSplitActive;
    bool castingWaterBubbleShield;
    bool castingTidalPull;
    bool castingPhaseSwim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hydroSnipeCooldown = 0f;
        waterPrisonCooldown = 0f;
        mirageSplitCooldown = 0f;
        waterBubbleShieldCooldown = 0f;
        acidWaveCooldown = 0f;
        tidalPullCooldown = 0f;
        depthChargeCooldown = 0f;
        phaseSwimCooldown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (hydroSnipeCooldown > 0f)
        {
            hydroSnipeCooldown -= Time.deltaTime; 
        }
        if (waterPrisonCooldown > 0f)
        {
            waterPrisonCooldown -= Time.deltaTime; 
        }
        if (mirageSplitCooldown > 0f)
        {
            mirageSplitCooldown -= Time.deltaTime;
        }
        if (waterBubbleShieldCooldown > 0f)
        {
            waterBubbleShieldCooldown -= Time.deltaTime;
        }
        if (acidWaveCooldown > 0f)
        {
            acidWaveCooldown -= Time.deltaTime;
        }
        if (tidalPullCooldown > 0f)
        {
            tidalPullCooldown -= Time.deltaTime;
        }
        if (depthChargeCooldown > 0f)
        {
            depthChargeCooldown -= Time.deltaTime;
        }
        if (phaseSwimCooldown > 0f)
        {
            phaseSwimCooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Casts the Water Prison ability, which after a short telegraph, spawns a prison at the player's position that holds them in place and spawns minions around them. The method first checks if the target is valid and if the ability is off cooldown, then sets the cooldown for Water Prison. It waits for the telegraph duration, then instantiates the Water Prison prefab at the target's position and initializes it with the necessary parameters for its behavior, including the target to imprison, the duration of the prison effect, the minion prefabs to spawn, the number of minions to spawn, and the radius around the prison to spawn the minions.
    /// </summary>
    /// <param name="caster">The transform of the boss casting the ability.</param>
    /// <param name="target">The transform of the player target.</param>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    public IEnumerator CastWaterPrison(Transform caster, Transform target)
    {
        if (target == null) { yield break; }

        if (waterPrisonCooldown > 0) { yield break; }

        waterPrisonCooldown = waterPrisonMaxCooldown;

        yield return new WaitForSeconds(waterPrisonTelegraph);

        if (waterPrisonPrefab != null)
        {
            Vector3 prisonPos = target.position;
            GameObject prison = Instantiate(waterPrisonPrefab, prisonPos, Quaternion.identity);
            WaterPrison pris = prison.GetComponent<WaterPrison>();

            if (pris != null)
            {
                pris.Initialize(target.transform, waterPrisonDuration, waterPrisonMinionPrefabs, waterPrisonMinionCount, waterPrisonMinionRadius);
            }
        }
    }

    /// <summary>
    /// Casts the Acid Wave ability, which after a short telegraph, spawns a projectile that shoots forward from the boss in a straight line, dealing damage and applying a damage-over-time effect to the player if hit. The method first checks if the caster and target are valid and if the ability is off cooldown, then sets the cooldown for Acid Wave. It waits for the telegraph duration, then calculates the spawn position for the Acid Wave projectile based on the caster's position and forward direction. The projectile is instantiated and initialized with its movement direction, speed, lifetime, damage, tick damage, tick interval, and DOT duration to control its behavior once spawned.
    /// </summary>
    /// <param name="caster">The transform of the boss casting the ability.</param>
    /// <param name="target">The transform of the player target.</param>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    public IEnumerator CastAcidWave(Transform caster, Transform target)
    {
        if (caster == null || target == null) { yield break; }
        if (acidWaveCooldown > 0f) { yield break; }

        acidWaveCooldown = acidWaveMaxCooldown;

        yield return new WaitForSeconds(acidWaveTelegraph);

        Vector3 dir = caster.forward;
        dir.y = 0f;
        dir = dir.normalized;

        Vector3 spawnPos = caster.position + dir * acidWaveSpawnDistance;

        GameObject acidWave = Instantiate(acidWavePrefab, spawnPos, Quaternion.LookRotation(dir));
        AcidWave aw = acidWave.GetComponent<AcidWave>();
        if (aw != null)
        {
            aw.Initialize(dir, acidWaveSpeed, acidWaveLifeTime, acidWaveDamage, acidWaveTickDamage, acidWaveTickInterval, acidWaveDOTDuration);
        }
    }

    /// <summary>
    /// Casts the Hydro Snipe ability, which after a short telegraph, spawns a projectile that shoots in a straight line towards the player's position at the time of casting. The method first checks if the caster and target are valid and if the ability is off cooldown, then waits for the telegraph duration before instantiating the Hydro Snipe projectile at the specified spawn point (or the caster's position if no spawn point is set). The projectile is initialized with its target position, speed, lifetime, and damage. Finally, the cooldown for Hydro Snipe is set to prevent it from being cast again until the cooldown expires.
    /// </summary>
    /// <param name="caster">The transform of the boss casting the ability.</param>
    /// <param name="target">The transform of the player target.</param>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    [System.Obsolete]
    public IEnumerator CastHydroSnipe(Transform caster, Transform target)
    {
        yield return new WaitForSeconds(hydroSnipeTelegraph);

        Transform spawn = hydroSnipeSpawnPoint != null ? hydroSnipeSpawnPoint : caster;

        if (hydroSnipePrefab != null && spawn != null)
        {
            GameObject proj = Instantiate(hydroSnipePrefab, spawn.position, Quaternion.identity);
            HydroSnipe wp = proj.GetComponent<HydroSnipe>();
            if (wp != null)
            {
                wp.Init(target.position, hydroSnipeProjectileSpeed, hydroSnipeLifeTime, hydroSnipeDamage);
            }
            else
            {
                Rigidbody rb = proj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = (target.position - spawn.position).normalized;
                    rb.velocity = dir * hydroSnipeProjectileSpeed;
                }
            }
        }

        hydroSnipeCooldown = hydroSnipeMaxCooldown;
    }

    /// <summary>
    /// Casts the Tidal Pull ability, which after a short telegraph, spawns a Tidal Pull zone at a set distance in front of the boss that pulls the player towards its center if they are within the zone radius, and then if the player is pulled within a smaller catch radius, they are held in place for a duration and take damage. The method first checks if the caster and target are valid and if the ability is off cooldown, then sets the cooldown and active state for casting Tidal Pull. It calculates the spawn position for the Tidal Pull zone based on the caster's position and forward direction, ensuring it is at the correct ground level. After waiting for the telegraph duration, it instantiates the Tidal Pull prefab at the calculated position and initializes it with the necessary parameters for its behavior. Finally, it waits for the duration of the Tidal Pull effect before allowing it to be cast again.
    /// </summary>
    /// <param name="caster">The transform of the boss casting the ability.</param>
    /// <param name="target">The transform of the player target.</param>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    public IEnumerator CastTidalPull(Transform caster, Transform target)
    {
        if (caster == null || target == null) { yield break; }

        if (tidalPullCooldown > 0f || castingTidalPull) { yield break; }

        castingTidalPull = true;
        tidalPullCooldown = tidalPullMaxCooldown;

        Vector3 savedPullCenter = caster.position + caster.forward * tidalPullSpawnDistance;
        savedPullCenter.y = tidalPullGroundY;

        yield return new WaitForSeconds(tidalPullTelegraph);

        GameObject tidalPull = Instantiate(tidalPullPrefab, savedPullCenter, Quaternion.identity);
        TidalPull tp = tidalPull.GetComponent<TidalPull>();
        if (tp != null)
        {
            tp.Initialize(target, savedPullCenter, tidalPullDuration, tidalPullStrength, tidalPullZoneRadius, tidalPullCatchRadius, tidalPullConeAngle, this, caster);
        }

        yield return new WaitForSeconds(tidalPullDuration);

        castingTidalPull = false;
    }

    /// <summary>
    /// Casts the Depth Charge ability, which spawns a number of depth charges around the player after a short telegraph. Each depth charge then explodes after a delay, dealing damage to the player if they are within the radius. The method first checks if the target is valid and if the ability is off cooldown, then sets the cooldown for the ability. It waits for the telegraph duration, then calculates random positions around the target to spawn the depth charges, ensuring they are at ground level. Each depth charge is initialized with its explosion parameters such as delay before explosion, radius, and damage.
    /// </summary>
    /// <param name="target">The transform of the player target.</param>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    public IEnumerator CastDepthCharge(Transform target)
    {
        if (target == null) { yield break; }

        if (depthChargeCooldown > 0f) { yield break; }

        depthChargeCooldown = depthChargeMaxCooldown;

        yield return new WaitForSeconds(depthChargeTelegraph);

        List<Vector3> directions = new List<Vector3>();
        Vector3 forward = target.forward;
        directions.Add(forward);
        Vector3 right = target.right;
        directions.Add(right);
        Vector3 left = -right;
        directions.Add(left);
        Vector3 forwardRight = (forward + right).normalized;
        directions.Add(forwardRight);
        Vector3 forwardLeft = (forward + left).normalized;
        directions.Add(forwardLeft);

        for (int i = 0; i < depthChargeCount; i++)
        {
            Vector3 randPos = target.position + directions[Random.Range(0, directions.Count)] * Random.Range(3f, 6f);
            randPos.y = 0f;

            GameObject depthCharge = Instantiate(depthChargePrefab, randPos, Quaternion.identity);

            DepthCharge dc = depthCharge.GetComponent<DepthCharge>();
            if (dc != null)
            {
                dc.Initialize(depthChargeDelay, depthChargeRadius,depthChargeDamage);
            }
        }
    }

    /// <summary>
    /// Casts the Water Bubble Shield ability, which creates a rotating shield of bubbles around the boss that then launch outward towards the player after a short duration. The method first checks if the caster is valid and if the ability is off cooldown, then sets the cooldown and active state for the ability. It waits for the telegraph duration, then builds the data for spawning the bubbles in a spherical distribution around the boss. It enters a loop to spawn the bubbles in a spinning pattern around the boss for the duration of the spin, then faces the boss towards the player and holds the shield for a set duration before launching all bubbles outward towards the player's position.
    /// </summary>
    /// <param name="caster">The transform of the boss casting the ability.</param>
    /// <param name="target">The transform of the player target.</param>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    public IEnumerator CastWaterBubbleShield(Transform caster, Transform target)
    {
        if (caster == null) { yield break; }
        if (waterBubbleShieldCooldown > 0 || castingWaterBubbleShield) { yield break; }

        castingWaterBubbleShield = true;
        waterBubbleShieldCooldown = waterBubbleShieldMaxCooldown;

        activeBubbleShieldBubbles.Clear();

        yield return new WaitForSeconds(waterBubbleShieldTelegraph);

        List<BubbleSpawnData> bubbleData = BuildBubbleSphereData(caster);

        float timer = 0f;
        float spinTimer = 0f;
        int spawnedIndex = 0;

        while (timer < spinDuration)
        {
            caster.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

            spinTimer += Time.deltaTime;

            if (spawnedIndex < bubbleData.Count && spinTimer >= bubbleSpawnInterval)
            {
                SpawnSingleBubble(caster, bubbleData[spawnedIndex]);
                spawnedIndex++;
                spinTimer = 0f;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        while (spawnedIndex < bubbleData.Count)
        {
            caster.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

            spinTimer += Time.deltaTime;

            if (spinTimer >= bubbleSpawnInterval)
            {
                SpawnSingleBubble(caster, bubbleData[spawnedIndex]);
                spawnedIndex++;
                spinTimer = 0f;
            }

            yield return null;
        }

        FacePlayer(caster, target);

        yield return new WaitForSeconds(shieldHoldDuration);

        FireAllBubblesOutward(caster);

        castingWaterBubbleShield = false;
    }

    /// <summary>
    /// Casts the Phase Swim ability, which allows the boss to dive underwater and reappear at a different location near the player, then immediately launches a Tidal Wave attack in a cone towards the player's position. The method first checks if the caster and target are valid and if the ability is off cooldown, then sets the cooldown and active state for Phase Swim. It waits for the telegraph duration, then hides the boss's visuals to simulate diving underwater. After waiting for the dive duration, it calculates a new position for the boss to reappear based on the player's position and a specified distance, ensuring it is at the correct water level. The boss is then moved to the new position, rotated to face the player, and its visuals are re-enabled. Finally, it spawns a Tidal Wave attack directed towards the player and performs a melee check after a short delay to damage any player that is too close when the boss reappears.
    /// </summary>
    /// <param name="caster">The transform of the boss casting the ability.</param>
    /// <param name="target">The transform of the player target.</param>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    public IEnumerator CastPhaseSwim(Transform caster, Transform target)
    {
        if (caster == null || target == null) { yield break; }

        if (phaseSwimCooldown > 0f) { yield break; }

        castingPhaseSwim = true;
        phaseSwimCooldown = phaseSwimMaxCooldown;

        yield return new WaitForSeconds(phaseSwimTelegraph);

        bossVisualRoot.SetActive(false);

        yield return new WaitForSeconds(diveDuration);

        Vector3 bossToPlayer = target.position - caster.position;
        bossToPlayer.y = 0f;

        if (bossToPlayer.sqrMagnitude < 0.001f)
        {
            bossToPlayer = caster.forward;
        }

        bossToPlayer.Normalize();

        Vector3 reappearPos = target.position - bossToPlayer * reappearDistanceFromPlayer;
        reappearPos.y = waterLevelY;

        caster.position = reappearPos;

        FacePlayer(caster, target);

        bossVisualRoot.SetActive(true);

        SpawnTidalWave(caster, target);

        yield return new WaitForSeconds(0.2f);

        PhaseSwimMeleeCheck(caster, target);

        castingPhaseSwim = false;
    }

    /// <summary>
    /// Casts the Mirage Split ability, which creates multiple clones of the boss that orbit around it and attack the player with Hydro Snipe projectiles. The method first checks if the caster and target are valid and if the ability is off cooldown, then sets the cooldown and active state for Mirage Split. It waits for the telegraph duration, then spawns the specified number of clones in a circular pattern around the boss, using NavMesh sampling to ensure they spawn on valid ground. Each clone is initialized with a reference to the player, the boss, the Hydro Snipe prefab for their attacks, and the parameters for their attack behavior. The clones are added to the activeClones list to keep track of them for later management (such as clearing them when Mirage Split ends or when they are destroyed).
    /// </summary>
    /// <param name="caster">The transform of the boss casting the ability.</param>
    /// <param name="target">The transform of the player target.</param>
    /// <returns>An IEnumerator for coroutine handling.</returns>
    public IEnumerator CastMirageSplit(Transform caster, Transform target)
    {
        if (caster == null || target == null) { yield break; }
        if (mirageSplitCooldown > 0 || mirageSplitActive) { yield break; }

        mirageSplitCooldown = mirageSplitMaxCooldown;
        mirageSplitActive = true;

        yield return new WaitForSeconds(mirageSplitTelegraph);

        activeClones.Clear();

        for (int i = 0; i < mirageCloneCount; i++)
        {
            float angle = i * Mathf.PI * 2f / mirageCloneCount;
            Vector3 offset = new Vector3(Mathf.Cos(angle) * cloneSpawnRadius, 0f, Mathf.Sin(angle) * cloneSpawnRadius);
            Vector3 spawnPos = caster.position + offset;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, cloneSpawnRadius, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }

            GameObject clone = Instantiate(mirageSplitPrefab, spawnPos, Quaternion.identity);

            MirageSplit ms = clone.GetComponent<MirageSplit>();
            if (ms != null)
            {
                ms.Initialize(target, caster, cloneHydroSnipePrefab, cloneAttackInterval, cloneProjectileSpeed, cloneProjectileLifeTime, cloneProjectileDamage, this);
                activeClones.Add(ms);
            }
        }
    }

    /// <summary>
    /// Destroys all active Mirage Split clones and clears the activeClones list, used to immediately end the Mirage Split ability and reset the boss's state regarding the clones. This method can be called by the boss's main script when certain conditions are met (such as the boss taking a certain amount of damage or reaching a specific phase) to prevent the clones from lingering indefinitely and to allow the boss to cast Mirage Split again after the cooldown. It iterates through the activeClones list in reverse order, destroys each clone's game object if it is not null, and then clears the list to remove all references to the destroyed clones.
    /// </summary>
    public void ClearMirageClones()
    {
        for(int i = activeClones.Count - 1; i >= 0; i--)
        {
            if (activeClones[i] != null)
            {
                Destroy(activeClones[i].gameObject);
            }
        }

        activeClones.Clear();
    }

    /// <summary>
    /// Immediately ends the Mirage Split ability by destroying all active clones and setting mirageSplitActive to false, allowing the boss to cast Mirage Split again after the cooldown. This method can be called by the boss's main script when certain conditions are met (such as the boss taking a certain amount of damage or reaching a specific phase) to prevent the clones from lingering indefinitely and to reset the boss's state regarding the Mirage Split ability.
    /// </summary>
    public void EndMirageSplitEarly()
    {
        ClearMirageClones();

        mirageSplitActive = false;
    }

    /// <summary>
    /// Removes a Mirage Split clone from the active clones list when it is destroyed, and if there are no more active clones, sets mirageSplitActive to false to allow the boss to cast Mirage Split again. This method is called by each Mirage Split clone in its OnDestroy method to ensure that the boss's state is updated correctly when clones are removed from the scene, preventing potential issues with lingering clones or the boss being unable to cast Mirage Split again after clones are destroyed.
    /// </summary>
    /// <param name="clone">The Mirage Split clone to remove from the active clones list.</param>
    public void RemoveMirageClone(MirageSplit clone)
    {
        if (activeClones.Contains(clone))
        {
            activeClones.Remove(clone);
        }

        if (activeClones.Count <= 0)
        {
            mirageSplitActive = false;
        }
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

    /// <summary>
    /// A struct to hold the necessary data for spawning and orbiting a single bubble in the Water Bubble Shield ability, including its height offset from the boss, its radius from the boss, and its starting angle for orbiting. This data is calculated in BuildBubbleSphereData to create a spherical distribution of bubbles around the boss, and is used in SpawnSingleBubble to position and initialize each bubble correctly.
    /// </summary>
    struct BubbleSpawnData
    {
        public float heightOffSet;
        public float radius;
        public float startingAngle;

        public BubbleSpawnData(float height, float rad, float angle)
        {
            heightOffSet = height;
            radius = rad;
            startingAngle = angle;
        }
    }

    /// <summary>
    /// Calculates the spawn positions for all bubbles in the Water Bubble Shield ability based on a spherical distribution around the boss, taking into account the boss's current scale to maintain consistent proportions. The method generates multiple rings of bubbles at different heights and radii to create a visually appealing shield effect, and returns a list of BubbleSpawnData that contains the necessary information for spawning each bubble in the correct position and orbiting pattern.
    /// </summary>
    /// <param name="caster">The transform of the boss.</param>
    /// <returns>A list of BubbleSpawnData containing the spawn positions and orbit parameters for each bubble.</returns>
    List<BubbleSpawnData> BuildBubbleSphereData(Transform caster)
    {
        List<BubbleSpawnData> data = new List<BubbleSpawnData>();

        float scaleMultplier = caster.localScale.y / referenceBossScale;

        float centerHeight = baseShieldCenterHeight * scaleMultplier;
        float halfHeight = baseShieldHalfHeight * scaleMultplier;
        float maxRadius = baseShieldMaxRadius * scaleMultplier;

        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, -1.15f, 8);
        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, -1.0f, 5);
        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, -0.75f, 8);
        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, -0.5f, 10);
        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, -0.25f, 12);
        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, 0.0f, 14);
        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, 0.25f, 12);
        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, 0.5f, 10);
        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, 0.75f, 8);
        AddBubbleRing(data, centerHeight, halfHeight, maxRadius, 1.0f, 5);

        return data;
    }

    /// <summary>
    /// Calculates the spawn positions for a ring of bubbles at a specific normalized height along the vertical axis of the bubble shield, and adds the corresponding BubbleSpawnData to the provided list. The radius of the ring is determined by the normalized height to create a spherical distribution of bubbles around the boss.
    /// </summary>
    /// <param name="data">The list to which the BubbleSpawnData will be added.</param>
    /// <param name="centerHeight">The center height of the bubble shield.</param>
    /// <param name="halfHeight">Half of the height of the bubble shield.</param>
    /// <param name="maxRadius">The maximum radius of the bubble shield.</param>
    /// <param name="normalizedHeight">The normalized height along the vertical axis of the bubble shield.</param>
    /// <param name="count">The number of bubbles to spawn in the ring.</param>
    void AddBubbleRing(List<BubbleSpawnData> data, float centerHeight, float halfHeight, float maxRadius, float normalizedHeight, int count)
    {
        float heightOffset = centerHeight + normalizedHeight * halfHeight;

        float radiusScale = Mathf.Sqrt(1f - normalizedHeight * normalizedHeight);
        float ringRadius = maxRadius * radiusScale;

        ringRadius = Mathf.Max(ringRadius, maxRadius * 0.45f);
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            data.Add(new BubbleSpawnData(heightOffset, ringRadius   , angle));
        }
    }

    /// <summary>
    /// Spawns a single bubble for the Water Bubble Shield ability at the appropriate position around the boss based on the provided BubbleSpawnData, and initializes it to orbit around the boss. Called repeatedly in CastWaterBubbleShield to spawn all bubbles in a spinning pattern.
    /// </summary>
    /// <param name="caster">The transform of the boss.</param>
    /// <param name="data">The data for the bubble's spawn position and orbit parameters.</param>
    void SpawnSingleBubble(Transform caster, BubbleSpawnData data)
    {
        if (waterBubbleShieldPrefab == null) { return; }

        Vector3 launchStartPos = caster.position + Vector3.up * data.heightOffSet;

        Vector3 orbitOffSet = new Vector3(Mathf.Cos(data.startingAngle) * data.radius, data.heightOffSet, Mathf.Sin(data.startingAngle) * data.radius);

        Vector3 orbitTargetPos = caster.position + orbitOffSet;

        GameObject bubble = Instantiate(waterBubbleShieldPrefab, launchStartPos, Quaternion.identity);

        BubbleShield bs = bubble.GetComponent<BubbleShield>();
        if (bs != null)
        {
            bs.Initialize(caster, data.radius, data.heightOffSet, waterBubbleDamage, waterBubbleOrbitSpeed, data.startingAngle, bubbleLaunchDuration, orbitTargetPos, bubbleShootSpeed, bubbleShootLifeTime);

            activeBubbleShieldBubbles.Add(bs);
        }
    }

    /// <summary>
    /// Launches all active bubble shield bubbles outward from the boss's position in a radial pattern, dealing damage to the player if hit. Called after the shield hold duration ends in CastWaterBubbleShield.
    /// </summary>
    /// <param name="caster">The transform of the boss.</param>
    void FireAllBubblesOutward(Transform caster)
    {
        for (int i = 0; i < activeBubbleShieldBubbles.Count; i++)
        {
            if (activeBubbleShieldBubbles[i] != null)
            {
                activeBubbleShieldBubbles[i].LaunchOutward(caster.position);
            }
        }

        activeBubbleShieldBubbles.Clear();
    }

    /// <summary>
    /// Spawns a tidal wave projectile that shoots forward from the boss's position after reappearing from Phase Swim, dealing damage and knockback to the player if hit.
    /// </summary>
    /// <param name="caster">The transform of the boss.</param>
    /// <param name="target">The transform of the player.</param>
    void SpawnTidalWave(Transform caster, Transform target)
    {
        if (tidalWavePrefab == null || target == null || caster == null) { return; }

        Vector3 dir = target.position - caster.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
        {
            dir = caster.forward;
        }

        dir.Normalize();

        Vector3 spawnPos = caster.position + dir * 1.5f;
        spawnPos.y = waterLevelY;

        GameObject tidalWaveObj = Instantiate(tidalWavePrefab, spawnPos, Quaternion.LookRotation(dir));

        PhaseSwim wave = tidalWaveObj.GetComponent<PhaseSwim>();
        if (wave != null)
        {
            wave.Initialize(dir, tidalWaveSpeed, tidalWaveLifeTime, tidalWaveDamage, tidalWaveKnockbackForce);
        }
    }

    /// <summary>
    /// Checks if the boss is within melee range of the player after reappearing from Phase Swim, and if so, applies damage.
    /// </summary>
    /// <param name="caster">The transform of the boss.</param>
    /// <param name="target">The transform of the player.</param>
    void PhaseSwimMeleeCheck(Transform caster, Transform target)
    {
        if (caster == null || target == null) { return; }

        float dist = Vector3.Distance(caster.position, target.position);

        if (dist <= meleeRange)
        {
            IDamage damage = target.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.TakeDamage(meleeDamage);
            }
        }
    }
}
