using System.Collections;
using TMPro;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera cutSceneCamera;

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
        if (playerController == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerController = playerObj.GetComponent<PlayerController>();
            }
        }

        GameManager.instance.cutSceneCamera = cutSceneCamera;

        GameManager.instance.playerCamera.gameObject.SetActive(false);
        GameManager.instance.cutSceneCamera.gameObject.SetActive(true);
        playerController.enabled = false;
        GameManager.instance.playerHealthUI.SetActive(false);
        GameManager.instance.playerSprintUI.SetActive(false);

        GameManager.instance.dialoguePanel.SetActive(true);

        foreach (string line in introLines)
        {
            GameManager.instance.dialogue.text = line;
            yield return new WaitForSeconds(lineDuration);
        }

        GameManager.instance.dialoguePanel.SetActive(false);

        GameManager.instance.playerCamera.gameObject.SetActive(true);
        GameManager.instance.cutSceneCamera.gameObject.SetActive(false);
        playerController.enabled = true;
        GameManager.instance.playerHealthUI.SetActive(true);
        GameManager.instance.playerSprintUI.SetActive(true);

        yield return new WaitForSeconds(lineDuration);

        GameManager.instance.LoadNextScene(sceneName);
    }
}
