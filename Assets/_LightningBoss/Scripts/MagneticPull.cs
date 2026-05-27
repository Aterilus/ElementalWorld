using System.Collections;
using UnityEngine;

public class MagneticPull : MonoBehaviour
{
    float pullRadius;
    float pullDuration;

    int pullStrength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(pullDuration);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] pullRadiusCol = Physics.OverlapSphere(transform.position, pullRadius);
        foreach (Collider col in pullRadiusCol)
        {
            if (col.CompareTag("Player"))
            {
                Vector3 dir = transform.position - col.transform.transform.position;
                dir = dir.normalized;

                CharacterController controller = col.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.Move(dir * pullStrength * Time.deltaTime);
                }
            }
        }
    }

    public void Initialize(int _pullStrength, float _pullRadius, float _pullDuration)
    {
        pullStrength = _pullStrength;
        pullRadius = _pullRadius;
        pullDuration = _pullDuration;
    }
}
