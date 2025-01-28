using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloadHelper : MonoBehaviour
{
    private static SceneReloadHelper _instance;

    public static SceneReloadHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject helperObject = new GameObject("SceneReloadHelper");
                _instance = helperObject.AddComponent<SceneReloadHelper>();
                DontDestroyOnLoad(helperObject); // Ensure it persists across scenes
            }
            return _instance;
        }
    }

    public void ReloadScene(string sceneName)
    {
        Debug.Log($"Reloading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}
