using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject dialoguePanel;
    public GameObject playerHealthUI;
    public GameObject playerSprintUI;
    public GameObject bossHPUI;
    public GameObject bossShieldUI;

    public Image playerHPBar;
    public Image playerSprintBar;
    public Image bossHPBar;
    public Image bossShieldBar;

    public Image blindFlashOverlay;

    public TextMeshProUGUI dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Displays the specified dialogue message in the Solaris dialogue UI for a limited time.
    /// </summary>
    /// <remarks>The dialogue UI will remain visible for a short duration before automatically hiding. Calling
    /// this method while a previous message is still visible will replace the displayed message.</remarks>
    /// <param name="dialogueMessage">The message to display in the dialogue UI. Cannot be <c>null</c>.</param>
    public void ShowDialogue(string dialogueMessage)
    {
        dialogue.gameObject.SetActive(true);
        dialogue.text = dialogueMessage;
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
        dialogue.gameObject.SetActive(false);
    }
}
