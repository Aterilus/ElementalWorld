using System.Collections;
using TMPro;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    [SerializeField] GameObject playerHPUI;
    [SerializeField] GameObject playerSprintUI;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogue;

    [SerializeField] string[] introLines;
    [SerializeField] string sceneName;
    [SerializeField] float lineDuration;

    bool isTriggered;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        { 
            isTriggered = true;
            StartCoroutine(StartCutSceneIntro());
        }
    }

    public IEnumerator StartCutSceneIntro()
    {
        GameManager.instance.playerCamera.gameObject.SetActive(false);
        GameManager.instance.cutSceneCamera.gameObject.SetActive(true);
        playerController.enabled = false;
        playerHPUI.SetActive(false);
        playerSprintUI.SetActive(false);

        dialoguePanel.SetActive(true);

        foreach (string line in introLines)
        {
            dialogue.text = line;
            yield return new WaitForSeconds(lineDuration);
        }

        dialoguePanel.SetActive(false);

        GameManager.instance.playerCamera.gameObject.SetActive(true);
        GameManager.instance.cutSceneCamera.gameObject.SetActive(false);
        playerController.enabled = true;
        playerHPUI.SetActive(true);
        playerSprintUI.SetActive(true);

        yield return new WaitForSeconds(lineDuration);

        GameManager.instance.LoadNextScene(sceneName);
    }
}
