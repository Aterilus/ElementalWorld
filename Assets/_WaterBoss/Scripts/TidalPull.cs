using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TidalPull : MonoBehaviour
{

    private Transform caster;
    private Transform target;

    private WaterAbilityPack waterAbilityPackRef;

    private float duration;
    private float zoneRadius;
    private float catchRadius;
    private float coneAngle;
    private float strength;

    private Vector3 centerPull;

    private bool hasTriggeredPrison;

    /// <summary>
    /// Initializes the tidal pull with the given parameters. Must be called immediately after instantiating the tidal pull prefab. This method sets up the tidal pull's behavior, including its duration, strength, and the area of effect. It also starts the coroutine that handles the tidal pull's logic.
    /// </summary>
    /// <param name="_target">The target that will be affected by the tidal pull.</param>
    /// <param name="_centerPull">The center point towards which the target will be pulled.</param>
    /// <param name="_duration">The duration for which the tidal pull will be active.</param>
    /// <param name="_strength">The strength of the pull effect.</param>
    /// <param name="_zoneRadius">The radius within which the tidal pull will affect the target.</param>
    /// <param name="_catchRadius">The radius within which the target will be considered caught and trigger the water prison effect.</param>
    /// <param name="_coneAngle">The angle of the cone within which the tidal pull will affect the target.</param>
    /// <param name="_waterAbilityPackRef">Reference to the WaterAbilityPack instance for triggering the water prison effect.</param>
    /// <param name="_caster">The transform of the caster initiating the tidal pull.</param>
    public void Initialize(Transform _target, Vector3 _centerPull, float _duration, float _strength, float _zoneRadius, float _catchRadius, float _coneAngle, WaterAbilityPack _waterAbilityPackRef, Transform _caster)
    {
        target = _target;
        centerPull = _centerPull;
        duration = _duration;
        strength = _strength;
        coneAngle = _coneAngle;
        zoneRadius = _zoneRadius;
        catchRadius = _catchRadius;
        waterAbilityPackRef = _waterAbilityPackRef;
        caster = _caster;
        target = _target; 

        transform.position = centerPull;

        StartCoroutine(TidalPullRoutine());
    }

    /// <summary>
    /// Coroutine that handles the tidal pull effect. It continuously checks if the player is within the defined radius and cone angle, and if so, it pulls the player towards the center of the pull. If the player gets close enough to the center, it triggers the water prison effect. The coroutine runs for the specified duration or until the player is trapped, after which it destroys the tidal pull object.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator TidalPullRoutine()
    {
        float timer = 0f;

        while (timer < duration)
        {
            if (target == null)
            {
                Destroy(gameObject);
                yield break;
            }

            Vector3 flatPlayerPos = target.position;
            flatPlayerPos.y = centerPull.y;

            float distanceToCenter = Vector3.Distance(flatPlayerPos, centerPull);

            Vector3 bossToPlaayer = target.position - caster.position;
            bossToPlaayer.y = 0f;

            float angleToPlayer = Vector3.Angle(caster.forward, bossToPlaayer.normalized);

            bool insideRadius = distanceToCenter <= zoneRadius;
            bool insideCone = angleToPlayer <= coneAngle;

            if (insideRadius && insideCone)
            {
                Vector3 directionToCenter = centerPull - flatPlayerPos;
                directionToCenter.y = 0f;

                if (directionToCenter.magnitude > 0.001f)
                {
                    target.position += directionToCenter.normalized * strength * Time.deltaTime;
                }
            }
                        
            if (!hasTriggeredPrison && distanceToCenter <= catchRadius)
            {
                Debug.Log("TRAPPING PLAYER: Distance: " + distanceToCenter + " Catch Radius: " + catchRadius);
                hasTriggeredPrison = true;

                if (waterAbilityPackRef != null)
                {
                    yield return StartCoroutine(waterAbilityPackRef.CastWaterPrison(caster, target));
                }

                Destroy(gameObject);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
