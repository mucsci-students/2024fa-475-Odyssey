using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Import Scene Management

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    // The name or build index of the scene to load
    public string sceneName;

    // so that player object persists between scenes
    void Awake()
    {
        // keep the same player instance across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // keep object between scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate player objects
        }
    }
    
    // Detect the player entering the trigger zone
   public void GoBoss()
    {
            // Load the specified scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}