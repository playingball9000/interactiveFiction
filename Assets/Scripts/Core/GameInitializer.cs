using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    public Room startingRoom;

    void Awake()
    {
        WorldState.GetInstance().player = new Player("Player", "Player description", startingRoom);

        NPC woman = new Woman();
        woman.referenceName = "Woman";
        woman.description = @"The woman is striking in a casual way. Her her auburn hair falling in loose waves around her shoulders, framing a face that was both striking and sincere.  Her outfit was casual yet put-together—fitted jeans, a maroon sweater that hugged her frame, and black ankle boots that looked both practical and stylish.";
        woman.currentLocation = startingRoom;

        startingRoom.npcs.Add(woman);

    }

    private void Start()
    {
        DisplayTextHandler.invokeDisplayRoomText(startingRoom);
    }

}
