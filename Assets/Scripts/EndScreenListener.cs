using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenListener : MonoBehaviour {

    public ObjGameOver objGameOver;

    void OnEnable() {
        // Subscribe to the event
        GlobalEventSystem.OnTransitionToEndScreen.AddListener(HandleTransitionToEndScreen);
        GlobalEventSystem.OnActivateGameOver.AddListener(ActivateGameOver);
    }

    void OnDisable() {
        // Unsubscribe from the event
        GlobalEventSystem.OnTransitionToEndScreen.RemoveListener(HandleTransitionToEndScreen);
        GlobalEventSystem.OnActivateGameOver.RemoveListener(ActivateGameOver);
    }

    void HandleTransitionToEndScreen(bool shouldWait) {
        Debug.Log($"Received transition event. Should wait: {shouldWait}");
        StartCoroutine(WaitAndLoadEndScreen(shouldWait));
    }

    IEnumerator WaitAndLoadEndScreen(bool shouldWait) {
        if (shouldWait) {
            Debug.Log("Waiting 10 seconds before loading End Screen...");
            yield return new WaitForSeconds(10f); // Adjusted to 10 seconds per the log statement
            Debug.Log("Loading End Screen...");
            SceneManager.LoadScene("End Screen");
        } else {
            Debug.Log("Loading BAD End Screen...");
            SceneManager.LoadScene("End Screen BAD");
        }
    }


    void ActivateGameOver()
    {
        Debug.Log("Activating Game Over canvas...");
        if (objGameOver != null)
        {
            objGameOver.ActivateGameOver();
        }
        else
        {
            Debug.LogWarning("ObjGameOver reference is missing! Ensure it is assigned in the Inspector.");
        }
    }
}
