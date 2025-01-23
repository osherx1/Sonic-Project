using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenListener : MonoBehaviour {
    void OnEnable() {
        // Subscribe to the event
        GlobalEventSystem.OnTransitionToEndScreen.AddListener(HandleTransitionToEndScreen);
    }

    void OnDisable() {
        // Unsubscribe from the event
        GlobalEventSystem.OnTransitionToEndScreen.RemoveListener(HandleTransitionToEndScreen);
    }

    void HandleTransitionToEndScreen(bool shouldWait) {
        Debug.Log($"Received transition event. Should wait: {shouldWait}");
        StartCoroutine(WaitAndLoadEndScreen(shouldWait));
    }

    IEnumerator WaitAndLoadEndScreen(bool shouldWait) {
        if (shouldWait) {
            Debug.Log("Waiting 10 seconds before loading End Screen...");
            yield return new WaitForSeconds(12f);

            Debug.Log("Loading End Screen...");
            SceneManager.LoadScene("End Screen");
        }

        // Load the End Screen scene
        Debug.Log("Loading BAD End Screen...");
        SceneManager.LoadScene("End Screen BAD");
    }
}
