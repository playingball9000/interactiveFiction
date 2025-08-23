using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    //Setup for the game here
    void Awake()
    {
        SetupCards.initialize();
        CardUtil.InitialUnlockCards();
        AreaRegistry.Initialize(); // Run after setup cards

        TimerManager.Instance.CreateTimer(TimerCode.Test, 1f, () =>
        {
            Debug.Log("💥 Boom!");
        });

        Room startingCamp = new RoomBuilder("Starting Camp", LocationCode.StartingCamp_r)
            .WithDescription(@"A few weather-beaten tents cluster together on a narrow plateau overlooking the chasm. Crates of supplies litter the ground. The air is thick with the scent of damp earth, and greenery. Nearby, a small bonfire crackles weakly, fighting back the creeping cold.")
            .WithExit(ExitDirection.South, LocationCode.AbyssEntrance_r)
            .Build();


        Room abyssEntrance = new RoomBuilder("Abyss Entrance", LocationCode.AbyssEntrance_r)
            .WithDescription(@"Here, the ground falls away into a vast, gaping darkness. Jagged rock walls curve inward, forming what looks like an enormous throat. Peering down, you catch glimpses of the first layer: lush, tangled vegetation clings to sheer walls.")
            .WithExit(ExitDirection.North, LocationCode.StartingCamp_r)
            .WithExit(ExitDirection.Enter, LocationCode.Abyss_a)
            .Build()
            .AddScenery("darkness", "A thick haze blocks your view beyond a few hundred meters.")
            .AddScenery("walls", "rock", "Dangerous looking.")
            .AddScenery("layer", "first", "The very start of every adventure.")
            .AddScenery("vegetation", "Lush and green");


        Player player = PlayerFactory.CreateNew("", LocationCode.StartingCamp_r);

        // TODO:This is silly, i set npc.currentLocation but also room.addNpc also sets it... should be able to do it once...
        NPC npcGrace = new Grace
        {
            displayName = "Grace",
            internalCode = "npc_grace",
            description = @"A petite girl. Her bright, emerald-green eyes sparkle with excitement and mischief, scanning every crevice and glimmer as though each one hides a secret treasure. Her hair, a wild tangle of chestnut curls, is tied back in a messy ponytail with a faded red ribbon that flutters behind her like a tiny flag.",
            currentLocation = startingCamp,
            dialogueFile = "womanDialogue"
        };

        startingCamp.AddNPC(npcGrace);

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
            displayName = "bag"
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
            displayName = "key",
            adjective = "master",
            unlocks = "door_room_102",
            internalCode = "item_master_key",
            description = "A key to all doors.",
        };

        // player.AddToInventory(masterKey);

        RoomBuilder.FinalizeRooms();

        WorldState.GetInstance().player = player;
        WorldState.GetInstance().roomData = RoomRegistry.GetDict();
        WorldState.GetInstance().areaData = AreaRegistry.GetDict();
        WorldState.GetInstance().cardData = CardRegistry.GetDict();
    }

    // Put stuff that happens at the start of the game here
    private void Start()
    {
        StoryTextHandler.invokeUpdateStoryDisplay(@"At the edge of the world lays a hole so deep it might as well be endless. A colossal vertical pit known only as the Abyss. Its gaping maw is wrapped in mist and mystery, calling out to those who yearn for adventure. Layers upon layers spiral downward, each home to ancient ruins, otherworldly creatures, and powerful relics. However, the deeper one ventures, the stranger, and deadlier, the world becomes...");
    }

}
