using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    //Setup for the game here
    void Awake()
    {
        Room trainCarTwo = new()
        {
            roomName = "TrainCarTwo",
            description = "The train car is modestly lit, with rows of cushioned seats lining both sides of the narrow aisle. Overhead, luggage racks brim with suitcases and duffel bags, one precariously teetering at the edge. The occasional rattle of the train blends with the low hum of conversation and the rhythmic clack of wheels on the tracks."
        };

        Room startingRoom = new()
        {
            roomName = "StartingTrainCar",
            description = "The train car stretched out in quiet solitude. Rows of cushioned seats, most unoccupied, lined either side of the aisle, their fabric worn smooth from years of passengers coming and going. Overhead, a few stray luggage compartments hung ajar. The faint scent of aged upholstery and the metallic tang of the train itself mingled in the still air."
        };

        Exit startingRoomExit = new()
        {
            exitDirection = ExitDirection.north,
            exitDescription = "There is a doorway to another car to the north",
            targetRoom = trainCarTwo
        };

        startingRoom.exits.Add(startingRoomExit);

        Exit trainCarTwoExit = new()
        {
            exitDirection = ExitDirection.south,
            exitDescription = "There is a doorway to another car to the south",
            targetRoom = startingRoom
        };

        trainCarTwo.exits.Add(trainCarTwoExit);

        Player player = new()
        {
            playerName = "Player",
            description = "description",
            currentLocation = startingRoom
        };


        NPC woman = new Woman
        {
            referenceName = "Woman",
            description = @"The woman is striking in a casual way. Her her auburn hair falling in loose waves around her shoulders, framing a face that was both striking and sincere.  Her outfit was casual yet put-together fitted jeans, a maroon sweater that hugged her frame, and black ankle boots that looked both practical and stylish.",
            currentLocation = startingRoom,
            dialogueFile = "womanDialogue"
        };

        startingRoom.npcs.Add(woman);

        IItem book = new ItemBase
        {
            referenceName = "book",
            description = "An old book with a worn cover",
            isGettable = true,
        };

        startingRoom.roomItems.Add(book);

        IExaminable seats = new Scenery
        {
            referenceName = "seats",
            description = "The cushioned seats sag with the wear of countless journeys. The once-vibrant fabric now bears the muted imprint of forgotten travelers, stories folded into the wrinkles of time."
        };

        startingRoom.roomScenery.Add(seats);

        IItem quill = new ItemBase
        {
            referenceName = "quill",
            description = "A simple feather quill",
            isGettable = true,
            pickUpNarration = "You pick up the feather quill. Carefully as to not get ink on you."
        };


        ContainerBase suitcase = new()
        {
            contents = new List<IItem>() { quill },
            description = "A small suitcase. There are a few stickers on the side denoting the places it has travelled to.",
            isGettable = false,
            isLocked = false,
            isOpen = false,
            pickUpNarration = "This doesn't belong to you.",
            referenceName = "suitcase"
        };

        startingRoom.roomItems.Add(suitcase);


        WorldState.GetInstance().player = player;

        WorldState.GetInstance().rooms.Add(startingRoom);
        WorldState.GetInstance().rooms.Add(trainCarTwo);
    }

    // Put stuff that happens at the start of the game here
    private void Start()
    {
        StoryTextHandler.invokeUpdateStoryDisplay("The train hummed softly as it cut through the countryside, the rhythmic clatter of its wheels blending seamlessly with the muted murmur of other passengers. You're seated by the window, gazing out at the scenery rushing by. Fields of tall grass sway in the breeze, their golden heads catching the warm hues of the setting sun. A cluster of trees here, a scattering of wildflowers there.");
    }

}
