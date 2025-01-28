using UnityEngine;

public class CharacterSonic : Character {
    public override void Start() {
        capabilities.Add(new CharacterCapabilityGround(this));
        capabilities.Add(new CharacterCapabilityAir(this));
        capabilities.Add(new CharacterCapabilityHurt(this));
        
        //capabilities.Add(new CharacterCapabilitySpindash(this));
        //capabilities.Add(new CharacterCapabilityPeelOut(this));
        capabilities.Add(new CharacterCapabilityDropdash(this));
        capabilities.Add(new CharacterCapabilityHomingAttack(this));
        //capabilities.Add(new CharacterCapabilityLightDash(this));
            
        capabilities.Add(new CharacterCapabilityJump(this));
        capabilities.Add(new CharacterCapabilityRolling(this));
        capabilities.Add(new CharacterCapabilityRollingAir(this));
        capabilities.Add(new CharacterCapabilityVictory(this));
        capabilities.Add(new CharacterCapabilityDeath(this));
        
        capabilities.Add(new CharacterCapabilityAfterImage(this));
            
        base.Start();
    }
}