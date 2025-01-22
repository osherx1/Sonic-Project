using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObjLevelClear : MonoBehaviour {
    Animator animator;
    AudioSource audioSource;
    Canvas canvas;
    Text scoreTextComponent;
    Text ringTextComponent;
    Text timeTextComponent;
    Text actTextComponent;

    void InitReferences() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        canvas = GetComponent<Canvas>();
        scoreTextComponent = transform.Find("Score Line/Score Value").GetComponent<Text>();
        ringTextComponent = transform.Find("Ring Line/Ring Bonus Value").GetComponent<Text>();
        timeTextComponent = transform.Find("Time Line/Time Bonus Value").GetComponent<Text>();
        actTextComponent = transform.Find("Act Group/Act Value").GetComponent<Text>();
    }

    void Start() {
        InitReferences();
    }

    public UnityEvent onNextLevel;

    public Character character;
    float showTimer = 2F;
    float tallyTimer = 4F;
    float tallyFrameTimer = 0;
    float endTimer = 3F;

    int GetTimeBonus(float time) {
        if (time < 30) return 50000;
        if (time < 45) return 10000;
        if (time < 60) return 5000;
        if (time < 90) return 4000;
        if (time < 120) return 3000;
        if (time < 180) return 2000;
        if (time < 240) return 1000;
        if (time < 300) return 500;
        if (time > 24 * 60 * 60) return 1;
        return 0;
    }

    int timeBonus = 0;
    int ringBonus = 0;

    void StartNextLevel(Level nextLevel) {
        character.currentLevel = nextLevel;
        character.timer = 0;
        character.rings = 0;
        character.respawnData.position = character.currentLevel.spawnPosition;
        character.respawnData.timer = 0;
        character.checkpointId = 0;

        character.timerPause = false;
        character.TryGetCapability("victory", (CharacterCapability capability) => {
            ((CharacterCapabilityVictory)capability).victoryLock = false;
        });
        character.positionMax = Mathf.Infinity * Vector2.one;

        if (character.characterCamera != null)
            character.characterCamera.maxPosition = Mathf.Infinity * Vector2.one;

        ObjTitleCard.Make(character, false);

        Destroy(gameObject);
    }

    public void LoadNextLevel() {
        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = "";

        // Determine the next scene based on the current scene
        if (currentScene == LevelManager.current.DefaultSceneName) {
            nextScene = LevelManager.current.SecondSceneName;
        } else if (currentScene == LevelManager.current.SecondSceneName) {
            nextScene = LevelManager.current.ThirdSceneName;
        } else if (currentScene == LevelManager.current.ThirdSceneName) {
            nextScene = "End Screen"; // Replace with your actual end screen scene name
        }

        // If a next scene is found, load Disposables first
        if (!string.IsNullOrEmpty(nextScene)) {
            StartCoroutine(LoadDisposablesAndNextScene(nextScene));
        } else {
            onNextLevel.Invoke();
        }
    }

IEnumerator LoadDisposablesAndNextScene(string nextScene) {
    // Ensure Disposables scene is loaded

    if (!SceneManager.GetSceneByName("Disposables").isLoaded) {
        Debug.Log("Loading Disposables scene...");
        AsyncOperation loadDisposables = SceneManager.LoadSceneAsync("Disposables", LoadSceneMode.Additive);
        while (!loadDisposables.isDone) {
            yield return null;
        }
        Debug.Log("Disposables scene loaded.");
    }

    // Trigger the event to notify listeners
    

    // Set Disposables as the active scene
    Scene disposablesScene = SceneManager.GetSceneByName("Disposables");
    if (disposablesScene.isLoaded) {
        SceneManager.SetActiveScene(disposablesScene);
        Debug.Log("Disposables scene set as active.");
    }


    // Unload non-essential scenes if needed (e.g., GHZ1)
    Scene ghz1Scene = SceneManager.GetSceneByName("GHZ1");
    if (ghz1Scene.isLoaded) {
        Debug.Log("Unloading GHZ1...");
        SceneManager.UnloadSceneAsync(ghz1Scene);
    }

    Scene levelScene = SceneManager.GetSceneByName("Level");
    if (levelScene.isLoaded) {
        Debug.Log("Unloading Level...");
        SceneManager.UnloadSceneAsync(levelScene);
    }

    yield break; // End coroutine as the listener handles the remaining logic
}



    // Update is called once per frame
    void Update() {
        if (endTimer <= 0) return;

        scoreTextComponent.text = character.score.ToString();
        ringTextComponent.text = ringBonus.ToString();
        timeTextComponent.text = timeBonus.ToString();

        if (showTimer > 0) {
            showTimer -= Utils.cappedDeltaTime;
            if (showTimer <= 0) {
                timeBonus = GetTimeBonus(character.timer);
                ringBonus = character.rings * 100;

                if (character.characterCamera != null)
                    canvas.worldCamera = character.characterCamera.camera;

                actTextComponent.text = character.currentLevel.act.ToString();

                character.TryGetCapability("victory", (CharacterCapability capability) => {
                    ((CharacterCapabilityVictory)capability).victoryLock = true;
                });
                character.effects.Clear();

                MusicManager.current.Play(new MusicManager.MusicStackEntry {
                    introPath = "Music/Level Clear"
                });
            }
            return;
        }

        if (tallyTimer > 0) {
            tallyTimer -= Utils.cappedDeltaTime;
            return;
        }

        if ((timeBonus > 0) || (ringBonus > 0)) {
            if (tallyFrameTimer > 0) {
                tallyFrameTimer -= Utils.cappedDeltaTime;
                if (tallyFrameTimer <= 0)
                    tallyFrameTimer = 1F / 60F;
                else return;
            }

            int transferAmtTime = Mathf.Min(100, timeBonus);
            int transferAmtRing = Mathf.Min(100, ringBonus);

            if (Input.GetButtonDown("Pause")) {
                transferAmtTime = timeBonus;
                transferAmtRing = ringBonus;
            }

            timeBonus -= transferAmtTime;
            ringBonus -= transferAmtRing;

            character.score += transferAmtTime;
            character.score += transferAmtRing;
            SFX.Play(audioSource, "sfxTallyBeep");

            if ((timeBonus <= 0) && (ringBonus <= 0)) {
                SFX.Play(audioSource, "sfxTallyChaChing");
                animator.Play("Items Exit");
            }
            return;
        }

        if (endTimer > 0) {
            endTimer -= Utils.cappedDeltaTime;
            if (endTimer <= 0) LoadNextLevel();
        }
    }
}
