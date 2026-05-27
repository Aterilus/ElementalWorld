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
    [SerializeField] GameObject menuEV;
    
    public GameObject uiRoot;
    public GameObject player;
    
    public PlayerController playerScript;
    public PlayerEVSystem playerEVSystem;

    public bool isPaused;
    public bool isEVMenuOpen;
    public bool bullManCompleted;
    public bool playerIsMarked;

    string sceneName;

    private float timeScaleOrig;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(uiRoot.gameObject);

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

        if (Input.GetButtonDown("Submit"))
        {
            ToggleEVMenu();
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

    public void ToggleEVMenu()
    {
        if (isEVMenuOpen == false)
        {
            ShowEVMenu();
        }
        else
        {
            HideEVMenu();
        }
    }

    public void ShowEVMenu()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isEVMenuOpen = true;

        menuActive = menuEV;
        menuActive.SetActive(true);

        playerEVSystem.UpdateEVUI();
    }

    public void HideEVMenu()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isEVMenuOpen = false;

        menuActive.SetActive(false);
        menuActive = null;
    }

}
