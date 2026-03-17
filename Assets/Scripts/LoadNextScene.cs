using UnityEngine;

public class LoadNextScene : MonoBehaviour
{
    /// <summary>
    /// Handles the event when another collider enters the trigger area.
    /// </summary>
    /// <remarks>If the entering collider is tagged as "Player", this method initiates the process to open the
    /// path to Solaris via the game manager.</remarks>
    /// <param name="other">The collider that has entered the trigger area. Must not be <see langword="null"/>.</param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.OpenPathToSolaris();
        }
    }
}
