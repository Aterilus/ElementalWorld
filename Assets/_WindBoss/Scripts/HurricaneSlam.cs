using System.Collections;
using UnityEngine;

public class HurricaneSlam : MonoBehaviour
{
    [SerializeField] GameObject slamVisuals;
    [SerializeField] float slamDelay;
    [SerializeField] float slamRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(HurricaneSlamAttack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HurricaneSlamAttack()
    {
        //yield return new WaitForSeconds(slamDelay);
        float timer = 0f;
        float dur = slamDelay;

        Vector3 startScale = new Vector3(0.1f, 0.025f, 0.1f);
        Vector3 endScale = new Vector3(slamRadius, 0.025f, slamRadius);

        while (timer < dur)
        {
            slamVisuals.transform.localScale = Vector3.Lerp(startScale, endScale, timer / dur);

            timer += Time.deltaTime;
            yield return null;
        }

        slamVisuals.transform.localScale = endScale;
    }
}
