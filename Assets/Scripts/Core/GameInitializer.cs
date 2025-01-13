using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    //Setup for the game here
    void Awake()
    {
        Room trainCarTwo = new Room
        {
            roomName = "TrainCarTwo",
            description = "The train car is modestly lit, with rows of cushioned seats lining both sides of the narrow aisle. A young woman in a hoodie dozes against the window, earbuds trailing down to her lap. Across from her, a middle-aged man in a wrinkled suit adjusts his tie, glancing at his watch with a faint air of impatience. Near the middle of the car, a couple in hiking gear chat quietly, a bulky backpack tucked awkwardly under their seat. Overhead, luggage racks brim with suitcases and duffel bags, one precariously teetering at the edge. The occasional rattle of the train blends with the low hum of conversation and the rhythmic clack of wheels on the tracks."
        };

        Room startingRoom = new Room
        {
            roomName = "StartingTrainCar",
            description = "The train car stretched out in quiet solitude. Rows of cushioned seats, most unoccupied, lined either side of the aisle, their fabric worn smooth from years of passengers coming and going. Overhead, a few stray luggage compartments hung ajar. The faint scent of aged upholstery and the metallic tang of the train itself mingled in the still air."
        };

        Exit startingRoomExit = new Exit
        {
            exitDirection = ExitDirection.north,
            targetRoom = trainCarTwo
        };

        startingRoom.exits.Add(startingRoomExit);

        Exit trainCarTwoExit = new Exit
        {
            exitDirection = ExitDirection.south,
            targetRoom = startingRoom
        };

        trainCarTwo.exits.Add(trainCarTwoExit);

        Player player = new Player
        {
            playerName = "Player",
            description = "description",
            currentLocation = startingRoom
        };


        NPC woman = new Woman
        {
            referenceName = "Woman",
            description = @"The woman is striking in a casual way. Her her auburn hair falling in loose waves around her shoulders, framing a face that was both striking and sincere.  Her outfit was casual yet put-together—fitted jeans, a maroon sweater that hugged her frame, and black ankle boots that looked both practical and stylish.",
            currentLocation = startingRoom,
            dialogueFile = "womanDialogue"
        };

        startingRoom.npcs.Add(woman);

        IItem book = new Book
        {
            referenceName = "book",
            description = "an old book with a worn cover"
        };

        startingRoom.roomItems.Add(book);

        IExaminable seats = new Scenery
        {
            referenceName = "seats",
            description = "The cushioned seats sag with the wear of countless journeys. The once-vibrant fabric now bears the muted imprint of forgotten travelers, stories folded into the wrinkles of time."
        };

        startingRoom.roomScenery.Add(seats);

        WorldState.GetInstance().player = player;

        WorldState.GetInstance().rooms.Add(startingRoom);
        WorldState.GetInstance().rooms.Add(trainCarTwo);
    }

    // Put stuff that happens at the start of the game here
    private void Start()
    {
        StoryTextHandler.invokeUpdateTextDisplay("The train hummed softly as it cut through the countryside, the rhythmic clatter of its wheels blending seamlessly with the muted murmur of other passengers. You’re seated by the window, gazing out at the scenery rushing by. Fields of tall grass sway in the breeze, their golden heads catching the warm hues of the setting sun. A cluster of trees here, a scattering of wildflowers there—they’re gone in an instant, replaced by something else fleeting yet equally picturesque.\r\n\r\nYou lean back in the seat, the plush upholstery giving slightly beneath you. The faint aroma of brewed coffee wafts through the air, a comforting counterpoint to the occasional squeak of the train’s brakes as it adjusts speed. A low creak from the seat across the aisle draws your attention momentarily, where an elderly man adjusts his hat, his hands gnarled but steady. He nods at you with a faint smile before returning to his book—a worn volume with a cracked spine.\r\n\r\nThe scene outside shifts again, now a stretch of calm water reflecting the deepening colors of twilight. Your reflection in the glass momentarily overlaps with a flash of a distant town’s lights. For a brief moment, you consider the train’s destination. Nowhere in particular, you remind yourself. The journey itself is the point, isn’t it?");
    }

}
