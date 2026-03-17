using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    /// <summary>
    /// Resumes the game if it is currently paused.
    /// </summary>
    /// <remarks>This method restores gameplay by unpausing the game. If the game is not paused, calling this
    /// method has no effect.</remarks>
    public void Resume()
    {
        GameManager.instance.UnPaused();
    }

    /// <summary>
    /// Restarts the current scene, resetting its state to the initial configuration.
    /// </summary>
    /// <remarks>This method reloads the active scene and resumes gameplay if it was previously paused. Any
    /// unsaved progress or state in the current scene will be lost upon restart.</remarks>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.UnPaused();
    }

    public void Save()
    {

    }

    public void Settings()
    {

    }

    /// <summary>
    /// Exits the application or stops play mode in the Unity Editor.
    /// </summary>
    /// <remarks>When running in the Unity Editor, this method stops play mode. When running a built
    /// application, it terminates the application. This method is typically used to provide a user-initiated exit or to
    /// handle shutdown scenarios.</remarks>
    public void Quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}
