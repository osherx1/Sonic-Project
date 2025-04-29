using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ObjGameOver : MonoBehaviour
{
    private AudioSource audioSource;
    private Canvas canvas;
    private bool isActive = false;
    public float delayBeforeComplete = 20f;

    private float gameOverTimer = 0f;
    private bool isGameOverActive = false;


     void Update()
    {
        // If the Game Over is active, count down the timer
        if (isGameOverActive)
        {
            gameOverTimer -= Time.unscaledDeltaTime;

            if (gameOverTimer <= 0f)
            {
                Debug.Log("Delay complete. Invoking OnTransitionToEndScreen...");
                GlobalEventSystem.OnTransitionToEndScreen.Invoke(false);
                isGameOverActive = false; // Stop the timer
            }
        }
    }

    void InitReferences()
    {
        audioSource = GetComponent<AudioSource>();
        canvas = GetComponent<Canvas>();

        if (canvas == null)
        {
            Debug.LogWarning("Canvas component is missing on the Game Over prefab.");
        }
        else
        {
            AssignCameraToCanvas();
        }
    }

    void Awake()
    {
        InitReferences();
    }

    void AssignCameraToCanvas()
    {
        Camera mainCamera = null;

        // Loop through all loaded scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            // Check if the scene is loaded
            if (scene.isLoaded)
            {
                // Find all root GameObjects in the scene
                GameObject[] rootObjects = scene.GetRootGameObjects();

                // Search for a Camera component in the root objects
                foreach (GameObject obj in rootObjects)
                {
                    Camera camera = obj.GetComponentInChildren<Camera>();
                    if (camera != null && camera.gameObject.activeInHierarchy)
                    {
                        mainCamera = camera;
                        break;
                    }
                }

                // Break out of the loop if a camera is found
                if (mainCamera != null) break;
            }
        }

        // Assign the found camera to the Canvas
        if (mainCamera != null)
        {
            canvas.worldCamera = mainCamera;
            Debug.Log($"Assigned camera '{mainCamera.name}' to the Canvas.");
        }
        else
        {
            Debug.LogWarning("No active camera found in the loaded scenes.");
        }
    }


    public void ActivateGameOver()
    {
        if (isActive) return; // Prevent reactivation
        isActive = true;

        // Reactivate the GameObject
        gameObject.SetActive(true);



        if (canvas != null)
            canvas.enabled = true;

        // Assign the camera again in case it wasn't assigned earlier
        AssignCameraToCanvas();

        MusicManager.current.Clear(); // Clear the music stack
        MusicManager.current.audioSourceIntro.Stop(); // Stop the intro music
        MusicManager.current.audioSourceLoop.Stop();  // Stop the looping music
        Debug.Log("Music stopped by ObjGameOver.");

        // Play audio if an AudioSource is assigned
        if (audioSource != null)
            audioSource.Play();

        isGameOverActive = true;
        gameOverTimer = delayBeforeComplete;

        // Start Game Over handling flow
        // StartCoroutine(HandleGameOver());
    }

    // IEnumerator HandleGameOver()
    // {
    //     // Wait for the specified delay
    //     Debug.Log("Starting Game Over delay...");
    //     yield return new WaitForSeconds(delayBeforeComplete);
    //     Debug.Log("Delay complete. Invoking OnTransitionToEndScreen...");

    //     // Invoke any custom logic after Game Over (e.g., UI buttons, scene transitions)
    //     //onGameOverComplete?.Invoke();

    //     GlobalEventSystem.OnTransitionToEndScreen.Invoke(false);
    // }

}
