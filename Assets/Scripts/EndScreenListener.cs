using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndScreenListener : MonoBehaviour {
    void OnEnable() {
        // Subscribe to the event
        GlobalEventSystem.OnTransitionToEndScreen.AddListener(HandleTransitionToEndScreen);
    }

    void OnDisable() {
        // Unsubscribe from the event
        GlobalEventSystem.OnTransitionToEndScreen.RemoveListener(HandleTransitionToEndScreen);
    }

    void HandleTransitionToEndScreen() {
        Debug.Log("Received transition event. Waiting 8 seconds...");
        StartCoroutine(WaitAndLoadEndScreen());
    }

    IEnumerator WaitAndLoadEndScreen() {
        // Wait for 8 seconds
        yield return new WaitForSeconds(15f);

        // Load the End Screen scene
        Debug.Log("Loading End Screen...");
        SceneManager.LoadScene("End Screen");
    }
}
