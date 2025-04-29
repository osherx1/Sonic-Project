using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public Character character;
    Canvas canvas;

    Text scoreText;
    Text timeText;
    Text timeTitleText;
    Text ringsText;
    Text ringsTitleText;
    Text livesText;

    void Awake() {
        canvas = GetComponent<Canvas>();
        scoreText = transform.Find("Score Content").GetComponent<Text>();
        timeText = transform.Find("Time Content").GetComponent<Text>();
        timeTitleText = transform.Find("Time Title").GetComponent<Text>();
        ringsText = transform.Find("Rings Content").GetComponent<Text>();
        ringsTitleText = transform.Find("Rings Title").GetComponent<Text>();
        livesText = transform.Find("Lives Content").GetComponent<Text>();
    }
    StringBuilder sb = new StringBuilder("", 50);

    public void Update() {
        canvas.worldCamera = character.characterCamera.camera;
        //Debug.Log(character.characterCamera.camera);

        scoreText.text = Utils.IntToStrCached(character.score);
        ringsText.text = Utils.IntToStrCached(character.rings);
        livesText.text = Utils.IntToStrCached(character.lives);

        int minutes = (int)(character.timer / 60);
        int seconds = (int)(character.timer % 60);

        sb.Clear();
        sb.Append(Utils.IntToStrCached(minutes));
        sb.Append(":");
        if (seconds < 10) sb.Append("0");
        sb.Append(Utils.IntToStrCached(seconds));
        
        
        timeText.text = sb.ToString();

        bool shouldFlash = (((int)(Time.unscaledTime * 60)) % 16) > 8;
        if (shouldFlash) {
            if (character.rings <= 0) ringsTitleText.color = Color.red;
            
            timeTitleText.color = Color.white;
            ringsTitleText.color = Color.white;
        }
    }
}
