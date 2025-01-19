using UnityEngine;
using System.Collections.Generic;

public class CharacterCapabilityLightDash : CharacterCapability {
    float failsafeTimer;

    public CharacterCapabilityLightDash(Character character) : base(character) { }

    ObjRing target;
    CharacterEffect afterImageEffect;

    public override void Init() {
        name = "lightDash";
        character.AddStateGroup("harmful", "lightDash");

        character.stats.Add(new Dictionary<string, object>() {
            ["lightDashSpeed"] = 9F
        });
    }

    public override void StateInit(string stateName, string prevStateName) {
        if (character.stateCurrent != "lightDash") return;
        
        failsafeTimer = 5F;
        SFX.Play(character.audioSource, "sfxLightDash");
        character.velocity = Vector3.zero;
        character.modeGroupCurrent = character.airModeGroup;
        afterImageEffect = new CharacterEffect(character, "afterImage");
        character.effects.Add(afterImageEffect);
        character.AnimatorPlay("Light Dash");
    }

    public override void StateDeinit(string stateName, string nextStateName) {
        if (character.stateCurrent != "lightDash") return;
        if (afterImageEffect != null)
            afterImageEffect.DestroyBase();
    }

    Vector3 positionPrev;

    public override void Update(float deltaTime) {
        if (character.stateCurrent != "lightDash") {
            if (!character.input.GetButtonDownPreventRepeat("Primary")) return;
            target = FindClosestTarget(true);
            if (target != null) character.stateCurrent = "lightDash";
            return;
        };

        if ((target == null) || (target.collected)) {            
            target = FindClosestTarget();
            if (target != null) character.stateCurrent = "lightDash";
            else {
                character.velocity = (character.position - positionPrev) * 60F * 0.75F;
                if (character.velocity.magnitude > character.stats.Get("terminalSpeed"))
                    character.velocity = character.stats.Get("terminalSpeed") * character.velocity.normalized;
                    
                character.stateCurrent = "air";
                character.AnimatorPlay("Fast");
            }
        } else {
            positionPrev = character.position;
            character.forwardAngle = character.position.AngleTowards(target.transform.position);
            if (character.flipX) character.forwardAngle += 180F;
            Vector3 newPos = Vector3.MoveTowards(
                character.position,
                target.transform.position,
                character.stats.Get("lightDashSpeed") * deltaTime * 2
            );
            newPos.z = character.position.z;
            character.position = newPos;

            character.spriteContainer.transform.eulerAngles = character.GetSpriteRotation(deltaTime);

            failsafeTimer -= deltaTime;
            if (failsafeTimer <= 0)
                character.stateCurrent = "rollingAir";
        }
    }

    // https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    ObjRing FindClosestTarget(bool inital = false, float distanceLimit = 4F) {
        ObjRing bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach(ObjRing target in GameObject.FindObjectsOfType<ObjRing>()) {
            ObjRing potentialTarget = target;
            if (!potentialTarget.gameObject.activeSelf) continue;
            if (!target.enabled) continue;
            if (target.collected) continue;
            if (target.falling) continue;
            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(
                character.position.AngleTowards(potentialTarget.transform.position),
                character.forwardAngle
            ));
            if ((angleDiff > 45) && (angleDiff < 135)) continue;
            else if (inital) {
                if ((angleDiff > 45) && character.facingRight) continue;
                else if ((angleDiff < 135) && !character.facingRight) continue;
            }

            Vector3 directionToTarget = potentialTarget.transform.position - character.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        if (closestDistanceSqr > distanceLimit) return null;
        return bestTarget;
    }
}