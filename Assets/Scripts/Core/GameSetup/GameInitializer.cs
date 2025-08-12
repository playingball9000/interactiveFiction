using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    //Setup for the game here
    void Awake()
    {
        CardRegistry.Initialize();
        AreaRegistry.Initialize(); // Run after CardRegistry
        CardUtil.InitialUnlockCards();

        TimerManager.Instance.CreateTimer(TimerCode.Test, 1f, () =>
        {
            Debug.Log("💥 Boom!");
        });

        //TODO: probably a good idea to make a hashmap of all the rooms for easy lookup
        Room startingCamp = new()
        {
            displayName = "Starting Camp",
            internalCode = RoomConstants.STARTING_CAMP,
            description = @"A few weather-beaten tents cluster together on a narrow plateau overlooking the chasm. Crates of supplies litter the ground. The air is thick with the scent of damp earth, and greenery. Nearby, a small bonfire crackles weakly, fighting back the creeping cold."
        };

        Room abyssEntrance = new()
        {
            displayName = "Abyss Entrance",
            internalCode = RoomConstants.ABYSS_ENTRANCE,
            description = @"Here, the ground falls away into a vast, gaping darkness. Jagged rock walls curve inward, forming what looks like an enormous throat. Peering down, you catch glimpses of the first layer: lush, tangled vegetation clings to sheer walls."
        };

        RoomRegistry.Register(startingCamp);
        RoomRegistry.Register(abyssEntrance);

        RoomFactory.LinkRoomsTwoWay(abyssEntrance, startingCamp, ExitDirection.North);


        Area startArea = AreaRegistry.GetArea("abyss_area");

        abyssEntrance.exits.Add(new Exit { exitDirection = ExitDirection.Enter, targetDestination = startArea });


        Player player = new()
        {
            playerName = "Player",
            description = "description",
            // currentLocation = startArea
            currentLocation = startArea
        };

        var startingStats = new Dictionary<Stat, float>()
        {
            { Stat.Health, 72 },
            { Stat.Food, 21 },
            { Stat.Water, 10 },
            { Stat.Strength, 12 },
            { Stat.Agility, 8 },
            { Stat.Intelligence, 6 }
        };

        player.stats.InitializeBaseStats(startingStats);


        // TODO:This is silly, i set npc.currentLocation but also room.addNpc also sets it... should be able to do it once...
        NPC npcGrace = new Grace
        {
            referenceName = "Grace",
            internalCode = "npc_grace",
            description = @"A petite girl. Her bright, emerald-green eyes sparkle with excitement and mischief, scanning every crevice and glimmer as though each one hides a secret treasure. Her hair, a wild tangle of chestnut curls, is tied back in a messy ponytail with a faded red ribbon that flutters behind her like a tiny flag.",
            currentLocation = startingCamp,
            dialogueFile = "womanDialogue"
        };

        startingCamp.AddNpc(npcGrace);

        startingCamp.roomScenery.AddRange(new List<Scenery>() {
            new Scenery {
                referenceName = "campfire",
                description = "A sad looking fire."
            },
            new Scenery {
                referenceName = "tents",
                description = "Morose looking tents."
            }
            });


        abyssEntrance.roomScenery.AddRange(new List<Scenery>() {
            new Scenery {
                referenceName = "darkness",
                description = "A thick haze blocks your view beyond a few hundred meters."
            },
            new Scenery {
                referenceName = "walls",
                adjective = "rock",
                description = "Dangerous looking."
            },
            new Scenery {
                referenceName = "layer",
                adjective = "first",
                description = "The very start of every adventure."
            },
            new Scenery {
                referenceName = "vegetation",
                description = "Lush and green"
            }
            });


        IItem itemBottle = ItemFactory.CreateItem(
            "bottle",
            "metal",
            "A dented metal container, still cool to the touch. It's about three-quarters full of water."
            );
        IItem itemBar = ItemFactory.CreateItem(
            "bar",
            "chocolate",
            "The label boasts a 'Rich, Decadent' experience. The ingredient list suggests otherwise."
        );

        ContainerBase bag = new()
        {
            contents = new List<IItem>() { itemBottle, itemBar },
            internalCode = "bag",
            description = "Your trusty bag, never leave home without it.",
            adjective = "duffel",
            isGettable = true,
            isOpen = false,
            pickUpNarration = "You hoist your trusty bag.",
            referenceName = "bag"
        };

        startingCamp.AddItem(bag);

        player.relationships.Add(npcGrace.internalCode, new Relationship { points = 0 });

        // IWearable gasMask = new WearableBase()
        // {
        //     referenceName = "mask",
        //     adjective = "gas",
        //     internalCode = "wearable_gas_mask",
        //     description = "Your trusty gas mask, protects you from all kinds of gas. The filter is one you stole from the UN.",
        //     layer = ClothingLayer.Outer,
        //     slotsTaken = new() { EquipmentSlot.Face },
        //     attributes = new() {
        //         new Attribute() {
        //             displayName = "Gas Immunity",
        //             internalCode = "ability_gas_immunity",
        //             type = AttributeType.Ability,
        //         }
        //     }
        // };

        // player.AddToInventory(gasMask);
        IItem masterKey = new KeyBase
        {
            referenceName = "key",
            adjective = "master",
            unlocks = "door_room_102",
            internalCode = "item_master_key",
            description = "A key to all doors.",
        };

        // player.AddToInventory(masterKey);

        WorldState.GetInstance().player = player;
        WorldState.GetInstance().roomsData.AddRange(RoomRegistry.GetAllRooms());
    }

    // Put stuff that happens at the start of the game here
    private void Start()
    {
        StoryTextHandler.invokeUpdateStoryDisplay(@"At the edge of the world lays a hole so deep it might as well be endless. A colossal vertical pit known only as the Abyss. Its gaping maw is wrapped in mist and mystery, calling out to those who yearn for adventure. Layers upon layers spiral downward, each home to ancient ruins, otherworldly creatures, and powerful relics. However, the deeper one ventures, the stranger, and deadlier, the world becomes...");
    }

}
