🌍 Elemental World

Elemental World is a 3D action RPG built in Unity, featuring modular boss systems, dynamic combat mechanics, and cinematic encounter design.
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

The Wind Boss is the second major boss encounter, expanding on the core boss system with a more chaotic and ability-driven combat style.

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
	•	Integrated UI + camera control through GameManager
	•	Fixed coroutine restart bug in death sequence

Notes
	•	Uses placeholder visuals for all abilities (primitive shapes)
	•	Final visual polish will be implemented during the Blender phase

⸻

⚔️ Combat System
	•	Raycast-based player attack system
	•	Interface-driven architecture:
	•	IDamage for damage handling
	•	IHeal for healing mechanics
	•	Modular design for reusable enemy/boss interactions

⸻

🎬 Cutscene System
	•	Triggered after boss defeat
	•	Player control disabling
	•	Camera switching (Player → Cutscene camera)
	•	Dialogue sequence system
	•	Scene transition integration

⸻

🌍 World Structure
	•	Refuge Town (hub area)
	•	Open World scene (in progress)
	•	Gateway system between scenes

⸻

🔄 Scene Management
	•	Centralized scene loading via GameManager
	•	Trigger-based scene transitions
	•	Clean separation between:
	•	Core systems (GameManager)
	•	Interaction triggers (gateways)

⸻

🧠 Technical Highlights
	•	State-driven AI systems (phase-based + ability-based)
	•	Coroutine-based attack sequencing and cutscenes
	•	Modular boss architecture using Ability Packs
	•	Event-driven mechanics (shield break → phase change)
	•	Clean separation of systems:
	•	AI
	•	Combat
	•	UI
	•	Scene Management

⸻

⚠️ Known Issues / Improvements (Planned)
	•	Wall of Light healing behavior tuning
	•	Shield damage feedback + UI improvements
	•	Some attack prefabs persist in scene (cleanup needed)
	•	Spawn point adjustments for better ground alignment

⸻

🚀 Future Development (Version 2)
	•	Expanded open world
	•	Additional elemental bosses:
	•	Fire, Ice, Lightning, Earth, Wind
	•	Advanced boss behaviors and patterns
	•	Improved visuals and animations (Blender integration)
	•	Player progression system (stats / abilities)
	•	Environmental storytelling and world-building

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
	•	Clean and scalable code architecture
	•	Iterative development (Version 1 → Version 2)

⸻

📊 Status
	•	Version 1 Core Loop Complete
	•	Boss System Expanded (Solaris + Wind Boss)
	•	Expanding World + Systems
	•	Moving into full game structure
