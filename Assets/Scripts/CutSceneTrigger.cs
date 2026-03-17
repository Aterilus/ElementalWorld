using System.Collections;
using TMPro;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    [SerializeField] Camera cutSceneCamera;
    [SerializeField] Camera playerCamera;
    [SerializeField] PlayerController playerController;

    [SerializeField] GameObject playerHPUI;
    [SerializeField] GameObject playerSprintUI;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI solarisDialogue;

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
        playerCamera.enabled = false;
        playerCamera.gameObject.SetActive(false);
        cutSceneCamera.enabled = true;
        cutSceneCamera.gameObject.SetActive(true);
        playerController.enabled = false;
        playerHPUI.SetActive(false);
        playerSprintUI.SetActive(false);

        dialoguePanel.SetActive(true);

        foreach (string line in introLines)
        {
            solarisDialogue.text = line;
            yield return new WaitForSeconds(lineDuration);
        }

        dialoguePanel.SetActive(false);

        playerCamera.enabled = true;
        playerCamera.gameObject.SetActive(true);
        cutSceneCamera.enabled = false;
        cutSceneCamera.gameObject.SetActive(false);
        playerController.enabled = true;
        playerHPUI.SetActive(true);
        playerSprintUI.SetActive(true);

        yield return new WaitForSeconds(lineDuration);

        GameManager.instance.LoadNextScene(sceneName);
    }
}
