using UnityEngine;

public class CharacterCapabilityCheats : CharacterCapability
{
    public CharacterCapabilityCheats(Character character) : base(character) { }

    public override void Init()
    {
        name = "cheats";
    }

    public override void Update(float deltaTime)
    {
        // Example: Soft Respawn with Ctrl+Q
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Ctrl+Q pressed: Performing full soft respawn...");
            character.SoftRespawn();
        }

        // Example: Reset position with Ctrl+W
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Ctrl+W pressed: Resetting character position...");
            ResetPositionToSpawn();
        }
    }

    private void ResetPositionToSpawn()
    {
        if (character.currentLevel != null)
        {
            character.position = character.currentLevel.spawnPosition;
            character.velocity = Vector3.zero; // Reset velocity
            Debug.Log($"Character position reset to {character.currentLevel.spawnPosition}.");
        }
        else
        {
            Debug.LogWarning("Current level is null, cannot reset position.");
        }
    }
}
