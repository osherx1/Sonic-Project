using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    static Dictionary<string, string> defaults = new Dictionary<string, string> {
        ["sfxDie"] = "Sound/SFX/Sonic 1/S1_A3",
        ["sfxDrown"] = "Sound/SFX/Sonic 1/S1_B2",
        ["sfxDropDashCharge"] = "Sound/SFX/Sonic 2/S2_60",
        ["sfxDropDashRelease"] = "Sound/SFX/Sonic 1/S1_BC",
        ["sfxSkid"] = "Sound/SFX/Sonic 1/S1_A4",
        ["sfxHomingAttack"] = "Sound/SFX/Megamix/Thok",
        ["sfxJump"] =  "Sound/SFX/Sonic 1/S1_A0",
        ["sfxLightDash"] = "Sound/SFX/Sonic 1/S1_BC",
        ["sfxPeelOutCharge"] = "Sound/SFX/Sonic CD/SCD_FM_11",
        ["sfxPeelOutRelease"] = "Sound/SFX/Sonic CD/SCD_FM_01",
        ["sfxRoll"] = "Sound/SFX/Sonic 1/S1_BE",
        ["sfxSpindashCharge"] = "Sound/SFX/Sonic 2/S2_60",
        ["sfxSpindashRelease"] = "Sound/SFX/Sonic 1/S1_BC",
        ["sfxHurt"] = "Sound/SFX/Sonic 1/S1_A3",
        ["sfxDieSpikes"] = "Sound/SFX/Sonic 1/S1_A6",
        ["sfxHurtRings"] = "Sound/SFX/Sonic 1/S1_C6",
        ["sfxTallyBeep"] = "Sound/SFX/Sonic 1/S1_CD",
        ["sfxTallyChaChing"] = "Sound/SFX/Sonic 1/S1_C5",
        ["sfxRingMonitor"] = "Sound/SFX/Sonic 1/S1_B5",
        ["sfxShieldNormal"] = "Sound/SFX/Sonic 1/S1_AF",
        ["sfxBossHit"] = "Sound/SFX/Sonic 1/S1_AC",

        ["prefabDropDashDust"] = "Prefabs/Dash Dust (Drop Dash)",
        ["prefabSpindashDust"] = "Prefabs/Dash Dust (Spindash)",
        ["prefabAnimal"] = "Prefabs/Animal",
        ["prefabExplosionEnemy"] = "Prefabs/Explosion (Enemy)",
        ["prefabExplosionBoss"] = "Prefabs/Explosion (Boss)",
        ["prefabPoints"] = "Prefabs/Points",
        ["prefabShieldNormal"] = "Prefabs/Shield (Normal)",
        ["prefabShieldFire"] = "Prefabs/Shield (Fire)",
        ["prefabShieldElectricity"] = "Prefabs/Shield (Electricity)",
        ["prefabShieldBubble"] = "Prefabs/Shield (Bubble)",
        ["prefabRing"] = "Prefabs/Ring",
        ["prefabLevelClear"] = "Prefabs/Level Clear",
        ["prefabScreenFadeOut"] = "Prefabs/Screen Fade Out",
        ["prefabTitleCard"] = "Prefabs/Title Card",
        ["prefabInvincibilityStars"] = "Prefabs/Invincibility Stars",
        ["prefabRingSparkle"] = "Prefabs/Ring Sparkle",
        ["prefabGHZBall"] = "Prefabs/GHZ Rolling Ball"
    };

    public static string Get(string key) => defaults[key];

    static Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();

    public static T Get<T>(string key) {
        string data = Get(key);

        if (typeof(T) == typeof(bool))
            return (T)(object)Utils.StringBool(data);

        if (typeof(T) == typeof(GameObject)) {
            if (!prefabCache.ContainsKey(data)) {
                prefabCache[data] = (GameObject)Resources.Load(data);
            }
            
            return (T)(object)prefabCache[data];
        }

        return (T)(object)data;
    }
}