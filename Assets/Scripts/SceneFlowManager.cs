using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager instance;

    public GameObject PathGate;

    [SerializeField] public Camera cutSceneCamera;
    [SerializeField] public Camera playerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (playerCamera == null)
        {
            playerCamera = GameManager.instance.player.GetComponentInChildren<Camera>(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    /// <summary>
    /// Loads the specified scene by name.
    /// </summary>
    /// <remarks>Loading a new scene will replace the current scene. Ensure that any unsaved data is handled
    /// before calling this method.</remarks>
    /// <param name="sceneName">The name of the scene to load. Must correspond to a scene included in the build settings.</param>
    public void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void OnSceneLoad(Scene sceneName, LoadSceneMode mode)
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawnPoint != null)
        {
            CharacterController controller = GameManager.instance.player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                GameManager.instance.player.transform.position = spawnPoint.transform.position;
                GameManager.instance.player.transform.rotation = spawnPoint.transform.rotation;
                controller.enabled = true;
            }
            else
            {
                GameManager.instance.player.transform.position = spawnPoint.transform.position;
                GameManager.instance.player.transform.rotation = spawnPoint.transform.rotation;
            }
        }
    }
}
