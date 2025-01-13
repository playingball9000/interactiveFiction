# interactiveFiction

## Setup
1. Open Unity Hub
2. "Select Add project from disk"
3. ![image](https://github.com/user-attachments/assets/9416fd4a-d36b-479b-a762-22a4fda393bf)
4. Find the cloned github project and add that folder.

- Make sure you load the "ifScene" before you start.
- All code is in Assets/Scripts
- GameInitializer.cs is used to setup all the classes and WorldState
- WorldState keeps track of the game state during play

## Rooms
1. Rooms have exits, NPCs, and what not. Should be pretty self explanatory.
2. Scenery is stuff that player can examine but not pick up. Should extend class Scenery.

## Actors
1. Player, NPC, etc.

## Core
1. Pretty much all the main interfaces/managers for the UI, gameplay setup, etc.
2. Most of these scripts are attached to the GameController GameObject and the necessary elements are injected via the Unity Inspector.

## Adding Actions
1. Actions are invoked via the command pattern
1. Actions should extend IPlayerAction
1. Make sure to update the ActionRegistry and ActionSynonyms with any new

### Action List
1. Look
2. Talk
3. Examine
4. Get
5. Drop
6. Inventory
7. Save
8. Load
9. Movement

### Inventory
1. Anything that can be placed in the inventory needs to extend IItem

### Examine
1. Anything examinable needs to extend IExaminable

## Dialogue
Located in Resources/Dialogue (file must match name of dialogueFile field in NPC)



