using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuLose;

    public GameObject player;
    public GameObject solarisPathGate;

    public Image playerHPBar;
    public Image playerSprintBar;

    public TextMeshProUGUI solarisDialogue;

    public PlayerController playerScript;

    public bool isPaused;

    public string sceneName;

    private float timeScaleOrig;

    void Awake()
    {
        instance = this;

        timeScaleOrig = Time.timeScale;

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                Paused();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                UnPaused();
            }
        }
    }

    /// <summary>
    /// Pauses the game by halting time progression and enabling the cursor.
    /// </summary>
    /// <remarks>This method sets the game's time scale to zero, effectively freezing all time-dependent
    /// actions. It also makes the cursor visible and unlocks it, allowing user interaction with UI elements.</remarks>
    public void Paused()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Resumes normal gameplay by restoring the time scale, hiding and locking the cursor, and deactivating the pause
    /// menu.
    /// </summary>
    /// <remarks>Call this method to exit the paused state and return to active gameplay. The pause menu will
    /// be hidden, and player input will be re-enabled. The cursor will be locked and made invisible to match typical
    /// gameplay conditions.</remarks>
    public void UnPaused()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    /// <summary>
    /// Displays the loss menu and pauses the game.
    /// </summary>
    /// <remarks>Call this method when the player loses to present the loss menu and halt gameplay. The game
    /// remains paused until resumed or restarted.</remarks>
    public void Lose()
    {
        Paused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    /// <summary>
    /// Opens the path to Solaris by deactivating the gate and loading the Solaris scene.
    /// </summary>
    /// <remarks>This method disables the gate blocking access to Solaris and transitions the application to
    /// the specified Solaris scene. Calling this method will immediately load the new scene, replacing the current
    /// one.</remarks>
    public void OpenPathToSolaris()
    {
        solarisPathGate.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Displays the specified dialogue message in the Solaris dialogue UI for a limited time.
    /// </summary>
    /// <remarks>The dialogue UI will remain visible for a short duration before automatically hiding. Calling
    /// this method while a previous message is still visible will replace the displayed message.</remarks>
    /// <param name="dialogueMessage">The message to display in the dialogue UI. Cannot be <c>null</c>.</param>
    public void ShowDialogue(string dialogueMessage)
    {
        solarisDialogue.gameObject.SetActive(true);
        solarisDialogue.text = dialogueMessage;
        StartCoroutine(HideDialogueAfterDelay(5f));
    }

    /// <summary>
    /// Waits for the specified delay before hiding the dialogue UI.
    /// </summary>
    /// <param name="delay">The time, in seconds, to wait before the dialogue is hidden. Must be non-negative.</param>
    /// <returns>An enumerator that yields until the delay has elapsed, after which the dialogue UI is hidden.</returns>
    private IEnumerator HideDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        solarisDialogue.gameObject.SetActive(false);
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
}
