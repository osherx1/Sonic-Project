using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : GameMode {
    // ========================================================================
    // Helper methods to get the currently loaded LevelManager.
    // If there are multiple LevelManagers loaded at once, this obviously won't work.
    // However, that shouldn't ever need to happen. I hope.

    static LevelManager _current;
    public static LevelManager current { get {
        if (_current == null)
            _current = GameObject.FindObjectOfType<LevelManager>();
        return _current;
    }}
    // ========================================================================
    // Inspector options
    public bool debugMutliplayer = true; // Allows P1 to spawn multiple players
    private string defaultScenePath = @"Resources\Levels\GHZ1\GHZ1\";
    public string defaultSceneName = "GHZ1";
    public string secondSceneName = "GHZ2";
    public string thirdSceneName = "GHZ3";
    public string DefaultSceneName => defaultSceneName;
    public string SecondSceneName => secondSceneName;
    public string ThirdSceneName => thirdSceneName;


    // ========================================================================

    public HashSet<Character> characters = new HashSet<Character>();

    void InitCharacter() {
        if (characters.Count == 0) {
            Character character = Instantiate(
                Resources.Load<GameObject>("Prefabs/Character")
            ).GetComponent<Character>();
            Utils.SetScene(character.transform, "Level");
            Time.timeScale = 0;
        }
        ReloadDisposablesScene();
    }

  void Start()
{
    // Must be run on Start rather than Awake to give any Levels time to spawn
    Utils.SetActiveScene("Level");

    // Check if a Level is already loaded in the scene
    Level levelDefault = FindObjectOfType<Level>();
    if (levelDefault == null)
    {
        // Determine the scene name to load: use defaultScenePath if no scene is provided
        string sceneNameToLoad = string.IsNullOrEmpty(defaultSceneName)
            ? defaultScenePath // Use the default scene name
            : defaultSceneName; // Use the provided scene name

        Debug.Log($"Loading scene: {sceneNameToLoad}");

        // Start asynchronous scene loading
        StartCoroutine(Utils.LoadLevelAsync(
            sceneNameToLoad, // Pass the scene name
            (Level level) => InitCharacter() // Callback after loading the scene
        ));
    }
    else
    {
        // Initialize the character immediately if the scene is already loaded
        InitCharacter();
    }
}

    public override void Update() {
        base.Update();

        // Ensure all temporary objects are loaded into "disposables"
        Scene disposablesCurrent = SceneManager.GetSceneByName("Disposables");

        if (disposablesCurrent.isLoaded)
            SceneManager.SetActiveScene(disposablesCurrent);

        // UpdateStartJoin();
    }

    public void ReloadDisposablesScene() {
        Scene disposablesCurrent = SceneManager.GetSceneByName("Disposables");
        if (disposablesCurrent.isLoaded) {
            foreach(GameObject obj in disposablesCurrent.GetRootGameObjects())
                Destroy(obj);
        } else SceneManager.LoadScene("Scenes/Disposables", LoadSceneMode.Additive);

    }

    // Get the smallest available player Id
    // (Player IDs should always be kept in a sequence, even if a player leaves)
    // (See Character.OnDestroy)
    public int GetFreePlayerId() {
        int id = -1;
        foreach (Character character in characters)
            id = Mathf.Max(id, character.playerId);
        return id + 1;
    }

    public static string GetCurrentLevelName() {
    for (int i = 0; i < SceneManager.sceneCount; i++) {
        Scene scene = SceneManager.GetSceneAt(i);
        if (scene.isLoaded) {
            foreach (GameObject obj in scene.GetRootGameObjects()) {
                if (obj.GetComponent<Level>() != null) {
                    return scene.name; // Return the name of the level scene
                }
            }
        }
    }
    Debug.LogError("Current level scene not found!");
    return null;
}

public static string GetNextScene(string currentSceneName) {
    if (currentSceneName == current.DefaultSceneName) {
        return current.SecondSceneName;
    } else if (currentSceneName == current.SecondSceneName) {
        return current.ThirdSceneName;
    } else if (currentSceneName == current.ThirdSceneName) {
        return "End Screen"; // Replace with your actual end screen name
    }

    Debug.LogWarning("No next scene found for current scene: " + currentSceneName);
    return null;
}



}