🌍 Elemental World

Elemental World is a 3D action RPG built in Unity, featuring modular boss systems, dynamic combat mechanics, cinematic encounter design, and scalable gameplay architecture.
This project focuses on building core gameplay systems first (Version 1) using placeholder visuals, with plans to expand into a full open-world experience.

⸻

🔁 Core Gameplay Loop
	1.	Fight Boss (Solaris)
	2.	Trigger Cutscene
	3.	Transition to Refuge Town (Hub)
	4.	Travel between Town ↔ Open World
	5.	Progress toward future elemental bosses

⸻

⚙️ Current Features (Version 1)

☀️ Boss System — Solaris
	•	Multi-phase boss AI:
	•	Phase 1 → Basic attacks
	•	Phase 2 → Wall of Light shield + new abilities
	•	Phase 3 → Aggressive combined attacks
	•	Dynamic attack system:
	•	Flare Attack (targeted telegraph)
	•	Dagger Projectile
	•	Light Beam (tracking damage over time)
	•	Light Pulse (AoE burst)
	•	Solar Rain (area strikes)
	•	Blinding Flash (player debuff)
	•	Teleportation system:
	•	Random arena repositioning
	•	Center teleport for special attacks

⸻

🛡️ Wall of Light System
	•	Shield-based defensive phase mechanic
	•	Separate shield HP system
	•	Damage absorption + break condition
	•	Event-driven phase transition on shield break
	•	Visual activation/deactivation

⸻

🌪️ Wind Boss

The Wind Boss is the second major boss encounter, expanding on the core boss system with a chaotic and ability-driven combat style.

Features
	•	Dynamic attack loop system (no fixed phases)
	•	Randomized ability selection from a move pool
	•	Cinematic intro sequence (boss descends from sky)
	•	Death cutscene with dialogue and UI transitions
	•	Fully modular ability system using WindAbilityPack

Implemented Abilities
	•	Cyclone
	•	Hurricane Strike
	•	Hurricane Slam
	•	Air Blade
	•	Wind Current
	•	Sky Lift
	•	Eye of the Storm
	•	Phantom Gust

Technical Highlights
	•	Coroutine-based attack execution for smooth chaining
	•	Ability modularization via separate Ability Pack
	•	Reusable combat interfaces (IDamage)
	•	Integrated UI + camera control through manager systems
	•	Fixed coroutine restart bug in death sequence

Notes
	•	Uses placeholder visuals for all abilities (primitive shapes)
	•	Final visual polish will be implemented during the Blender phase

⸻

🌊 Water Boss

The Water Boss expands the encounter system with advanced repositioning AI, clone mechanics, environmental pressure abilities, and cinematic encounter flow.

Features
	•	Full cinematic intro and ending cutscene system
	•	Dynamic repositioning combat AI
	•	Randomized modular move pool
	•	Mirage Split clone system with independent clone AI
	•	Secondary Mirage Split HP system
	•	Exit gate spawning after boss defeat
	•	Integrated manager-based UI and scene flow systems

Implemented Abilities
	•	Hydro Snipe
	•	Water Prison
	•	Acid Wave
	•	Tidal Pull
	•	Depth Charge
	•	Water Bubble Shield
	•	Mirage Split
	•	Phase Swim

Mirage Split System
	•	Clone-specific AI movement and attacks
	•	Independent clone HP system
	•	Separate Mirage Split boss HP bar
	•	Clone repositioning around the player using NavMesh
	•	Coroutine-driven attack rhythm system

Technical Highlights
	•	Advanced coroutine-driven boss sequencing
	•	Interface-driven UI updates (IHealthUI)
	•	Interface-based cutscene structure (ICutscene)
	•	Refactored architecture:
	•	GameManager
	•	UIManager
	•	SceneFlowManager
	•	Reusable modular ability pack architecture
	•	NavMesh-based roaming and repositioning systems

Notes
	•	Uses placeholder visuals for all Water Boss abilities
	•	Additional Blender animations and polish planned for Version 1.5+
	•	Some Water Boss abilities still require damage tuning and polish passes

⸻

⚡Lightning Boss

• Fully implemented Lightning Boss encounter featuring dynamic attack selection, conditional mechanics, and multi-layered combat interactions.

• Implemented Moves:
• Arc Spear
• Thunder Strike
• Voltage Mines
• Magnetic Pull
• Storm Surge
• Ion Crash
• EMP Pulse
• Static Detonation

