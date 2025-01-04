# interactiveFiction

## Setup
1. Open Unity Hub
2. "Select Add project from disk"
3. ![image](https://github.com/user-attachments/assets/9416fd4a-d36b-479b-a762-22a4fda393bf)
4. Find the cloned github project and add that folder.

Make sure you load the "ifScene" before you start.

## Rooms
1. Rooms have exits, NPCs, and what not. Should be pretty self explanatory.
1. Unlike the other things, Rooms are controlled through the Unity Inspector.

## Actors
1. Player, NPC, etc.

## Core
1. Pretty much all the main interfaces/managers for the UI, gameplay setup, etc.

## Adding Actions
1. Actions are invoked via the command pattern
1. Actions should extend IPlayerAction
1. Make sure to update the ActionRegistry and ActionSynonyms with any new 

## Other
1. The other notable is IExaminable which you'll want to extend if you want to make something actionable from the "examine" action
