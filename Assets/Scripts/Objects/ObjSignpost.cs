using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjSignpost : MonoBehaviour {
    Animator animator;
    AudioSource audioSource;
    public UnityEvent onNextLevel;

    void InitReferences() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        InitReferences();
    }

    HashSet<Character> charactersTouched = new HashSet<Character>();

    void Touch(Character character) {
        if (charactersTouched.Contains(character)) return;
        charactersTouched.Add(character);
        animator.Play("Spin");
        audioSource.Play();

        // Instantiate the ObjLevelClear object and configure it
        ObjLevelClear levelClearObj = Instantiate(
            Constants.Get<GameObject>("prefabLevelClear"),
            Vector3.zero,
            Quaternion.identity
        ).GetComponent<ObjLevelClear>();

        levelClearObj.character = character;
        levelClearObj.onNextLevel = onNextLevel;

        // Determine and set the next level using LevelManager
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        string nextScene = "";

        if (currentScene == LevelManager.current.DefaultSceneName) {
            nextScene = LevelManager.current.SecondSceneName;
        } else if (currentScene == LevelManager.current.SecondSceneName) {
            nextScene = LevelManager.current.ThirdSceneName;
        } else if (currentScene == LevelManager.current.ThirdSceneName) {
            nextScene = "End Screen"; // Replace with your actual end screen scene name
        }

        // levelClearObj.nextLevelName = nextScene; // Add this field to ObjLevelClear to track the next scene name

        character.timerPause = true;

        if (character.characterCamera != null) {
            character.characterCamera.LockHorizontal(transform.position.x);
            character.characterCamera.SetCharacterBoundsFromCamera();
        }

        Debug.Log("Triggering transition event...");
        GlobalEventSystem.OnTransitionToEndScreen.Invoke(true);

        // Call StartNextLevel to handle the level transition
        // levelClearObj.StartNextLevel(character.currentLevel);
    }

    void OnTriggerEnter(Collider other) {
        Character[] characters = other.gameObject.GetComponentsInParent<Character>();
        if (characters.Length == 0) return;
        Character character = characters[0];
        Touch(character);
    }
}
