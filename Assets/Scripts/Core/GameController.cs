using UnityEngine;

/*
Overall list of stuff I'm thinking about

- Should I have a IHideable for stuff i don't want to immediately show
- how to implement skill checks

*/

public class GameController : MonoBehaviour
{
    public Canvas dialogueCanvas;
    public Canvas mainDisplayCanvas;
    public Canvas exploreCanvas;


    public delegate void ShowDialogueCanvasDelegate();
    public static ShowDialogueCanvasDelegate invokeShowDialogueCanvas;

    public delegate void ShowMainCanvasDelegate();
    public static ShowMainCanvasDelegate invokeShowMainCanvas;

    public delegate void ShowExploreCanvasDelegate();
    public static ShowExploreCanvasDelegate invokeShowExploreCanvas;

    private void OnEnable()
    {
        invokeShowDialogueCanvas += ShowDialogueCanvas;
        invokeShowMainCanvas += ShowMainCanvas;
        invokeShowExploreCanvas += ShowExploreCanvas;

        GameEvents.OnEnterArea += ShowExploreCanvas;
        GameEvents.OnDieInArea += ResetPlayerOnDeath;
    }

    private void OnDisable()
    {
        invokeShowDialogueCanvas -= ShowDialogueCanvas;
        invokeShowMainCanvas -= ShowMainCanvas;
        invokeShowExploreCanvas -= ShowExploreCanvas;
    }

    void Start()
    {
        // invokeShowMainCanvas();
        // invokeShowExploreCanvas();
        GameEvents.RaiseEnterArea();
    }


    public void ShowMainCanvas()
    {
        WorldState.GetInstance().FLAG_dialogWindowActive = false;
        dialogueCanvas.gameObject.SetActive(false);
        mainDisplayCanvas.gameObject.SetActive(true);
        exploreCanvas.gameObject.SetActive(false);
    }

    public void ShowDialogueCanvas()
    {
        WorldState.GetInstance().FLAG_dialogWindowActive = true;
        dialogueCanvas.gameObject.SetActive(true);
        mainDisplayCanvas.gameObject.SetActive(false);
        exploreCanvas.gameObject.SetActive(false);
    }


    public void ShowExploreCanvas()
    {
        WorldState.GetInstance().FLAG_dialogWindowActive = false;
        dialogueCanvas.gameObject.SetActive(false);
        mainDisplayCanvas.gameObject.SetActive(false);
        exploreCanvas.gameObject.SetActive(true);
    }

    public void ResetPlayerOnDeath()
    {
        PlayerContext.Get.currentLocation = RoomRegistry.GetRoom(RoomConstants.STARTING_CAMP);
        // Always call the delegate to do all the related stuff
        invokeShowMainCanvas();
        CoroutineRunner.Instance.RunCoroutine(CommonCoroutines.Wait(0.5f));
        StoryTextHandler.invokeUpdateStoryDisplay("\nYou died... but it is not the end.\n", UiConstants.EFFECT_TYPEWRITER);

    }

}
