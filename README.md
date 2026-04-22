# LOST ENERGY

## Game Design Document

### Third-Person Adventure / Resource Management Game

| Title            | Info                        |
| ---------------- | --------------------------- |
| Developer        | Doğukan Parlak - 221805040 |
| Engine           | Unity 6 (URP)               |
| Target Platform  | Windows (PC)                |
| Document Version | 1.0                         |
| Project Version  | 1.0                         |
| Date             | April 2026                  |

---

## Table of Contents

## Main Cover Art Design

1. [Design History](#1-design-history)
2. [Game Overview](#2-game-overview)
3. [Feature Set](#3-feature-set)
4. [Game World](#4-game-world)
5. [Camera and Controls](#5-camera-and-controls)
6. [Gameplay Systems](#6-gameplay-systems)
7. [World Layout and Progression Flow](#7-world-layout-and-progression-flow)
8. [Characters](#8-characters)
9. [User Interface](#9-user-interface)
10. [Audio and Music](#10-audio-and-music)
11. [Single-Player Experience](#11-single-player-experience)
12. [Story and Narrative](#12-story-and-narrative)
13. [Technical Implementation](#13-technical-implementation)
14. [Asset and License Information](#14-asset-and-license-information)
15. [Setup and Installation](#15-setup-and-installation)

---

## Main Cover Art Design

![Figure 1. Lost Energy cover image](image/ChatGPT%20Image%2013%20Nis%202026%2020_39_57.png)

*Figure 1. Main cover artwork created for Lost Energy.*

![Figure 2. Alternative cover design](image/Gemini_Generated_Image_sncx22sncx22sncx.png)

*Figure 2. Alternative cover design supporting the game's atmosphere and narrative tone.*

## 1. Design History

### Version 1.0

This version represents the submission-ready current design document for the Lost Energy project.

- Three playable scenes completed: `SampleScene`, `SampleScene2`, `SampleScene3`.
- Third-person character control, orbit camera, and interaction system integrated.
- Oxygen-based resource management, crystal collection loop, and hazard zone system completed.
- Main menu, pause menu, settings panel, and loading screen implemented.
- NPC dialogue system, scene transition flow, and basic audio management completed.
- Asset, resource, and license information integrated into the document.

---

## 2. Game Overview

### Game Description

Lost Energy is a single-player third-person adventure game. The player manages their oxygen to survive on a fragmented island floating in the void, collects crystals scattered across the environment, avoids hazardous zones, and unlocks doors that advance progression between scenes.

![Figure 3. General gameplay view](image/SampleScene.png)

*Figure 3. General gameplay view showing the player character, HUD, and scene layout together.*

### Design Goals

- Build gameplay that is quickly understood through simple rules yet creates sustained pressure.
- Use the oxygen mechanic simultaneously as a life indicator, time pressure, and strategic resource.
- Combine environmental threats, exploration, and storytelling into a short yet cohesive experience.
- Produce a vertical slice with a manageable scope for academic submission that feels complete.

### Core Design Principles

#### 1. Constant Pressure

The player loses oxygen even in safe areas. As a result, inactivity is penalized, decision-making is accelerated, and every crystal becomes meaningful.

#### 2. Clear Objectives

The crystal objective is explicit in the first two scenes. The player can always see how many to collect, when the door will open, and what the progression condition is.

#### 3. Environmental Storytelling

Red hazardous surfaces, scene transitions, island boundaries, and guard dialogues work together to reinforce the world's fractured structure.

### Key Frequently Asked Questions

#### What is the game?

A short third-person adventure experience built on resource management, exploration, and scene-based progression.

#### Where does the game take place?

The game takes place in an island system floating in the void, part of an incomplete creation.

#### What does the player control?

A single character is controlled. The character moves, jumps, interacts, collects crystals, and progresses between scenes.

#### What is the main focus of the game?

The main focus is choosing the right route under oxygen pressure, collecting crystals, and reaching the exit.

#### What makes this game different?

The oxygen system is not merely a health indicator; it is the core mechanic that simultaneously determines time pressure, route optimization, and reward structure.

---

## 3. Feature Set

### Core Gameplay Features

- Third-person character control
- Freely rotating orbit camera system
- Real-time oxygen consumption and replenishment system
- Crystal collection and objective completion structure
- `IInteractable`-based interaction architecture
- Additional oxygen drain from hazard zones
- Death/respawn flow triggered by falling and oxygen depletion
- NPC dialogue system and scene-linked narrative delivery

### Supporting Systems

- Main menu and pause menu
- Settings panel and volume control
- Scene transitions via loading screen
- Per-scene music switching
- VFX and SFX support for crystal collection
- Branching ending selection in the final scene

### User Experience Features

- Always-visible oxygen and crystal indicators
- On-screen prompt text for nearby interactable targets
- Explicit control scheme
- Compact level structure suited to short play sessions

---

## 4. Game World

### General Structure

The Lost Energy world is designed as an incomplete realm of existence. The island is physically bounded; stepping beyond the boundaries results in death. Each new scene represents an area where the deterioration has become more visible.

### World Features

#### Fragmented Island Structure

The play area is built on a layout where safe surfaces and risky gaps coexist. This makes navigation, staying on platforms, and choosing safe routes important.

#### Deterioration Zones

Red hazardous surfaces do not merely provide visual variety; they are mechanical threat zones that force the player to alter their route.

### Physical World Summary

The following values represent the core physical parameters of the player character. Movement speeds and jump height are configured via the Unity Inspector and may be tuned per scene; the values shown reflect the primary configuration used across gameplay scenes.

| Property        | Value             |
| --------------- | ----------------- |
| Perspective     | Third-person (3D) |
| Walk Speed      | 10 units/s        |
| Sprint Speed    | 25 units/s        |
| Jump Height     | 1.5 units         |
| Camera Distance | 5 units           |

*Table 1. Core physical movement parameters governing the player character.*

![Figure 4. Scene 2 general view](image/SampleScene2.png)

*Figure 4. General view of the second scene where deterioration has increased.*

![Figure 5. Scene 3 general view](image/SampleScene3.png)

*Figure 5. General environment view of the final section.*

---

## 5. Camera and Controls

### Camera System

The camera system is managed within `PlayerController3P`. The camera orbits around the player; yaw and pitch inputs are handled independently. Because movement direction is calculated relative to the camera's facing direction, the control feel is designed similarly to third-person action games.

| Parameter         | Value   |
| ----------------- | ------- |
| Mouse Sensitivity | 0.2     |
| Pitch Minimum     | -30°   |
| Pitch Maximum     | +60°   |
| Camera Distance   | 5 units |

*Table 2. Camera configuration values defined in `PlayerController3P`. Pitch limits prevent the camera from flipping at extreme angles: the −30° floor avoids disorientation on narrow platforms, while the +60° ceiling provides a generous upward view without exposing the empty void above the island. Mouse Sensitivity of 0.2 was tuned to feel responsive without over-rotating on typical mouse hardware.*

### Control Scheme

| Action                 | Input             |
| ---------------------- | ----------------- |
| Move                   | WASD / Arrow Keys |
| Jump                   | Space             |
| Sprint                 | Left Shift        |
| Interact               | E                 |
| Camera                 | Mouse             |
| Pause / Release Cursor | ESC               |
| Skip                   | Tab               |
| Interact Npc           | Space             |

*Table 3. Full player input mapping. All inputs are handled by Unity's new Input System package (`InputSystem_Actions`). The ESC key both pauses gameplay and releases/recaptures the cursor, removing the need for a separate cursor-toggle binding.*

---

## 6. Gameplay Systems

### Oxygen System

Oxygen is the game's primary resource structure. The player consumes oxygen as long as they are in a scene. When the value reaches zero, the character dies and returns to the last safe starting point.

| Area    | Starting Oxygen | Normal Drain | Hazard Extra Drain | Crystal Bonus |
| ------- | --------------- | ------------ | ------------------ | ------------- |
| Scene 1 | 100             | 3 units/s    | +5 units/s         | +5            |
| Scene 2 | 100             | 3 units/s    | +6 units/s         | +6            |
| Scene 3 | N/A             | N/A          | N/A                | N/A           |

*Table 4. Per-scene oxygen parameters. Scene 3 has no crystal target and both exit doors are unlocked from the start; however, oxygen drain continues at the same base rate. The only pressure in Stage 3 is selection.*

### Crystal System

- Crystals are spawned in defined areas within the scene.
- The player collects a crystal by interacting via the `E` key.
- Visual and audio feedback is generated during the collection action.
- In the first two scenes, the exit door opens once the crystal target is met.
- In the final scene there is no crystal target; the ending choice is presented directly to the player.

Each collected crystal replenishes **+5 oxygen** (Scene 1) or **+6 oxygen** (Scene 2), capped at the maximum of 100. At the 25-crystal target in Scene 1 the potential total replenishment is 125 oxygen — roughly two full oxygen cycles — which offsets approximately 42 seconds of normal drain. This balance is intentional: the reward is meaningful enough to incentivize collection but never fully eliminates pressure.

### Hazard Zone System

- `HazardZone` areas increase oxygen consumption.
- These zones are visually marked with red surfaces.
- Music and tension are intensified upon entering a hazard area.

![Figure 6. Hazard zone example](image/hazardzone.png)

*Figure 6. Example screenshot showing the effect of a hazard zone on the play area.*

![Figure 7. Hazard zone close-up view](image/hazardzone2.png)

*Figure 7. Second view showing the pressure a hazard area exerts on the player.*

### Interaction System

The interaction structure is built on the `IInteractable` interface. Crystals, doors, NPCs, and potions all use the same approach. `PlayerInteraction` determines the best target for the player and displays the appropriate prompt text on screen.

![Figure 8. Crystal interaction screen](image/crystalınteractable.png)

*Figure 8. Interaction prompt and HUD components visible at the moment of crystal collection.*

### Reward and Progression Loop

```text
Collect crystal -> Replenish oxygen -> Survive longer
               -> Collect more crystals -> Complete objective
               -> Open exit door -> Transition to new scene
```

---

## 7. World Layout and Progression Flow

### General Flow

The game consists of three main scenes. In the first two scenes the player collects a set number of crystals; in the third scene the experience is concluded or looped through a final choice.

### Scene 1 – Island Entrance

| Field          | Description                                         |
| -------------- | --------------------------------------------------- |
| Atmosphere     | Relatively calm starting area                       |
| Crystal Target | 25                                                  |
| Hazards        | Falling off island edge, low-intensity hazard areas |
| NPC            | Guard                                               |
| Exit           | Scene 2 via `ExitDoorSceneLoader`                 |

*Table 5. Scene 1 layout and objective summary.*

### Scene 2 – Deteriorated Zone

| Field          | Description                                                        |
| -------------- | ------------------------------------------------------------------ |
| Atmosphere     | Increased deterioration, more aggressive risk areas                |
| Crystal Target | 30                                                                 |
| Hazards        | Falling off island edge, dense hazard zones, harder route pressure |
| NPC            | Guard                                                              |
| Exit           | Scene 3 via `ExitDoorSceneLoader`                                |

*Table 6. Scene 2 layout and objective summary.*

### Scene 3 – End of Creation

| Field          | Description                                        |
| -------------- | -------------------------------------------------- |
| Atmosphere     | The final threshold of an unfinished existence     |
| Crystal Target | 0                                                  |
| Hazards        | Final decision — return or exit choice            |
| NPC            | Guard                                              |
| Exit           | Loop end or win screen via `FinalDoorController` |

*Table 7. Scene 3 layout and objective summary. This scene has no crystal requirement; both doors are open from the start.*

### Game Flow Diagram

```text
Main Menu
  -> LoadingScreen
  -> Scene 1
  -> Scene 2
  -> Scene 3
     -> Loop Door: Return to Scene 1
     -> Exit Door: Congratulations screen
```

![Figure 9. Loading screen](image/loadingpanel.png)

*Figure 9. Loading screen used for transitions between scenes.*

![Figure 10. Loop door interaction](image/loopdoorınteractable.png)

*Figure 10. Door interaction in the final scene offering the option to loop back.*

---

## 8. Characters

### Player Character

| Property             | Description                                                                                                                                                                                                                            |
| -------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Model                | Kenney Animated Characters Protagonists                                                                                                                                                                                                |
| Control Setup        | `PlayerController3P` + `CharacterController`                                                                                                                                                                                       |
| Primary Goal         | Collect crystals and reach the exit under oxygen management                                                                                                                                                                            |
| Death Conditions     | Oxygen reaching zero or falling such that `Y <-1`                                                                                                                                                                                    |
| Narrative Background | A figure trapped within an unfinished creation whose reason for existing is unclear. Initially reactive and survival-driven — a stranger to the system. By Scene 3, capable of a conscious choice about their place within the cycle. |

*Table 8. Player character properties and narrative role.*

### NPC – Guard

The Guard is the game's sole narrative-focused NPC. In each scene they greet the player, provide information about the area, and gradually reveal the world's backstory. Because the character is integrated with the island they cannot leave this world, which reinforces their role in the narrative.

### Dialogue System Behavior

- Managed via the `NPCDialogue` and `DialogueData` structures.
- Oxygen consumption is paused while dialogue is active.
- Dialogue simultaneously delivers system tutorials and story content to the player.
- During dialogue, [Space] advances to the next line (or closes the panel if on the last line), and [Tab] skips the entire conversation instantly. Prompt text for these keys is shown at the bottom of the dialogue panel.

![Figure 11. Dialogue with the Guard](image/dialoguepanel.png)

*Figure 11. Dialogue panel opening during interaction with the Guard character.*

---

## 9. User Interface

### HUD Components

| Component          | Function                                                         |
| ------------------ | ---------------------------------------------------------------- |
| Oxygen Slider      | Visually displays the current oxygen level                       |
| Oxygen Value       | Shows the numerical value of oxygen                              |
| Crystal Counter    | Displays collected crystal count in `current / target` format  |
| Interaction Prompt | Provides contextual information for a nearby interactable object |

*Table 9. HUD components permanently visible during gameplay. All elements update in real time and are managed by `UIManager`.*

### Panel Structure

| Panel          | Trigger Condition                             |
| -------------- | --------------------------------------------- |
| Game Over      | Oxygen depletion or fall death                |
| Win            | Selecting the victory exit in the final scene |
| Pause          | ESC key                                       |
| Settings       | Accessed from within the pause menu           |
| Controls       | Accessed from the main menu or pause menu     |
| Loading Screen | Opens automatically during scene transitions  |

*Table 10. In-game panel inventory and their activation conditions. All panel states are managed through `UIManager` and `PauseManager`.*

### Menu Structure

The main menu includes options to start the game, access settings, view controls, and quit. At game start `Time.timeScale = 1f` and the cursor is free.

![Figure 12. Main menu screen](image/mainmenupanel.png)

*Figure 12. The game's main menu interface.*

![Figure 13. Controls screen](image/controlspanel.png)

*Figure 13. User interface screen showing the game's control scheme.*

![Figure 14. Settings screen](image/settingspanel.png)

*Figure 14. Settings panel containing volume level and mute options.*

---

## 10. Audio and Music

### Audio Architecture

The project uses three primary channels via `GameAudioMixer`.

| Channel   | Purpose                                             |
| --------- | --------------------------------------------------- |
| MasterVol | Overall volume for all audio                        |
| MusicVol  | Background music tracks                             |
| SFXVol    | In-game effects, dialogue sounds, and hazard alerts |

*Table 11. Audio mixer channel structure. All three channels are exposed as parameters in `GameAudioMixer` and controlled at runtime by `SettingsManager` using logarithmic dB conversion. Volume preferences are persisted via `PlayerPrefs`.*

### Music Usage

- Different music streams are used for the main menu and gameplay scenes.
- Hazard areas are supported with additional audio layers to heighten tension.
- `MusicManager` maintains music continuity across scene transitions.

### Source Summary

| Source                          | Content                                                 |
| ------------------------------- | ------------------------------------------------------- |
| Flowerhead - SomeWhatGood: Lofi | `Observatory and chill 2`                             |
| Not Jam Music Pack              | `ChillMenu`, `CriticalTheme`, `SwitchWithMeTheme` |
| Freesound                       | Crystal collection and NPC sound effects                |

*Table 12. Music track sources used across the project's scenes and menus.*

---

## 11. Single-Player Experience

### Game Loop

The single-player experience is built on a scene-based structure that is brief but generates intense decision pressure. In each scene the player reads the environment, evaluates risky routes, collects crystals, and reaches the exit door.

### Estimated Playtime

| Play Style                       | Duration       |
| -------------------------------- | -------------- |
| Fast Completion                  | 5–6 minutes   |
| Exploration and Dialogue Focused | 10–12 minutes |

*Table 13. Estimated completion time by playstyle.*

### Win and Lose Conditions

| State | Condition                                                        |
| ----- | ---------------------------------------------------------------- |
| Win   | Selecting the exit ending via `FinalDoorController` in Scene 3 |
| Lose  | Oxygen reaching zero or falling out of the scene                 |

*Table 14. Win and lose conditions. There is no partial failure state; any death immediately triggers the respawn flow managed by `RespawnManager`.*

![Figure 15. Exit door interaction](image/windoorınteractable.png)

*Figure 15. Exit door interaction offering the option to complete the game.*

![Figure 16. Congratulations screen](image/windoorpanel.png)

*Figure 16. Result screen displayed after completing the game.*

---

## 12. Story and Narrative

### Story Summary

Lost Energy's narrative is delivered almost entirely through three conversations with the Guard — the sole human presence in the game world. Across three encounters the Guard progressively reveals the true nature of the island: it is not a real world but an unfinished creation, abandoned mid-construction by a god who was still learning. The oxygen instability, the red deterioration zones, the void at the island's edges — all are symptoms of an incomplete existence.

The player character arrives without memory or explanation, navigates the fragmenting island, and ultimately reaches a binary choice: step through the exit door and attempt to escape the cycle, or return through the loop door and forget everything once more. The Guard cannot make this journey — he is part of the world, condemned to remember every iteration of the loop while the player perpetually forgets.

Crystals carry a thematic weight beyond their mechanical function: they are described by the Guard as the only "solidly built" fragments within the broken creation — pieces of genuine reality preserved within the decay. Collecting them is not merely a survival act; it is the act of anchoring oneself to what little is real.

### Thematic Structure

| Element    | Meaning                                                                 |
| ---------- | ----------------------------------------------------------------------- |
| Island     | An unfinished creation abandoned by its creator                         |
| Oxygen     | The world's fundamental instability made tangible                       |
| Crystals   | The only solidly built fragments — preserved pieces of genuine reality |
| HazardZone | Physical manifestation of spreading deterioration                       |
| Guard      | A witness bound to the system, unable to leave it                       |
| Loop Door  | The choice to return, forget, and begin again                           |
| Exit Door  | The choice to confront an uncertain outside                             |

*Table 15. Narrative symbolism mapping each game element to its thematic meaning within the world of Lost Energy.*

### Scene-by-Scene Narrative Development

| Scene   | Narrative Function                                                                             |
| ------- | ---------------------------------------------------------------------------------------------- |
| Scene 1 | World rules introduced; player as unknowing arrival; god as distant but present force          |
| Scene 2 | Decay revealed as spreading; god reframed as inexperienced; existential unease escalated       |
| Scene 3 | Full truth delivered: incomplete creation, loop existence, Guard's limitation, player's choice |

*Table 16. Per-scene narrative delivery functions. Each scene's dialogue builds directly on the previous, forming a three-act revelation structure.*

### Character Motivations

- **Player character:** Arrives without explanation and operates on survival instinct for the first two scenes. In Scene 3 they are offered the full truth and must decide whether to act on it or surrender to the loop. They are silent — they cannot speak — which the Guard explicitly acknowledges mid-dialogue in Scene 1.
- **The Guard:** Locked into his role as a part of the world he describes. He guides out of function, not hope. He remembers every cycle the player has forgotten, and carries the weight of that knowledge alone.

### Full Guard Dialogue

The Guard's dialogue is triggered once per scene via `NPCDialogue` when the player interacts with him. Oxygen consumption is paused for the entire duration. The scripts below document all lines from `DialogueData.json`, `DialogueData2.json`, and `DialogueData3.json`, organized into narrative beats. Beat labels are documentation groupings — they are not tagged in the asset files.

---

#### Scene 1 – Island Entrance (`DialogueData.json`)

**Narrative function:** Introduce the player to the world's rules, the oxygen system, crystals as exit tickets, and the island's boundary. Close with the first hint of the world's deeper philosophical nature.

**Beat 1 — The Guard stops the player**

| # | Turkish                                         | English                                                |
| - | ----------------------------------------------- | ------------------------------------------------------ |
| 1 | Dur!                                            | Stop!                                                  |
| 2 | Gitmeden önce bilmen gereken birkaç şey var. | There are a few things you need to know before you go. |

**Beat 2 — Existential ambivalence about the player's presence**

| # | Turkish                          | English                           |
| - | -------------------------------- | --------------------------------- |
| 3 | Buraya nasıl geldin bilmiyorum. | I don't know how you got here.    |
| 4 | Neden geldin onu da bilmiyorum.  | I don't know why you came either. |
| 5 | Açıkçası umurumda da değil. | Frankly, I don't care.            |
| 6 | Ama gelmişsen...                | But if you came here...           |
| 7 | Tanrının bir bildiği vardır. | God must know something.          |

**Beat 3 — Mission: deliver the briefing**

| # | Turkish                                      | English                                       |
| - | -------------------------------------------- | --------------------------------------------- |
| 8 | Benim görevim, bilmen gerekenleri anlatmak. | My duty is to tell you what you need to know. |

**Beat 4 — The island and the void**

| #  | Turkish                                        | English                                                    |
| -- | ---------------------------------------------- | ---------------------------------------------------------- |
| 9  | İlk olarak...                                 | First of all...                                            |
| 10 | Hiçliğin ortasında süzülen bir adadayız. | We are on an island floating in the middle of nothingness. |
| 11 | Ormanların içine girmeni pek tavsiye etmem.  | I don't recommend going into the forests.                  |
| 12 | Ada sınırına fazla yaklaşırsan...         | If you get too close to the island's edge...               |
| 13 | Hiçliğe düşersin.                          | You'll fall into the void.                                 |

**Beat 5 — Oxygen bar tutorial**

| #  | Turkish                                 | English                                    |
| -- | --------------------------------------- | ------------------------------------------ |
| 14 | Şimdi asıl konuya gelelim.            | Now let's get to the main point.           |
| 15 | Kafanın üstünde gördüğün şey... | What you see above your head...            |
| 16 | O bir oksijen barı.                    | That's an oxygen bar.                      |
| 17 | Bitmesini isteyeceğini sanmıyorum.    | I don't think you'd want it to run out.    |
| 18 | Ama denemek istersen...                 | But if you want to try...                  |
| 19 | Sonuçlarını ben de merak ediyorum.   | I'm curious about the consequences myself. |

**Beat 6 — Navigation**

| #  | Turkish                                   | English                                     |
| -- | ----------------------------------------- | ------------------------------------------- |
| 20 | Çıkışa gidene kadar okları takip et. | Follow the arrows until you reach the exit. |

**Beat 7 — Crystal tutorial**

| #  | Turkish                           | English                               |
| -- | --------------------------------- | ------------------------------------- |
| 21 | Kristalleri toplamayı da unutma. | Don't forget to collect the crystals. |
| 22 | Onlar senin çıkış biletin.    | They are your exit ticket.            |
| 23 | Biraz da can verirler.            | They'll give you a little life too.   |

**Beat 8 — Creator's nature (first hint)**

| #  | Turkish                               | English                      |
| -- | ------------------------------------- | ---------------------------- |
| 24 | Tanrımızın garip huyları vardır. | Our god has strange habits.  |
| 25 | Ama bazen insaflıdır.               | But sometimes he's merciful. |

**Beat 9 — The player cannot speak**

| #  | Turkish                             | English                             |
| -- | ----------------------------------- | ----------------------------------- |
| 26 | Ee...                               | Well...                             |
| 27 | Sormak istediğin bir şey var mı? | Is there something you want to ask? |
| 28 | ...                                 | ...                                 |
| 29 | Ah...                               | Ah...                               |
| 30 | Siz konuşamıyorsunuz.             | You can't speak.                    |
| 31 | Unutuyorum bazen.                   | I forget sometimes.                 |

**Beat 10 — Shared nature (existential close)**

| #  | Turkish                              | English                             |
| -- | ------------------------------------ | ----------------------------------- |
| 32 | Bizim tek farkımız...              | Our only difference...              |
| 33 | Biraz daha fazla farkında olmamız. | is that we're a little more aware.  |
| 34 | Ama sonuçta...                      | But in the end...                   |
| 35 | Aynı yerin parçalarıyız.         | we are all parts of the same place. |

**Closing**

| #  | Turkish                                 | English                          |
| -- | --------------------------------------- | -------------------------------- |
| 36 | İleride daha çok şey öğreneceksin. | You will learn much more ahead.  |
| 37 | Dediklerimi unutma.                     | Don't forget what I've told you. |
| 38 | Bol şans, yabancı.                    | Good luck, stranger.             |

---

#### Scene 2 – Deteriorated Zone (`DialogueData2.json`)

**Narrative function:** Acknowledge the player's progress; explain hazard zones as corrupted areas; reframe the god as a new, inexperienced creator causing involuntary damage; escalate existential unease.

**Beat 1 — Recognition**

| # | Turkish                        | English                         |
| - | ------------------------------ | ------------------------------- |
| 1 | Hey...                         | Hey...                          |
| 2 | Yine sen.                      | It's you again.                 |
| 3 | Buraya kadar gelebildin demek. | So you managed to get this far. |
| 4 | İlk aşamayı geçtin.        | You passed the first stage.     |

**Beat 2 — Omitted warning**

| # | Turkish                                   | English                                   |
| - | ----------------------------------------- | ----------------------------------------- |
| 5 | Sana geçen sefer söylemeyi unutmuştum. | I forgot to tell you something last time. |
| 6 | Ama fark etmişsindir.                    | But you must have noticed.                |

**Beat 3 — Hazard zones explained**

| #  | Turkish                                         | English                                      |
| -- | ----------------------------------------------- | -------------------------------------------- |
| 7  | Bazı bölgelerde oksijen daha hızlı bitiyor. | Oxygen runs out faster in some areas.        |
| 8  | Neden mi?                                       | Why?                                         |
| 9  | Çünkü bu yer... düzgün çalışmıyor.     | Because this place... doesn't work properly. |
| 10 | Fark ettiysen, zemin de kırmızı.             | If you noticed, the floor is also red.       |
| 11 | Bu iyiye işaret değil.                        | That's not a good sign.                      |
| 12 | Oralar daha çok bozulmuş alanlar.             | Those are more corrupted areas.              |

**Beat 4 — God's experiments spreading the decay**

| #  | Turkish                                            | English                                 |
| -- | -------------------------------------------------- | --------------------------------------- |
| 13 | Tanrı yine bir şeyler denerken...                | While God was trying things again...    |
| 14 | Bir şeyleri bozmuş.                              | He broke something.                     |
| 15 | Fark etmişsindir...                               | You must have noticed...                |
| 16 | Şimdi de alanlar büyümüş.                     | Now the areas have grown too.           |
| 17 | Bu da bozulmanın yayıldığı anlamına geliyor. | This means the corruption is spreading. |

**Beat 5 — Warning**

| #  | Turkish      | English     |
| -- | ------------ | ----------- |
| 18 | Dikkatli ol. | Be careful. |

**Beat 6 — Remaining rules unchanged**

| #  | Turkish                   | English                    |
| -- | ------------------------- | -------------------------- |
| 19 | Geri kalan şeyler aynı. | The rest remains the same. |
| 20 | Hızlı ol.               | Be quick.                  |
| 21 | Kristalleri topla.        | Collect the crystals.      |
| 22 | Kapıya ulaş.            | Reach the door.            |

**Beat 7 — Unstable world, ominous luck wish**

| #  | Turkish                             | English                        |
| -- | ----------------------------------- | ------------------------------ |
| 23 | Bol şans.                          | Good luck.                     |
| 24 | Bu bölgede buna ihtiyacın olacak. | You'll need it in this area.   |
| 25 | Bu ada...                           | This island...                 |
| 26 | Hiç stabil sayılmaz.              | Can't be called stable at all. |

**Beat 8 — God as a newborn creator**

| #  | Turkish                               | English                              |
| -- | ------------------------------------- | ------------------------------------ |
| 27 | Neyse...                              | Anyway...                            |
| 28 | Fazla konuşmayayım.                 | I shouldn't talk too much.           |
| 29 | Sağı solu belli olmuyor.            | Nothing is predictable anymore.      |
| 30 | Galiba bu evrende yeni.               | He seems to be new to this universe. |
| 31 | Her şeyi öğrenmeye çalışıyor.  | Trying to learn everything.          |
| 32 | Tıpkı yeni doğmuş bir bebek gibi. | Just like a newborn baby.            |
| 33 | Bu yüzden bu anomaliler.             | That's why these anomalies.          |

**Beat 9 — Uncertain farewell**

| #  | Turkish                  | English                          |
| -- | ------------------------ | -------------------------------- |
| 34 | Neyse...                 | Anyway...                        |
| 35 | Bu bölgeyi geçersen... | If you pass through this area... |
| 36 | Yine karşılaşırız.  | We'll meet again.                |
| 37 | Tabii...                 | Of course...                     |
| 38 | Orası da varsa.         | If that place still exists.      |

**Closing**

| #  | Turkish              | English                    |
| -- | -------------------- | -------------------------- |
| 39 | Görüşmek üzere.  | Until we meet again.       |
| 40 | Güç seninle olsun. | May the force be with you. |
| 41 | İhtiyacın olacak.  | You'll need it.            |

---

#### Scene 3 – End of Creation (`DialogueData3.json`)

**Narrative function:** Deliver the complete truth — the world is an unfinished creation; crystals are fragments of reality; the loop is real; the Guard cannot leave; the player has a choice.

**Beat 1 — Arrival acknowledged**

| # | Turkish                       | English                |
| - | ----------------------------- | ---------------------- |
| 1 | Ah...                         | Ah...                  |
| 2 | Sonunda geldin.               | You finally made it.   |
| 3 | Buraya kadar gelen pek olmaz. | Not many get this far. |

**Beat 2 — Truth begins**

| # | Turkish                      | English                       |
| - | ---------------------------- | ----------------------------- |
| 4 | Şimdi...                    | Now...                        |
| 5 | Gerçeği öğrenme zamanı. | It's time to learn the truth. |

**Beat 3 — The world is unfinished**

| #  | Turkish                                           | English                                                      |
| -- | ------------------------------------------------- | ------------------------------------------------------------ |
| 6  | Burası bir dünya değil.                        | This is not a world.                                         |
| 7  | Yarım kalmış bir yaratım.                     | An unfinished creation.                                      |
| 8  | Tanrının denemesi.                              | God's experiment.                                            |
| 9  | Ama tamamlanamamış.                             | But it was never completed.                                  |
| 10 | O yüzden her şey bozuk, çirkin ve orantısız. | That's why everything is broken, ugly, and disproportionate. |
| 11 | O yüzden oksijen azalıyor.                      | That's why oxygen diminishes.                                |
| 12 | O yüzden hiçlik var.                            | That's why the void exists.                                  |
| 13 | Bu yer silinmedi.                                 | This place was not deleted.                                  |
| 14 | Ama düzeltilmedi de.                             | But it wasn't fixed either.                                  |
| 15 | Sadece bırakıldı.                              | It was simply left behind.                                   |

**Beat 4 — Crystals redefined as fragments of reality**

| #  | Turkish                                                   | English                                              |
| -- | --------------------------------------------------------- | ---------------------------------------------------- |
| 16 | Kristaller...                                             | Crystals...                                          |
| 17 | Onlar bu yerin sağlam inşa edilmiş yegane parçaları. | They are the only solidly built parts of this place. |
| 18 | Gerçekliğin kırıntıları.                            | Fragments of reality.                                |
| 19 | Topladıkça...                                           | As you collect them...                               |
| 20 | Buradan koparsın.                                        | You detach from this place.                          |
| 21 | Ve belki çıkarsın.                                     | And perhaps you escape.                              |

**Beat 5 — Doubt about the exit**

| #  | Turkish                  | English                     |
| -- | ------------------------ | --------------------------- |
| 22 | Ama çıkış...         | But the exit...             |
| 23 | Gerçek mi bilmiyorum.   | I don't know if it's real.  |
| 24 | Belki başka bir deneme. | Perhaps another experiment. |

**Beat 6 — Staying means decay**

| #  | Turkish              | English                |
| -- | -------------------- | ---------------------- |
| 25 | Ama burada kalmak... | But staying here...    |
| 26 | Onu biliyorum.       | That I know.           |
| 27 | Burası çürüyor.  | This place is rotting. |

**Beat 7 — The loop revealed**

| #  | Turkish                        | English                    |
| -- | ------------------------------ | -------------------------- |
| 28 | Ve evet...                     | And yes...                 |
| 29 | Bu bir döngü.                | This is a loop.            |
| 30 | Sen daha önce de buradaydın. | You have been here before. |
| 31 | Ama hatırlamıyorsun.         | But you don't remember.    |
| 32 | Ben hatırlıyorum.            | I remember.                |

**Beat 8 — The Guard's nature revealed**

| #  | Turkish                     | English                  |
| -- | --------------------------- | ------------------------ |
| 33 | Ben çıkamıyorum.         | I cannot leave.          |
| 34 | Ben bu yerin parçasıyım. | I am part of this place. |
| 35 | Ama sen...                  | But you...               |
| 36 | Henüz değilsin.           | Not yet.                 |

**Beat 9 — The binary choice**

| #  | Turkish       | English            |
| -- | ------------- | ------------------ |
| 37 | Seçimin var. | You have a choice. |
| 38 | Devam et.     | Continue.          |
| 39 | Ya da kal.    | Or stay.           |
| 40 | Ve unut.      | And forget.        |
| 41 | ...           | ...                |

**Beat 10 — The doors pointed out**

| #  | Turkish         | English              |
| -- | --------------- | -------------------- |
| 42 | Kapılar orada. | The doors are there. |

**Closing**

| #  | Turkish                               | English                          |
| -- | ------------------------------------- | -------------------------------- |
| 43 | Bu sefer son olabilir.                | This time could be the end.      |
| 44 | Ya da sadece başka bir başlangıç. | Or just another beginning.       |
| 45 | Bol şans.                            | Good luck.                       |
| 46 | Bu defa gerçekten ihtiyacın olacak. | This time you'll really need it. |

*Table 17. Full Guard dialogue scripts for all three scenes. Original Turkish text with English translation. Beat groupings are documentation labels — they are not tagged in the `DialogueData` ScriptableObject assets. Delivered via `NPCDialogue` + `DialogueManager`; oxygen drain is paused for the full duration of each conversation.*

---

## 13. Technical Implementation

### Architecture Summary

```text
LostEnergy Namespace
├── GameManager
├── MusicManager
├── SettingsManager
├── SceneLoader
├── UIManager
└── GameLogger
```

### Core Scripts

| Script                   | Responsibility                                       |
| ------------------------ | ---------------------------------------------------- |
| `PlayerController3P`   | Character movement, sprint, jump, and orbit camera   |
| `OxygenSystem`         | Oxygen consumption and replenishment                 |
| `GameManager`          | Crystal counting, objective checking, and death flow |
| `CrystalCollectible`   | Crystal interaction, SFX and VFX triggering          |
| `HazardZone`           | Applying additional oxygen drain                     |
| `DialogueManager`      | Dialogue flow and oxygen pausing                     |
| `RespawnManager`       | Position reset after death                           |
| `PauseManager`         | Pause flow and panel management                      |
| `LoadingScreenManager` | Asynchronous scene loading and loading screen        |
| `GameLogger`           | File-based session event logger                      |

*Table 18. Core script inventory and their primary responsibilities. Scripts communicate through C# events (`UnityAction`, `Action<T>`) rather than direct references, keeping coupling low.*

### Log System

`GameLogger` is a singleton logger that is auto-spawned via `RuntimeInitializeOnLoadMethod` before any scene loads. It persists across all scene transitions using `DontDestroyOnLoad` and requires no manual GameObject placement.

Each session creates a new timestamped log file. A header block is written at startup containing the session timestamp, application version, platform, and Unity version.

**Log file location:**

| Environment | Path                                                            |
| ----------- | --------------------------------------------------------------- |
| Editor      | `<ProjectRoot>/Logs/Sessions/session_yyyy-MM-dd_HH-mm-ss.log` |
| Build       | `<BuildFolder>/Logs/session_yyyy-MM-dd_HH-mm-ss.log`          |

**Log line format:**

```text
[yyyy-MM-dd HH:mm:ss] | EVENT_TYPE | detail
```

**Logged event types:**

| Event Type     | Source Script(s)                                               | Detail Example                                         |
| -------------- | -------------------------------------------------------------- | ------------------------------------------------------ |
| `CRYSTAL`    | `CrystalCollectible`                                         | `3/25 \| Scene: SampleScene \| Pos: (12.0, 1.0, -4.5)` |
| `DEATH`      | `OxygenSystem`                                               | `Oxygen depleted`                                    |
| `SCENE_LOAD` | `ExitDoorSceneLoader`, `PauseManager`, `MainMenuManager` | `SampleScene2`                                       |
| `INTERACT`   | `PlayerInteraction`                                          | Target object name                                     |
| `PAUSE`      | `PauseManager`                                               | `Game paused`                                        |
| `RESUME`     | `PauseManager`                                               | `Game resumed`                                       |
| `RESTART`    | `GameOverRestartButton`, `RestartButton`                   | `Restarted after game over`                          |
| `UI`         | `PauseManager`, `MainMenuManager`                          | `Settings opened`                                    |
| `SETTINGS`   | `SettingsManager`                                            | `MusicVol increased: 0.50 → 0.80`                   |
| `MUTE`       | `SettingsManager`                                            | `Muted` / `Unmuted`                                |
| `BUTTON`     | `ButtonSfx`                                                  | GameObject name of the clicked button                  |
| `QUIT`       | `MainMenuManager`                                            | `Application quit`                                   |

*Table 18.1 - GameLogger event types, their source scripts, and example detail strings.*

### Technical Decisions

- **Modular script structure**: Each system (oxygen, crystals, hazards, dialogue) is encapsulated in its own script and communicates via C# events. This makes individual systems independently testable and allows `DialogueManager` to pause `OxygenSystem` without any coupling to `GameManager`.
- **`MusicManager` uses `DontDestroyOnLoad`**: This prevents music from restarting or cutting out during asynchronous scene loading. The alternative — restarting music from `Awake()` in each scene — was evaluated but produced an audible silence gap during transitions that broke immersion.
- **`SettingsManager` uses `PlayerPrefs`**: Volume preferences persist between play sessions without requiring a save file. A `ScriptableObject`-based approach was considered but rejected as unnecessarily complex for three simple float values.
- **References cached in `Start()`**: Components are located once at initialization and stored in private fields. Calling `GetComponent<T>()` every frame inside `Update()` at 60 fps would add unnecessary overhead across all active scripts.
- **`GameLogger` uses `RuntimeInitializeOnLoadMethod`**: The logger is spawned automatically before the first scene loads, eliminating the need to place it manually in every scene. File writes are guarded with a `lock` to prevent race conditions if multiple events fire on the same frame.

### Performance Notes

- The crystal VFX structure uses `Instantiate + Destroy` approach, sufficient for low object density.
- Scene scales are limited to short gameplay sessions, keeping overall runtime cost controlled.

---

## 14. Asset and License Information

### License Summary

| Source Group                    | License / Usage Terms                            |
| ------------------------------- | ------------------------------------------------ |
| Kenney assets                   | CC0 1.0 Universal                                |
| Unity Asset Store assets        | Standard Unity Asset Store EULA                  |
| Not Jam Music Pack              | CC0 1.0                                          |
| Flowerhead - SomeWhatGood: Lofi | Royalty free (author declaration)                |
| Freesound effects               | Per-file CC license                              |
| Unity built-in packages         | Unity Asset Store EULA / Unity Companion License |

*Table 19. High-level license summary by asset source group.*

### Kenney Assets

All Kenney assets are published under the [CC0 1.0 Universal](https://creativecommons.org/publicdomain/zero/1.0/) license.

| Asset Pack                       | Version | Source                                       |
| -------------------------------- | ------- | -------------------------------------------- |
| Animated Characters Protagonists | 1.1     | https://kenney.nl/assets/animated-characters |
| Fantasy Town Kit                 | 2.0     | https://kenney.nl/assets/fantasy-town-kit    |
| Graveyard Kit                    | 5.0     | https://kenney.nl/assets/graveyard-kit       |
| Nature Kit                       | 2.1     | https://kenney.nl/assets/nature-kit          |
| Platformer Kit                   | 4.1     | https://kenney.nl/assets/platformer-kit      |

*Table 20. Kenney asset packs used in the project. All are published under CC0 1.0 Universal — no attribution required.*

### Unity Asset Store Assets

| Asset Pack       | Publisher | Version |
| ---------------- | --------- | ------- |
| Stylized Crystal | LowlyPoly | 1.0     |
| Stylized door    | lowpoly89 | 1.0     |

*Table 21. Unity Asset Store assets used in the project.*

License: [Unity Asset Store End User License Agreement](https://unity.com/legal/as-terms)

### Audio and Music Sources

| File / Pack                         | Source                                              | License                           |
| ----------------------------------- | --------------------------------------------------- | --------------------------------- |
| 794489__gobbe57__coin-pickup.wav    | https://freesound.org/people/gobbe57/sounds/794489/ | Per-file Freesound license        |
| 822698__metris__retro-npc-voice.wav | https://freesound.org/people/Metris/sounds/822698/  | Per-file Freesound license        |
| ChillMenu.wav                       | https://not-jam.itch.io/not-jam-music-pack          | CC0 1.0                           |
| CriticalTheme.wav                   | https://not-jam.itch.io/not-jam-music-pack          | CC0 1.0                           |
| SwitchWithMeTheme.wav               | https://not-jam.itch.io/not-jam-music-pack          | CC0 1.0                           |
| Observatory and chill 2.wav         | https://flowerheadmusic.itch.io/somewhat-good-lofi  | Royalty free (author declaration) |

*Table 22. Individual audio and music file sources. Files obtained from Freesound require per-file license verification; CC BY files require written attribution.*

### Unity Built-in Packages

| Package               | Publisher          | License                 |
| --------------------- | ------------------ | ----------------------- |
| TextMesh Pro          | Unity Technologies | Unity Asset Store EULA  |
| Input System          | Unity Technologies | Unity Companion License |
| Post Processing Stack | Unity Technologies | Unity Companion License |

*Table 23. Unity built-in packages included via the Package Manager.*

### Usage Notes

- Usage terms for Unity Asset Store assets are covered under the standard Unity EULA.
- For files obtained from Freesound, the license type must be separately verified per file.
- If a CC BY licensed Freesound file is used, written attribution is mandatory.

---

## 15. Setup and Installation

### Development Environment Requirements

- Unity 6 (recommended: 6000.0.x LTS)
- Universal Render Pipeline (URP)
- Input System package

### Minimum System Requirements

| Requirement | Minimum Specification             |
| ----------- | --------------------------------- |
| OS          | Windows 10 64-bit                 |
| Processor   | Intel Core i3 class or equivalent |
| Memory      | 4 GB RAM                          |
| Graphics    | DirectX 11 compatible GPU         |
| Storage     | At least 1 GB free space          |
| Resolution  | 1280 × 720                       |

*Table 24. Minimum system requirements for running the compiled Windows build.*

### Build Steps

1. Open the `File -> Build Settings` menu in the Unity Editor.
2. Verify that the scenes are listed in the following order:
   - `0` MainMenu
   - `1` LoadingScreen
   - `2` SampleScene
   - `3` SampleScene2
   - `4` SampleScene3
3. Select `Windows, x86_64` as the platform.
4. Use the `Build` command and select `Builds/Windows/` as the output directory.

### Running the Game

The compiled build is launched from the `Builds/Windows/Lost Energy.exe` file.

### Quick Control Reference

```text
WASD        -> Move
Left Shift  -> Sprint
Space       -> Jump
E           -> Interact / Collect Crystal
Mouse       -> Camera
ESC         -> Pause Menu
```
