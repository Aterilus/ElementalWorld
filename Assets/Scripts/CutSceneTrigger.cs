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

        SceneFlowManager.instance.cutSceneCamera = cutSceneCamera;

        SceneFlowManager.instance.playerCamera.gameObject.SetActive(false);
        SceneFlowManager.instance.cutSceneCamera.gameObject.SetActive(true);
        playerController.enabled = false;
        UIManager.instance.playerHealthUI.SetActive(false);
        UIManager.instance.playerSprintUI.SetActive(false);

        UIManager.instance.dialoguePanel.SetActive(true);

        foreach (string line in introLines)
        {
            UIManager.instance.dialogue.text = line;
            yield return new WaitForSeconds(lineDuration);
        }

        UIManager.instance.dialoguePanel.SetActive(false);

        SceneFlowManager.instance.playerCamera.gameObject.SetActive(true);
        SceneFlowManager.instance.cutSceneCamera.gameObject.SetActive(false);
        playerController.enabled = true;
        UIManager.instance.playerHealthUI.SetActive(true);
        UIManager.instance.playerSprintUI.SetActive(true);

        yield return new WaitForSeconds(lineDuration);

        SceneFlowManager.instance.LoadNextScene(sceneName);
    }
}
