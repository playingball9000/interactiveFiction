using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    //Setup for the game here
    void Awake()
    {
        Room trainCarTwo = new()
        {
            roomName = "A Spacious Train Car",
            internalCode = "room_carTwo",
            description = "The train car is modestly lit, with rows of cushioned seats lining both sides of the narrow aisle. Overhead, luggage racks brim with suitcases and duffel bags, one precariously teetering at the edge.  At the far end of the car, a service trolley rests unattended, its surface cluttered with various items."
        };

        Room startingRoom = new()
        {
            roomName = "Comfy Train Car",
            internalCode = "room_start",
            description = "You are sitting in the booth seats of a cozy train car, the rhythmic clatter of the wheels filling the air like a steady heartbeat. The large window beside you frames a rolling countryside. Overhead, a luggage rack runs the length of the car. A small stack of folded newspapers lays forlorn on a nearby seat. The air carries a faint scent of leather, paper, and coffee."
        };

        Room startingRoom_berth = new()
        {
            roomName = "Room 100",
            internalCode = "room_start_berth",
            description = "You are in a compact sleeper berth. The space is just big enough for a narrow bed, a small storage compartment, and a dim overhead light casting a warm glow. The air is still, carrying the faint scent of fabric and metal. A thin door separates you from the rest of the hallway."
        };

        startingRoom_berth.exits.Add(new()
        {
            exitDirection = ExitDirection.Enter,
            targetRoom = startingRoom,
        });

        startingRoom.exits.Add(new()
        {
            exitDirection = ExitDirection.Enter,
            targetRoom = startingRoom_berth,
        });

        startingRoom.exits.Add(new()
        {
            exitDirection = ExitDirection.North,
            exitDescription = "There is a doorway to another car to the north",
            targetRoom = trainCarTwo,
        });

        trainCarTwo.exits.Add(new()
        {
            exitDirection = ExitDirection.South,
            exitDescription = "There is a doorway to another car to the south",
            targetRoom = startingRoom
        });

        Player player = new()
        {
            playerName = "Player",
            description = "description",
            currentLocation = startingRoom
        };


        NPC npc_kate = new Woman
        {
            referenceName = "Kate",
            internalCode = "npc_kate",
            description = @"The woman is striking in a casual way. Her her auburn hair falling in loose waves around her shoulders, framing a face that was both striking and sincere.  Her outfit was casual yet put-together fitted jeans, a maroon sweater that hugged her frame, and black ankle boots that looked both practical and stylish.",
            currentLocation = startingRoom,
            dialogueFile = "womanDialogue"
        };

        startingRoom.npcs.Add(npc_kate);

        startingRoom.roomItems.AddItem(new ItemBase
        {
            referenceName = "book",
            adjective = "old",
            internalCode = "item_old_book",
            description = "An old book with a worn cover. Inside, someone has doodled a mustache on a portrait of a very serious-looking duke.",
        });

        startingRoom.roomScenery.AddRange(new List<Scenery>() {
            new Scenery {
                referenceName = "seats",
                adjective = "booth",
                description = "The cushioned seats sag with the wear of countless journeys. The once-vibrant fabric now bears the muted imprint of many travelers' rear ends."
            },
            new Scenery {
                referenceName = "window",
                adjective = "large",
                description = "The window offers a sweeping view of fields and distant hills. A lone cow stands in the middle of a pasture, staring directly at the train. You wonder what it could be thinking about."
            },
            new Scenery {
                referenceName = "rack",
                adjective = "luggage",
                description = "The overhead luggage rack lies empty. You could have put your bag there, but you didn't."
            },
            new Scenery {
                referenceName = "newspapers",
                adjective = "folded",
                description = "A stack of what seems to be old Chinese newspapers... 'Chiang Kai Shek died'."
            }
            });

        trainCarTwo.roomScenery.AddRange(new List<Scenery>() {
            new Scenery {
                referenceName = "seats",
                description = "More train seats, nothing you haven't seen a lifetime of."
            },
            new Scenery {
                referenceName = "rack",
                adjective = "luggage",
                description = "You see a few bags, nothing of interest except the duffel bag."
            },
            new Scenery {
                referenceName = "trolley",
                adjective = "service",
                description = "The service trolley is a battlefield of abandoned beverages. Half-empty coffee cups form a sad little army waiting for reinforcements. A crumpled napkin lies in defeat, stained with the remains of what was probably a chocolate muffin — a tragic end for a noble pastry."
            }
            });

        IItem quill = new ItemBase
        {
            referenceName = "quill",
            adjective = "feather",
            internalCode = "item_feather_quill",
            description = "A simple feather quill with many possibilities.",
            pickUpNarration = "You pick up the feather quill. Carefully as to not get ink on you."
        };

        ContainerBase bag = new()
        {
            contents = new List<IItem>() { quill },
            description = "A small bag. There are a few stickers on the side denoting the places it has travelled to.",
            adjective = "duffel",
            isGettable = false,
            isOpen = false,
            pickUpNarration = "This doesn't belong to you.",
            referenceName = "bag"
        };

        ContainerBase bag2 = new()
        {
            contents = new List<IItem>(),
            description = "A sleek looking back pack. There is a hole in the bottom.",
            referenceName = "pack",
            adjective = "back",
            isGettable = false,
            isOpen = true,
            pickUpNarration = "This doesn't belong to you.",
        };

        trainCarTwo.roomItems.AddItem(bag);
        trainCarTwo.roomItems.AddItem(bag2);
        trainCarTwo.roomItems.AddItem(new ItemBase
        {
            referenceName = "bar",
            adjective = "chocolate",
            internalCode = "item_chocolate_bar",
            description = "The label boasts a 'Rich, Decadent' experience. The ingredient list suggests otherwise.",
        });

        player.relationships.Add(npc_kate.internalCode, new Relationship { points = 0 });

        IWearable gasMask = new WearableBase()
        {
            referenceName = "mask",
            adjective = "gas",
            internalCode = "wearable_gas_mask",
            description = "Your trusty gas mask, protects you from all kinds of gas. The filter is one you stole from the UN.",
            layer = ClothingLayer.Outer,
            slotsTaken = new() { EquipmentSlot.Face },
            attributes = new() {
                new Attribute() {
                    displayName = "Gas Immunity",
                    internalCode = "ability_gas_immunity",
                    type = AttributeType.Ability,
                }
            }
        };

        player.inventory.AddItem(gasMask);

        WorldState.GetInstance().player = player;

        WorldState.GetInstance().rooms.Add(startingRoom);
        WorldState.GetInstance().rooms.Add(trainCarTwo);
    }

    // Put stuff that happens at the start of the game here
    private void Start()
    {
        StoryTextHandler.invokeUpdateStoryDisplay("The train hummed softly as it cut through the countryside. You're seated by the window, gazing out at the scenery rushing by. Fields of tall grass sway in the breeze, their golden heads catching the warm hues of the setting sun. A cluster of trees here, a scattering of wildflowers there. The booth where you sit is quiet, your bag neatly tucked beneath your feet. A faint smell of coffee and worn leather lingers in the air.");
    }

}
