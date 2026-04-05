using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class WindAbilityPack : MonoBehaviour
{
    [SerializeField] GameObject cyclonePrefab;
    [SerializeField] GameObject hurricaneStrikePrefab;
    [SerializeField] GameObject airBladePrefab;
    [SerializeField] GameObject WindCurrentPrefab;
    [SerializeField] GameObject phantomGustAttackPrefab;
    [SerializeField] GameObject hurricaneSlamPrefab;
    [SerializeField] GameObject eyeOfTheStormPrefab;
    [SerializeField] GameObject skyLiftPrefab;
    [SerializeField] GameObject arenaCenter;

    [Header("-----Cyclone Components-----")]
    [SerializeField] GameObject cycloneSpawnPoint;
    [SerializeField] int cycloneDamageAmount;
    [SerializeField] float cycloneLifeTime;
    [SerializeField] float cycloneMoveSpeed;
    [SerializeField] float cyloneCastRecovery;

    [Header("-----Hurricane Components-----")]
    [SerializeField] GameObject hurricaneSpawnPoint;
    [SerializeField] int hurricaneDamageAmount;
    [SerializeField] float hurricaneLifeTime;
    [SerializeField] float hurricaneSpeed;
    [SerializeField] float hurricaneCastRecovery;

    [Header("-----Hurricane Slam Components-----")]
    [SerializeField] GameObject hurricaneSlamSpawnPoint;
    [SerializeField] int hurricaneSlamDamageAmount;
    [SerializeField] float hurricaneSlamRiseHeight;
    [SerializeField] float hurricaneSlamRiseTime;
    [SerializeField] float hurricaneSlamHangTime;
    [SerializeField] float hurricaneSlamDropTime;
    [SerializeField] float hurricaneSlamRadius;
    [SerializeField] float  hurricaneSlamKnockbackForce;
    [SerializeField] float hurricaneSlamCastTime;
    [SerializeField] float hurricaneSlamRecoveryTime;
    [SerializeField] float hurricaneSlamEffectLifeTime;

    [Header("-----Air Blade Components-----")]
    [SerializeField] GameObject airBladeSpawnPoint;
    [SerializeField] int airBladeDamageAmount;
    [SerializeField] float airBladeLifeTime;
    [SerializeField] float airBladeSpeed;
    [SerializeField] float airBladeCastRecovery;

    [Header("-----Wind Current Components-----")]
    [SerializeField] float windCurrentMoveSpeed;
    [SerializeField] float windCurrentDuration;
    [SerializeField] float windCurrentCastRecovery;

    [Header("-----Sky Lift Components-----")]
    [SerializeField] float skyLiftLifeTime;
    [SerializeField] float skyLiftRadius;
    [SerializeField] float skyLiftUpwardForce;
    [SerializeField] float skyLiftCastRecovery;

    [Header("-----Eye of the Storm Components-----")]
    [SerializeField] float eyeOfTheStormRadius;
    [SerializeField] float eyeOfTheStormDuration;
    [SerializeField] int eyeOfTheStormDamageTick;
    [SerializeField] float eyeOfTheStormTickRate;
    [SerializeField] float eyeOfTheStormCastRecovery;
    [SerializeField] float eyeOfTheStormLiftForce;
    [SerializeField] float eyeOfTheStormCastTIme;

    [Header("-----Phantom Gust Components-----")]
    [SerializeField] GameObject phantomGustWarningPrefab;
    [SerializeField] float phantomGustWarningDuration;
    [SerializeField] float phantomGustCastRecovery;
    [SerializeField] float phantomGustSpeed;
    [SerializeField] int phantomGustDamageAmount;
    [SerializeField] float phantomGustLifeTime;
    [SerializeField] Transform[] phantomGustSpawnPoints;

    public IEnumerator CastCyclone(Transform caster, Transform target)
    {
        if (cyclonePrefab == null || caster == null || target == null) { yield break; }

        Transform spawn = cycloneSpawnPoint != null ? cycloneSpawnPoint.transform : caster;

        Vector3 targetPos = target.position;
        Vector3 dir = targetPos - spawn.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            dir = spawn.forward;
        }

        dir.Normalize();

        GameObject cyclone = Instantiate(cyclonePrefab, spawn.position, Quaternion.LookRotation(dir));
        WindCyclone cycloneScript = cyclone.GetComponent<WindCyclone>();
        if (cycloneScript != null)
        {
            cycloneScript.Initialize(dir, cycloneMoveSpeed, cycloneDamageAmount, cycloneLifeTime);
        }

        yield return new WaitForSeconds(cyloneCastRecovery);
    }

    public IEnumerator CastHurricaneStrike(Transform caster)
    {
        if (hurricaneStrikePrefab == null || caster == null) { yield break; }

        Transform spawn = hurricaneSpawnPoint != null ? hurricaneSpawnPoint.transform : caster;

        List<Vector3> directions = new List<Vector3>();
        Vector3 forward = spawn.forward;
        directions.Add(forward);
        Vector3 backwards = -forward;
        directions.Add(backwards);
        Vector3 right = spawn.right;
        directions.Add(right);
        Vector3 left = -right;
        directions.Add(left);
        Vector3 forwardRight = (forward + right).normalized;
        directions.Add(forwardRight);
        Vector3 forwardLeft = (forward + left).normalized;
        directions.Add(forwardLeft);
        Vector3 backwardsRight = (backwards + right).normalized;
        directions.Add(backwardsRight);
        Vector3 backwardsLeft = (backwards + left).normalized;
        directions.Add(backwardsLeft);

        foreach (Vector3 dir in directions)
        {
            GameObject strike = Instantiate(hurricaneStrikePrefab, spawn.position, Quaternion.LookRotation(dir));
            WindHurricane hurricaneScript = strike.GetComponent<WindHurricane>();
            if (hurricaneScript != null)
            {
                // Initialize hurricane strike if needed
                hurricaneScript.Initialize(dir, hurricaneSpeed, hurricaneDamageAmount, hurricaneLifeTime);
            }
        }

        yield return new WaitForSeconds(hurricaneCastRecovery);
    }

    public IEnumerator CastHurricaneSlam(Transform caster, Transform target)
    {
        if (caster == null) { yield break; }

        NavMeshAgent agent = caster.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        Vector3 startPos = caster.position;
        Vector3 topPos = startPos + Vector3.up * hurricaneSlamRiseHeight;
        Vector3 landPos = startPos;
        if (target != null)
        {
            landPos = target.position;
            landPos.y = startPos.y;
        }

        float timer = 0f;
        while (timer < hurricaneSlamRiseTime)
        {
            caster.position = Vector3.Lerp(startPos, topPos, timer / hurricaneSlamRiseTime);
            timer += Time.deltaTime;
            yield return null;
        }
        caster.position = topPos;

        yield return new WaitForSeconds(hurricaneSlamHangTime);

        timer = 0f;
        while (timer < hurricaneSlamDropTime)
        {
            caster.position = Vector3.Lerp(topPos, landPos, timer / hurricaneSlamDropTime);
            timer += Time.deltaTime;
            yield return null;
        }
        caster.position = landPos;

        Vector3 slamCenter = landPos;

        GameObject slam = null;
        if (hurricaneSlamPrefab != null)
        {
            slam = Instantiate(hurricaneSlamPrefab, slamCenter, Quaternion.identity);
            Destroy(slam, hurricaneSlamEffectLifeTime);
        }

        if (target != null)
        {
            Vector3 toPlayer = target.position - slamCenter;
            toPlayer.y = 0f;

            if (toPlayer.magnitude <= hurricaneSlamRadius)
            {
                IDamage damageable = target.GetComponent<IDamage>();
                if (damageable != null)
                {
                    damageable.TakeDamage(hurricaneSlamDamageAmount);
                }

                CharacterController playerController = target.GetComponent<CharacterController>();
                if (playerController != null)
                {
                    Vector3 knockbackDir = toPlayer.normalized;
                    playerController.Move(knockbackDir * hurricaneSlamKnockbackForce * Time.deltaTime);
                }
            }
        }

        if (agent != null)
        {
            agent.enabled = true;
        }

        yield return new WaitForSeconds(hurricaneSlamRecoveryTime);
    }

    public IEnumerator CastAirBlade(Transform caster)
    {
        if (caster == null || airBladePrefab == null) { yield break; }

        Transform spawn = airBladeSpawnPoint != null ? airBladeSpawnPoint.transform : caster;

        List<Vector3> directions = new List<Vector3>();
        Vector3 forward = caster.forward;
        directions.Add(forward);
        Vector3 right = caster.right;
        directions.Add(right);
        Vector3 left = -right;
        directions.Add(left);
        Vector3 forwardRight = (forward + right).normalized;
        directions.Add(forwardRight);
        Vector3 forwardLeft = (forward + left).normalized;
        directions.Add(forwardLeft);

        foreach (Vector3 dir in directions)
        {
            GameObject blade = Instantiate(airBladePrefab, spawn.position, Quaternion.LookRotation(dir));
            WindAirBlade bladeScript = blade.GetComponent<WindAirBlade>();
            if (bladeScript != null)
            {
                bladeScript.Initialize(dir, airBladeSpeed, airBladeDamageAmount, airBladeLifeTime);
            }
        }

        yield return new WaitForSeconds(airBladeCastRecovery);
    }

    public IEnumerator CastWindCurrent(Transform caster, Transform target)
    {
        if (caster == null || target == null) { yield break; }

        List<Vector3> directions = new List<Vector3>();

        Vector3 forward = caster.forward;
        forward.y = 0f;
        forward.Normalize();
        directions.Add(forward);

        Vector3 right = caster.right;
        right.y = 0f;
        right.Normalize();
        directions.Add(right);

        Vector3 left = -right;
        directions.Add(left);

        Vector3 back = -forward;
        directions.Add(back);

        Vector3 windDire = directions[Random.Range(0, directions.Count)];

        float timer = 0f;
        while (timer < windCurrentDuration)
        {
            if (target != null)
            {
                CharacterController playerController = target.GetComponent<CharacterController>();
                if (playerController != null)
                {
                    playerController.Move(windDire * windCurrentMoveSpeed * Time.deltaTime);
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(windCurrentCastRecovery);
    }

    public IEnumerator CastSkyLift(Transform caster, Transform target)
    {
        if (caster == null || target == null) { yield break; }

        Vector3 liftCenter = target.position;
        liftCenter.y = caster.position.y;

        yield return new WaitForSeconds(0.5f);

        GameObject skyLift = Instantiate(skyLiftPrefab, liftCenter, Quaternion.identity);
        Destroy(skyLift, skyLiftLifeTime);

        Vector3 toPlayer = target.position - liftCenter;
        toPlayer.y = 0f;

        if (toPlayer.magnitude <= skyLiftRadius)
        {
            CharacterController playerController = target.GetComponent<CharacterController>();
            if (playerController != null)
            {
                float timer = 0f;
                while (timer < skyLiftLifeTime)
                {
                    playerController.Move(Vector3.up * skyLiftUpwardForce * Time.deltaTime);
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
        }

        yield return new WaitForSeconds(skyLiftCastRecovery);
    }

    public IEnumerator CastEyeOfTheStorm(Transform caster, Transform target)
    {
        if (caster == null || target == null) { yield break; }

        Vector3 stormCenter = target.position;
        stormCenter.y = caster.position.y;

        yield return new WaitForSeconds(eyeOfTheStormCastTIme);

        GameObject storm = Instantiate(eyeOfTheStormPrefab, stormCenter, Quaternion.identity);
        Destroy(storm, eyeOfTheStormDuration);

        float timer = 0f;
        float tickTimer = 0f;

        while (timer < eyeOfTheStormDuration)
        {
            if (target != null)
            {
                Vector3 toPlayer = target.position - stormCenter;
                toPlayer.y = 0f;

                if (toPlayer.magnitude <= eyeOfTheStormRadius)
                {
                    CharacterController controller = target.GetComponent<CharacterController>();
                    if (controller != null)
                    {
                        controller.Move(Vector3.up * eyeOfTheStormLiftForce * Time.deltaTime);
                    }

                    tickTimer += Time.deltaTime;
                    if (tickTimer >= eyeOfTheStormTickRate)
                    {
                        IDamage damageable = target.GetComponent<IDamage>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(eyeOfTheStormDamageTick);
                        }

                        tickTimer = 0f;
                    }
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(eyeOfTheStormCastRecovery);
    }
    public IEnumerator CastPhantomGust(Transform caster)
    {
        if (caster == null || phantomGustAttackPrefab == null || phantomGustWarningPrefab == null || phantomGustSpawnPoints == null || phantomGustSpawnPoints.Length == 0) { yield break; }

        List<GameObject> warnings = new List<GameObject>();

        foreach (Transform point in phantomGustSpawnPoints)
        {
            GameObject warning = Instantiate(phantomGustWarningPrefab, point.position, Quaternion.identity);
            warnings.Add(warning);
        }

        yield return new WaitForSeconds(phantomGustWarningDuration);

        foreach (GameObject warning in warnings)
        {
            if (warning != null)
            {
                Destroy(warning);
            }
        }

        foreach (Transform point in phantomGustSpawnPoints)
        {
            if (point == null) { continue; }

            Vector3 dir = point.forward;
            dir.y = 0f;
            dir.Normalize();

            GameObject gust = Instantiate(phantomGustAttackPrefab, point.position, Quaternion.LookRotation(dir));

            WindPhantomGust gustScript = gust.GetComponent<WindPhantomGust>();
            if (gustScript != null)
            {
                gustScript.Initialize(dir, phantomGustSpeed, phantomGustDamageAmount, phantomGustLifeTime);
            }
        }

        yield return new WaitForSeconds(phantomGustCastRecovery);
    }
}
