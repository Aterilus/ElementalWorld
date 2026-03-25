using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class BullManHorde : MonoBehaviour
{
    [SerializeField] GameObject bossBullManPrefab;
    [SerializeField] GameObject lowerBullManPrefab;
    [SerializeField] Camera cutSceneCamera;
    [SerializeField] Transform[] lowerSpawnPoints;
    [SerializeField] Transform bossSpawnPoint;
    [SerializeField] GameObject exitGate;

    [SerializeField] PlayerController playerController;

    
    [SerializeField] int lowerBullManCountMin;
    [SerializeField] int lowerBullManCountMax;
    [SerializeField] int maxBullManCount;
    [SerializeField] int evPointsRewarded;

    int enemiesAlive;
    int enemiesSpawnedSoFar;

    bool eventStarted;
    bool bossSpawned;
    bool eventCompleted;
    bool bossPhaseStarted;

   void Awake()
    {
        if (cutSceneCamera != null)
        {
            GameManager.instance.cutSceneCamera = cutSceneCamera;
        }

        if (exitGate != null)
        {
            exitGate.SetActive(false);
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!eventStarted && other.CompareTag("Player"))
        {
            eventStarted = true;
            StartCoroutine(StartEncounter());
        }
    }

    IEnumerator StartEncounter()
    {
        if (playerController == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerController = playerObj.GetComponent<PlayerController>();
            }
        }

        playerController.enabled = false;

        BossBullManAI bossAI = bossBullManPrefab.GetComponent<BossBullManAI>();
        if (bossAI != null)
        {
            bossAI.enabled = false;
        }

        NavMeshAgent bossAgent = bossBullManPrefab.GetComponent<NavMeshAgent>();
        if (bossAgent != null)
        {
            bossAgent.enabled = false;
        }

        GameManager.instance.playerCamera.gameObject.SetActive(false);
        GameManager.instance.cutSceneCamera.gameObject.SetActive(true);

        GameManager.instance.dialoguePanel.gameObject.SetActive(true);
        GameManager.instance.dialogue.text = "You've stumbled upon my domain.";

        yield return new WaitForSeconds(3f);

        GameManager.instance.dialogue.text = "My children will eat you alive.";

        yield return new WaitForSeconds(3f);

        GameManager.instance.dialogue.text = "Get ready to die....";

        yield return new WaitForSeconds(3f);

        GameManager.instance.dialoguePanel.gameObject.SetActive(false);
        GameManager.instance.cutSceneCamera.gameObject.SetActive(false);
        GameManager.instance.playerCamera.gameObject.SetActive(true);
        playerController.enabled = true;

        bossBullManPrefab.SetActive(false);
        bossSpawned = false;

        SpawnLowerRankedBullMan();
    }

    void SpawnLowerRankedBullMan()
    {
        if (enemiesSpawnedSoFar >= maxBullManCount) { return; }

        int remainingToSpawn = maxBullManCount - enemiesSpawnedSoFar;
        int spawnCount = Random.Range(lowerBullManCountMin, lowerBullManCountMax + 1);

        if (spawnCount > remainingToSpawn)
        {
            spawnCount = remainingToSpawn;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject spawnedBullMan = Instantiate(lowerBullManPrefab, lowerSpawnPoints[i].position, lowerSpawnPoints[i].rotation);
            
            LowerBullManAI bullManAI = spawnedBullMan.GetComponent< LowerBullManAI>();
            if (bullManAI != null)
            {
                bullManAI.SetHordeManager(this);
            }

            enemiesAlive++;
            enemiesSpawnedSoFar++;
        }
    }

    public void OnEnemyDefeated()
    {
        enemiesAlive--;
        if (enemiesAlive > 0) { return; }

        if (!bossSpawned)
        {
            if (enemiesSpawnedSoFar < maxBullManCount)
            {
                SpawnLowerRankedBullMan();
            }
            else if (!bossPhaseStarted)
            {
                bossPhaseStarted = true;
                StartCoroutine(StartBossPhase());
            }
        }
        else if (!eventCompleted)
        {
            CompleteHorde();
        }
        
    }

    IEnumerator StartBossPhase()
    {
        if (playerController == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerController = playerObj.GetComponent<PlayerController>();
            }
        }

        playerController.enabled = false;
        GameManager.instance.playerCamera.gameObject.SetActive(false);
        GameManager.instance.cutSceneCamera.gameObject.SetActive(true);

        bossBullManPrefab.transform.position = bossSpawnPoint.position;
        bossBullManPrefab.SetActive(true);

        BossBullManAI bossAI = bossBullManPrefab.GetComponent<BossBullManAI>();
        if (bossAI != null)
        {
            bossAI.SetHordeManager(this);
            bossAI.enabled = true;
        }

        GameManager.instance.dialoguePanel.gameObject.SetActive(true);
        GameManager.instance.dialogue.text = "You've managed to defeat my children.";
        yield return new WaitForSeconds(3f);
        GameManager.instance.dialogue.text = "Now get ready to taste defeat.";
        yield return new WaitForSeconds(3f);
        GameManager.instance.dialogue.text = "Die human!!!!";
        yield return new WaitForSeconds(3f);

        GameManager.instance.dialoguePanel.gameObject.SetActive(false);
        GameManager.instance.cutSceneCamera.gameObject.SetActive(false);
        GameManager.instance.playerCamera.gameObject.SetActive(true);
        playerController.enabled = true;

        NavMeshAgent bossAgent = bossBullManPrefab.GetComponent<NavMeshAgent>();
        if (bossAgent != null)
        {
            bossAgent.enabled = true;
        }

        GameManager.instance.bossHPUI.gameObject.SetActive(true);

        enemiesAlive = 1;
        bossSpawned = true;
    }

    void CompleteHorde()
    {
        eventCompleted = true;

        GameManager.instance.bossHPUI.gameObject.SetActive(false);

        if (exitGate != null)
        {
            exitGate.SetActive(true);
        }

        GameManager.instance.bullManCompleted = true;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerEVSystem evSystem = playerObj.GetComponent<PlayerEVSystem>();
            if (evSystem != null)
            {
                evSystem.GiveEvPoints(evPointsRewarded);
            }
        }
    }
}
