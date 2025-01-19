using UnityEngine;
using System.Collections.Generic;

public class CharacterCapabilityDeath : CharacterCapability {
    float dyingTimer;

    public CharacterCapabilityDeath(Character character) : base(character) { }

    public override void Init() {
        name = "dying";
        character.AddStateGroup("death", "dying");
        character.AddStateGroup("death", "drowning");
        character.AddStateGroup("death", "dead");
        character.AddStateGroup("noControl", "dying");
        character.AddStateGroup("noControl", "drowning");
        character.AddStateGroup("noControl", "dead");
        character.AddStateGroup("ignore", "dying");
        character.AddStateGroup("ignore", "drowning");
        character.AddStateGroup("ignore", "dead");
        character.AddStateGroup("dying", "dying");
        character.AddStateGroup("dying", "drowning");
        character.AddStateGroup("dead", "dead");

        character.stats.Add(new Dictionary<string, object>() {
            ["gravityDying"] = -0.21875F
        });
    }

    public override void StateInit(string stateName, string prevStateName) {
        if (!character.InStateGroup("death")) return;
        character.modeGroupCurrent = null;

        if (character.InStateGroup("dying")) {
            if (LevelManager.current.characters.Count == 1)
                Time.timeScale = 0;

            character.shield = null;
            character.ClearEffects();
            dyingTimer = 3F;

            if (character.stateCurrent == "drowning") {
                character.velocity = Vector3.zero;
                SFX.Play(character.audioSource, "sfxDrown");
                character.AnimatorPlay("Drowning");
            } else if (character.stateCurrent == "dying") {
                character.velocity = new Vector3(
                    0, 7 * character.physicsScale, 0
                );
                SFX.Play(character.audioSource, "sfxDie");
                character.AnimatorPlay("Dying");
            }
        } else if (character.stateCurrent == "dead") {
            character.lives--;
            if (LevelManager.current.characters.Count == 1) {
                character.currentLevel.ReloadFadeOut(character);
            } else character.SoftRespawn();
        }
    }

    public override void Update(float deltaTime) {
        if (!character.InStateGroup("dying")) return;

        character.velocity += Vector3.up * character.stats.Get("gravityDying") * deltaTime * 60F;
        
        dyingTimer -= deltaTime;
        if (dyingTimer <= 0) {
            character.stateCurrent = "dead";
            return;
        }
        
        character.spriteContainer.position = character.position;
        character.spriteContainer.eulerAngles = Vector3.zero;
        character.flipX = false;

        if (Time.timeScale == 0)
            character.position += character.velocity * deltaTime;
    }
}