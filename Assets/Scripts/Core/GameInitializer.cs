using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    public Room startingRoom;

    void Awake()
    {
        WorldState.GetInstance().player = new Player("Player", "Player description", startingRoom);

        NPC woman = new Woman();
        woman.referenceName = "Trapped Woman";
        woman.description = @"The woman is striking in a casual way. Her chestnut hair falls in loose waves around her shoulders. A faint sheen of blush colors her cheeks, though whether from embarrassment or the warmth of the train car is unclear. Her hazel eyes are sharp and expressive, darting between you and her trapped ankles with a mix of self-conscious humor and a silent plea for help.

Her legs, though trapped, are slender and toned, the hem of her jeans slightly pulled up to reveal smooth, tanned skin. Despite her predicament, she manages a small, apologetic smile, her demeanor equal parts charm and awkward.";
        woman.currentLocation = startingRoom;

        Shoes pumps = new Shoes("Pumps", "The high heels are sleek and stylish, a glossy black that reflects the warm light filtering through the train's windows. The pointed toes are sharp and elegant, complemented by a modest stiletto heel that suggests both sophistication and practicality. A faint scuff on the side of the right shoe hints at frequent wear, but the leather remains supple and well-maintained.", "black");
        woman.clothes.Add(pumps);

        woman.bodyParts.Add(new BodyPart("feet", "Her feet were petite and impeccably cared for, the kind that might belong to someone who indulged in regular spa visits. Her toes were adorned with a glossy coat of pale pink polish that complemented her tan skin. They exuded an effortless femininity, adding to her overall allure.", true));

        startingRoom.npcs.Add(woman);

    }

    private void Start()
    {
        DisplayTextHandler.invokeDisplayRoomText(startingRoom);
    }

}
