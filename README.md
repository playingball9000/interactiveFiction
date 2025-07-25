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
2. Have Memory, which is just key value store of Facts basically. Used to run rules.

## Core

1. Pretty much all the main interfaces/managers for the UI, gameplay setup, etc.
2. Most of these scripts are attached to the GameController.

## Adding Actions

1. Actions are invoked via the command pattern
1. Actions should implement IPlayerAction
1. Make sure to update the ActionRegistry and ActionSynonyms with any new ones

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
10. Open / Close
11. Put
12. Equip / Unequip

### Inventory

1. Anything that can be placed in the inventory needs to implement IItem
2. Mostly I use ItemBase for items.

### Containers

1. Stuff that holds stuff, has a list of IItems in it.
2. Anything that can be a container needs to extend ContainerBase.
3. You can put containers in Inventory, but can toggle with isGettable

### Wearable ie. clothing

1. Use WearableBase
2. Wearables have a slot (head, body, legs, etc) and layer (base, mid, outer)
3. You can have Socks (feet, base) and Shoes (feet, outer) or shirt(body, mid) and jacket (body, outer)
4. Wearables can have multiple slots like Jumpsuit would take up (body, legs)

### Examine

1. Anything examinable needs to implement IExaminable

## Dialogue

1. Built in DialogueBuilder
2. DialogueParser manages all the interactions and UI coordination
3. Can add rules to choices to determine whether to show them.
4. Can add actions (code) to choices to run the code if the player chooses that in a dialogue.

## The Rule Engine

1. Fact - flat data field representing the world in key:value form. ex. "in_inventory":"book"
2. Query - List of facts representing the state (player, player memory, world, etc.)
3. Criteria - A test for a Fact. ex. exists("in_inventory", "book") or equals("concept", "onMove")
4. Rule - A list of criteria. When a query satisfies the rule, a corresponding action will be done.

## Other

1. Certain classes have an internalCode field. This is for use for referencing it in the code, for example in the rule engine as opposed what is displayed on the UI.

# Deploy to itch.io

1. File > Build Profiles
2. Select "Web" and then click the "Build" button (may need to install some build packs)
3. Export to whatever folder you want
4. Zip all the items in that folder (not the folder itself)
5. Upload the zip file (Create/Edit game > upload)

# Reference

    [Awake]         ← called first (even if disabled)
        ↓
    [OnEnable]      ← if object is active
        ↓
    [Start]         ← if object is active, before first Update
        ↓
    [Update]        ← every frame
        ↓
    [LateUpdate]    ← every frame, after Update
        ↓
    [OnDisable]     ← when disabled
        ↓
    [OnEnable]      ← if re-enabled
        ↓
    [OnDestroy]     ← before destroyed

# Markers

TODO:
QUESTION:
