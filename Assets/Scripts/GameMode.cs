using UnityEngine;
using UnityEngine.Audio;

public class GameMode : MonoBehaviour {
    public virtual void Awake() {
        Time.timeScale = 1;
         Debug.Log("This is a debug message!");
        AudioMixer mixer = Resources.Load<AudioMixer>("Main");
         Debug.Log("This is a debug message!2");
        mixer.SetFloat("Music Pitch", 1);
        mixer.SetFloat("Music Pitch Shift", 1);
        mixer.SetFloat("Music Volume", 0);
        mixer.SetFloat("SFX Volume", 0);
    }
    
    int physicsStepsPerFrame = 4;

public virtual void Update()
{
    Utils.SetFramerate();

    float deltaTime = Utils.cappedUnscaledDeltaTime;

    // Limit small fluctuations in deltaTime and ensure it's never zero or negative
    deltaTime = Mathf.Max(
        Mathf.Round(deltaTime * Application.targetFrameRate) / Application.targetFrameRate,
        1F / Application.targetFrameRate
    );

    InputCustom.preventRepeatLock = false;

    // Process the frame in steps to allow catching up on lag
    while (deltaTime > 0)
    {
        // Clamp modDeltaTime to ensure it's valid
        float modDeltaTime = Mathf.Min(deltaTime, 1F / 60F); // Max step size: 1/60 seconds
        modDeltaTime = Mathf.Max(modDeltaTime, 0.0001F); // Minimum step size to avoid zero or negative values

        foreach (GameBehaviour gameBehaviour in GameBehaviour.allGameBehvaiours)
        {
            float gbDeltaTime = gameBehaviour.useUnscaledDeltaTime
                ? modDeltaTime
                : modDeltaTime * Time.timeScale;

            gameBehaviour.UpdateDelta(gbDeltaTime);
        }

        // Run multiple physics simulations per frame to prevent clipping
        for (int i = 0; i < physicsStepsPerFrame; i++)
        {
            float physicsDeltaTime = modDeltaTime * Time.timeScale * (1F / physicsStepsPerFrame);
            if (physicsDeltaTime > 0)
            {
                Physics.Simulate(physicsDeltaTime);
            }
        }

        foreach (GameBehaviour gameBehaviour in GameBehaviour.allGameBehvaiours)
        {
            float gbDeltaTime = gameBehaviour.useUnscaledDeltaTime
                ? modDeltaTime
                : modDeltaTime * Time.timeScale;

            gameBehaviour.LateUpdateDelta(gbDeltaTime);
        }

        // Prevent Input.GetButtonDown from firing multiple times
        InputCustom.preventRepeatLock = true;

        // Decrease deltaTime by modDeltaTime
        deltaTime -= modDeltaTime;
    }
}

}