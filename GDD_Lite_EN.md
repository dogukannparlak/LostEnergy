# LOST ENERGY

## GDD Lite

### Project Information

**Project Name:** Lost Energy
**Document Owner:** Dogukan Parlak
**Student ID:** 221805040
**Engine:** Unity 6 (URP)
**Target Platform:** Windows (PC)

## Project Summary

Lost Energy is a single-player, short-form third-person collection and survival game. The player moves across fragmented islands floating in the void, manages oxygen, collects crystals, avoids dangerous areas, and tries to reach the exit gate.

The project aims to deliver a gameplay loop that is easy to understand in a short time but still maintains constant pressure on the player. Its main focus is exploration, resource management, risky route selection, and a short yet cohesive progression.

## High Concept

The player tries to survive in a fragmented and unfinished world. While oxygen constantly decreases, crystals act both as a progression requirement and as a short-term support resource. The main goal is simple: collect enough crystals and reach the exit.

## Target Player Experience

The game is designed to make the player feel:

- constant time and resource pressure
- a sense of progress and achievement as crystals are collected
- a short but memorable adventure

## Core Design Pillars

**1. Constant Pressure:** Oxygen decreases even in safe areas, encouraging the player to keep moving.

**2. Clear Goals:** The player is clearly informed about how many crystals are needed and when the exit will open.

**3. Simple but Effective Loop:** Exploration, crystal collection, oxygen management, and reaching the exit form the core gameplay loop.

## Core Gameplay Loop

**Explore, collect crystals, preserve your oxygen, and reach the exit.**

In each scene, the player explores the environment, gathers crystals, avoids hazards, and moves toward the next objective after making enough progress. This creates a clear and understandable flow from beginning to end.

## Main Mechanics

- third-person character control
- free camera movement
- oxygen decreasing over time
- collecting crystals through interaction
- additional oxygen loss in dangerous zones
- exit opening after the crystal target is completed
- win, lose, and restart flow

## Controls

- **WASD / Arrow Keys:** Movement
- **Mouse:** Camera control
- **Space:** Jump
- **Left Shift:** Sprint
- **E:** Interact
- **ESC:** Pause

## Scene Structure

**1. Main Menu**

Provides access to starting the game, settings, and the controls screen.

**2. Game Scenes**

**2.a - Intro Island:** The player learns the basics of movement, camera usage, interaction, and crystal collection here. If needed, a short NPC encounter can be used for guidance.

**2.b - Broken Island:** The environment becomes more complex. Dangerous surfaces, harder route choices, and more careful oxygen management are required.

**2.c - Final Island:** In the final section, the player uses all previously learned systems together. This is where the game reaches its ending, and the player may face a narrative choice such as staying in the loop or breaking it.

**3. Result Screen**

If the player reaches the exit, a success screen is shown. If oxygen runs out or the player falls into the void, a failure screen appears and a retry option is offered. If the player chooses to remain in the loop, the game presents this as a narrative return.

## Resource and Success System

The main resource in the game is oxygen.

- the player loses oxygen over time
- oxygen loss increases in dangerous areas
- crystals can partially restore oxygen
- if oxygen reaches zero, the player loses the level

The success condition is reaching the exit gate after collecting the required number of crystals. Failure occurs when oxygen is depleted or when the player falls from the islands.

## Difficulty Progression

The difficulty increases gradually while teaching the player the core systems.

This structure keeps teaching and difficulty scaling simple within the same gameplay flow.

## Failure Feedback

When the player fails, the reason should be clearly understood.

- a short failure message appears on screen
- it is stated whether oxygen ran out or the player fell into the void
- a short sound effect supports the failure moment
- the player is quickly offered a retry option

This feedback gives the player clear information while encouraging another attempt.

## UI and Audio

The interface continuously shows the information the player needs:

- oxygen level
- crystal counter
- interaction prompt
- pause menu
- result panels

On the audio side, menu music and gameplay music are separated. Basic sound effects are used to provide feedback during crystal collection, danger, success, and failure moments.

## Technical Plan

The project will be built in Unity as a set of small and manageable systems:

- `PlayerController3P`: movement, jumping, and camera control
- `OxygenSystem`: oxygen consumption and restoration
- `GameManager`: crystal tracking, scene flow, and basic game management
- `HazardZone`: additional oxygen loss in dangerous areas
- `PauseManager`: pause menu management
- `UIManager`: HUD, warnings, and result panels

This structure keeps the system modular and ensures that the project remains manageable at a student-project scope.

## Minimum Delivery Target

The main demonstrable outputs for this project are:

- a working main menu
- playable game scenes
- third-person character control
- oxygen and crystal systems
- danger zones
- exit objective and gate opening logic
- simple win and lose screens

## Risks and Scope Control

The biggest risk in the project is expanding the scope by adding unnecessary features. For that reason, the focus should remain on the following systems:

- movement
- oxygen management
- crystal collection
- in-scene progression
- basic UI and feedback

NPC dialogue, branching endings, denser story layers, and advanced visual effects should only be considered after the core gameplay is complete.

## Why Is This Project Suitable for the Course?

Lost Energy is a good fit for this course because it directly satisfies the third-person gameplay requirement, supports a structured multi-scene game flow, and makes player progress measurable through clear systems such as oxygen, crystals, and the exit objective. The project scope is also realistic for a student production; it can first be built as a core prototype focused on movement, collection, and survival, then expanded with UI, audio, scene transitions, and stronger presentation quality for a more polished final submission. This structure makes the project both technically manageable and sufficiently substantial in terms of course outcomes.
