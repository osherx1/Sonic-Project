using UnityEngine;
using System.Collections.Generic;

public class CharacterCapabilityHomingAttack : CharacterCapability {
    string[] buttonsHomingAttack = new string[] { "Secondary", "Tertiary" };
    
    float failsafeTimer;

    public CharacterCapabilityHomingAttack(Character character) : base(character) { }

    Transform target;
    CharacterEffect afterImageEffect;

    public override void Init() {
        name = "homingAttack";
        character.AddStateGroup("rolling", "homingAttack");
        character.AddStateGroup("airCollision", "homingAttack");
        character.AddStateGroup("harmful", "homingAttack");

        character.stats.Add(new Dictionary<string, object>() {
            ["homingAttackSpeed"] = 9F,
            ["homingAttackBounceSpeed"] = 6.5F
        });
    }

    public override void StateInit(string stateName, string prevStateName) {
        if (character.stateCurrent != "homingAttack") return;

        failsafeTimer = 5F;        
        SFX.PlayOneShot(character.audioSource, "sfxHomingAttack");
        
        target = FindClosestTarget();

        if (target == null) {
            character.velocity = new Vector2(
                (
                    character.stats.Get("homingAttackSpeed") *
                    (character.facingRight ? 1 : -1)
                ),
                0
            );
            character.stateCurrent = "rollingAir";
            character.effects.Add(new CharacterEffect(character, "afterImage", 0.25F));
        } else {
            character.velocity = Vector3.zero;
            character.modeGroupCurrent = character.rollingAirModeGroup;
            afterImageEffect = new CharacterEffect(character, "afterImage");
            character.effects.Add(afterImageEffect);
        }
    }

    public override void StateDeinit(string stateName, string nextStateName) {
        if (character.stateCurrent != "homingAttack") return;
        if (afterImageEffect != null)
            afterImageEffect.DestroyBase();
    }

    public override void Update(float deltaTime) {
        if (character.stateCurrent == "jump") {
            if (character.input.GetButtonsDownPreventRepeat(buttonsHomingAttack))
                character.stateCurrent = "homingAttack";
        }

        if (character.stateCurrent != "homingAttack") return;

        if (target == null) {
            character.stateCurrent = "rollingAir";
        } else {
            character.position = Vector3.MoveTowards(
                character.position,
                target.position,
                character.stats.Get("homingAttackSpeed") * deltaTime * 2
            );

            failsafeTimer -= deltaTime;
            if (failsafeTimer <= 0)
                character.stateCurrent = "rollingAir";
        }
    }

    public override void OnCollisionEnter(Collision collision) {
        if (character.stateCurrent != "homingAttack") return;
        if (collision.collider.isTrigger) return;
        character.stateCurrent = "rollingAir";        
    }

    public override void OnTriggerEnter(Collider other) {
        if (character.stateCurrent != "homingAttack") return;
        HomingAttackTarget[] targets = other.gameObject.GetComponentsInParent<HomingAttackTarget>();
        if (targets.Length == 0) return;

        character.velocity = new Vector2(
            0,
            character.stats.Get("homingAttackBounceSpeed")
        );
        character.stateCurrent = "jump";
    }

    // https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    Transform FindClosestTarget(float distanceLimit = 24F) {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach(HomingAttackTarget target in GameObject.FindObjectsOfType<HomingAttackTarget>()) {
            Transform potentialTarget = target.transform;
            if (!potentialTarget.gameObject.activeSelf) continue;
            if (
                (potentialTarget.position.x <= character.position.x) &&
                character.facingRight
            ) continue;
            if (
                (potentialTarget.position.x >= character.position.x) &&
                !character.facingRight
            ) continue;
            if (!target.enabled) continue;

            Vector3 directionToTarget = potentialTarget.position - character.position;
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