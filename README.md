Sonic Project
Original Green Hill Zone Act 1 (Sonic the Hedgehog, 1991)
- First playable level: a lush, grassy landscape on “South Island” with checkered ground patterns, waterfalls, and distant hills.
- Signature landmarks: palm trees, loop-de-loops, springs, spikes, and early “Badnik” enemies (e.g. Motobug, Crabmeat).
- Collectibles & hazards: golden rings scattered along the path; breakable platforms that collapse after you land on them.
- Layout: a mostly linear route to the right, with a fork leading either over collapsing platforms or down to encounter spikes first.

Course Context
- Assignment: Recreate a game between 1978-1992, Beacuse i chose Sonic i needed to recretate GHZ1 Act 1 as a Unity 2D platformer for the Computer Game Development program at Hebrew University & Bezalel.
- Goals: Capture the feel of Sonic’s original level—speedy traversal, environmental interaction, and simple enemy encounters.

Your Unity Implementation
1. Tilemap-based Level
   – Built the ground, slopes, and loop sections using Unity’s 2D Tilemap.
2. Parallax Scrolling
   – Implemented multiple background layers (hills, trees, clouds) moving at different speeds to convey depth.
3. Player Controller (PlayerController.cs)
   – 2D physics for running, jumping, and the “drop dash.”
   – Ground-and-air acceleration tuned to feel responsive.
4. Camera Follow (CameraFollow.cs)
   – Smooth X/Y tracking of the player, with configurable margins to show more of the upcoming terrain.
5. Collectibles & Hazard Logic
   – Ring pickup via trigger colliders; ring count display stubbed for UI.
   – Collapsing platforms and basic Badnik “patrol” enemy prefabs.
6. Boundary Checks
   – Prevented the player from leaving the playable area by clamping position within level bounds.

Folder Structure:
sonic-project/
├── .gitignore             ← Git ignore file
├── README.md              ← project guide
└── Assets/
    ├── Scenes/            ← Unity scene files (e.g. GHZ1.unity, Level1.unity)
    ├── Scripts/           ← C# scripts (PlayerController.cs, CameraFollow.cs)
    ├── PathCreator/       ← external plugin for path creation (Editor & Runtime)
    ├── Resources/         ← runtime-loaded assets
    │   ├── End Screen/    ← UI and animations for level completion
    │   ├── Fonts/         ← TextMeshPro fonts
    │   ├── Levels/        ← lighting, palette, and color settings per level
    │   └── Misc/          ← miscellaneous assets (shaders, materials)
    ├── Prefabs/           ← reusable prefabs (platforms, enemies, etc.)
    ├── Tilemaps/          ← tilemaps for level layouts
    ├── Sound/             ← audio files and mixers
    └── Settings/          ← project settings (URP, input system, etc.)


Screenshots:

![image](https://github.com/user-attachments/assets/d6cb08a5-cf80-47b6-8e68-6a9116c6547c)


![image](https://github.com/user-attachments/assets/f94445f4-5459-4bad-9066-9489d280b774)


![image](https://github.com/user-attachments/assets/5c1da9fb-7b72-4c81-9c37-7601c8a7cdd6)

![image](https://github.com/user-attachments/assets/4165aeac-b20b-4267-abaa-115bf02d3126)

    
Cheats in the game:
The game includes several cheat commands that can be used for testing, debugging, or exploring the game more freely. Below is a list of the cheats, what they do, and how to activate them.

1. Soft Respawn
What it does:
Resets the character to a "soft respawn" state.
Clears the ring count, resets velocity, and restores the character to a safe, default state.
How to activate:
Press Ctrl + Q during gameplay.
Use case:
Quickly recover from a challenging situation without restarting the level.
2. Reset Position
What it does:
Resets the character's position to the initial spawn point of the level.
Also resets the velocity to ensure proper placement.
How to activate:
Press Ctrl + W during gameplay.
Use case:
Return to the starting point of the level for testing or exploration purposes.
3. Reload Current Scene
What it does:
Reloads the current level from scratch.
Useful for resetting all objects, mechanics, and conditions in the level.
How to activate:
Press Ctrl + E during gameplay.
Use case:
Reinitialize the level for debugging or restarting without returning to the menu.


Thanks https://sonicretro.org/ for a lot of info about the game.

©
Copyright belongs to the original creators; this project is for fun and non-profit purposes.