• Implemented Mechanics:
• Static Mark system
• Mine tracking and detonation system
• Conditional attack logic
• Telegraph and warning indicators
• Cooldown management
• Randomized move selection AI

• Encounter Features:
• Intro cutscene
• End cutscene
• Boss HP UI integration
• Death sequence
• Exit gate spawning
• Scene Flow Manager integration
• Modular Ability Pack architecture

• Lightning Boss Design:
• Uses thunder-based attacks, area denial, player marking mechanics, and mine interactions to create a chaotic and aggressive combat experience.
• Conditional abilities allow the boss to react to player status and battlefield conditions.

🔧 Planned Version 1.5 Polish
• Thunderstorm cloud visuals
• Lightning environment effects
• Improved attack VFX
• Ion Crash flight/slam animation
• EMP Pulse charge-up visuals
• Enhanced telegraphs and cinematic presentation
⸻

⚔️ Combat System
	•	Raycast-based player attack system
	•	Interface-driven architecture:
	•	IDamage for damage handling
	•	IHeal for healing mechanics
	•	IHealthUI for reusable HP UI updates
	•	ICutscene for reusable cutscene systems
	•	Modular design for reusable enemy/boss interactions

⸻

🎬 Cutscene System
	•	Triggered before and after boss encounters
	•	Player control disabling
	•	Camera switching (Player → Cutscene camera)
	•	Dialogue sequence system
	•	Scene transition integration
	•	Coroutine-driven cinematic sequences

⸻

🧩 Manager Architecture

The project now uses separated manager systems for cleaner architecture and scalability.

🕹️ GameManager
	•	Game state handling
	•	Pause system
	•	Player menu systems
	•	Global gameplay state management

🖥️ UIManager
	•	Player HP and Sprint UI
	•	Boss HP systems
	•	Dialogue UI
	•	UI visibility management

🌍 SceneFlowManager
	•	Scene transitions
	•	Gate systems
	•	Scene-loaded setup
	•	Boss flow and transition handling

⸻

🌍 World Structure
	•	Refuge Town (hub area)
	•	Open World scene (in progress)
	•	Elemental boss arenas
	•	Gateway system between scenes

⸻

🔄 Scene Management
	•	Manager-driven scene loading flow
	•	Trigger-based scene transitions
	•	Clean separation between:
	•	Game systems
	•	UI systems
	•	Scene systems
	•	Boss encounter systems

⸻

🧠 Technical Highlights
	•	State-driven AI systems (phase-based + ability-based)
	•	Coroutine-based attack sequencing and cutscenes
	•	Modular boss architecture using Ability Packs
	•	Event-driven mechanics
	•	Interface-driven architecture
	•	Manager-based system organization
	•	NavMesh-driven repositioning AI
	•	Reusable combat and UI systems

⸻

⚠️ Known Issues / Improvements (Planned)
	•	Water Boss death timing and exit gate spawn delay
	•	Some Water Boss abilities inconsistently damage the player
	•	Solaris blinding flash overlay can remain active during dialogue
	•	Wind Boss return gate flow needs verification
	•	Additional visual polish and VFX tuning
	•	Placeholder animations still in use
	•	Spawn point adjustments for better ground alignment

⸻

🚀 Future Development (Version 2)
	•	Expanded open world
	•	Additional elemental bosses:
	•	Fire
	•	Ice
	•	Lightning
	•	Earth
	•	Dark Boss
	•	Advanced boss behaviors and patterns
	•	Improved visuals and animations (Blender integration)
	•	Player progression system (stats / abilities)
	•	Environmental storytelling and world-building
	•	Roaming calamity systems
	•	Dynamic world events

⸻

🛠️ Built With
	•	Unity (C#)
	•	Visual Studio
	•	Blender
	•	GitHub (Version Control)

⸻

🧾 Developer Notes

This project is being developed as a structured learning and portfolio piece, focusing on:
	•	Strong gameplay systems
	•	Scalable architecture
	•	Clean and reusable code structure
	•	Modular boss encounter design
	•	Iterative development (Version 1 → Version 2)

⸻

📊 Status
	•	Version 1 Core Loop Complete
	•	Boss System Expanded (Solaris + Wind Boss + Water Boss)
	•	Manager Architecture Refactor Complete
	•	Cutscene System Expanded
	•	Expanding World + Systems
	•	Beginning Lightning Boss implementation
