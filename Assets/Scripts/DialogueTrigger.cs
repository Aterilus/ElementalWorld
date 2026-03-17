using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string dialogueName;

    bool isTriggered = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            GameManager.instance.ShowDialogue(dialogueName);
        }
    }
}
