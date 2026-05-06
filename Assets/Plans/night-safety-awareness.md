# Project Overview
- Game Title: Night Safety Awareness
- High-Level Concept: An interactive educational game where players navigate night-time scenarios, making safety-focused decisions to return home safely.
- Players: Single player
- Inspiration / Reference Games: Choice-based narrative games (e.g., Detroit: Become Human, Life is Strange), Educational simulations.
- Tone / Art Direction: Realistic, tense but educational, night-time atmosphere.
- Target Platform: PC (Stand-alone Windows)
- Render Pipeline: URP (PC_RPAsset detected)

# Game Mechanics
## Core Gameplay Loop
1. **Exploration**: Move through 3D environments (Nightclub Exit, Streets).
2. **Encounter**: Reach trigger zones that initiate safety scenarios.
3. **Decision**: Choose between Safe and Risky actions via UI prompts.
4. **Feedback**: Receive immediate educational feedback and score updates.
5. **Outcome**: Reach home and see final safety performance.

## Controls and Input Methods
- **WASD / Arrow Keys**: Character movement.
- **Mouse**: Camera control (Third-person) and UI interaction.
- **New Input System**: Handling movement and interactions.

# UI
- **Main Menu**: Already implemented (Start, Settings, Exit).
- **In-Game HUD**: Score display in the corner.
- **Decision Panel**: Central UI overlay with choices (Action A / B or Yes / No).
- **Feedback Panel**: Informational popup explaining the consequence of a choice.

# Key Asset & Context
- **Scripts**:
    - `GameManager`: Persists data (score, state) across scenes.
    - `PlayerController`: Third-person movement logic.
    - `ScenarioController`: Manages trigger zones and decision UI activation.
    - `UIManager`: Handles in-game HUD and decision/feedback panels.
    - `AudioManager`: Manages ambient music transitions and SFX.
- **Scenes**: `MainMenu`, `NightclubExit`, `StreetEncounter`, `WalkingHome`, `FinalOutcome`.

# Implementation Steps
1. **Infrastructure**:
    - Create `GameManager` (Singleton) to track score and game state.
    - Setup the 4 gameplay scenes and update Build Settings.
2. **Player & Movement**:
    - Implement a basic Third-Person `PlayerController`.
    - Setup a Camera follow system (Cinemachine if available, or simple script).
3. **Scenario System**:
    - Create `ScenarioTrigger` script to detect player entry.
    - Implement the `ScenarioController` to show UI and pause/resume gameplay.
4. **UI Toolkit In-Game**:
    - Design `GameHUD.uxml` and `DecisionPanel.uxml`.
    - Update `UIManager` to bind these to game events.
5. **Scene Content**:
    - **Scene 1**: Simple street layout with nightclub exit.
    - **Scene 2**: Stranger NPC and drug offer scenario.
    - **Scene 3**: "Following" mechanic and lit area choice.
    - **Scene 4**: Result screen with total score.
6. **Audio & Polishing**:
    - Integrate the generated BGM and add SFX (heartbeat, footsteps).

# Verification & Testing
- Verify score increases on safe choices and stays same on risky ones.
- Ensure scene transitions trigger correctly after decisions.
- Test that "Mute" setting in Main Menu persists into gameplay.
