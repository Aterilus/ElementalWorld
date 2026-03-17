using System.Collections;
using UnityEngine;

public class SolarisAI : MonoBehaviour, IDamage
{
    [SerializeField] int hp;

    public GameObject player;

    [Header("-----Movement-----")]
    public Transform[] teleportPoints;
    public float teleportTimer;
    public float teleportMaxTimer;

    Color origColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        teleportTimer -= Time.deltaTime;
        FacePlayer();
        TickTeleport();
    }

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

    void TickTeleport()
    {
        if (teleportTimer <= 0)
        {
            TeleportToRandomPoint();
            teleportTimer = teleportMaxTimer;
        }
    }

    void TeleportToRandomPoint()
    {
        int index = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[index].position;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            //GameManager.instance.
        }
    }
}
