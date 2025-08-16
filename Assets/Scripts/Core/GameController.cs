using UnityEngine;

/*
Make more stuff enums

*/

public class GameController : MonoBehaviour
{
    public Canvas dialogueCanvas;
    public Canvas mainDisplayCanvas;
    public Canvas exploreCanvas;

    /*
    Canvases are controlled by delegates and not events due to the nature of Unity lifecycle.
        InvokeCanvas -> enables canvas -> calls OnEnable for all sub elements

    OnEnable is where the event subscription happens, therefore, if events are used, it actually
    raises the events before the UI objects are subscribed (they are unsubscribed during OnDisable)
    */
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

        EventManager.Subscribe(GameEvent.DieInArea, ResetPlayerOnDeath);
    }

    private void OnDisable()
    {
        invokeShowDialogueCanvas -= ShowDialogueCanvas;
        invokeShowMainCanvas -= ShowMainCanvas;
        invokeShowExploreCanvas -= ShowExploreCanvas;

        EventManager.Unsubscribe(GameEvent.DieInArea, ResetPlayerOnDeath);
    }

    void Start()
    {
        // invokeShowMainCanvas();
        invokeShowExploreCanvas();
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

        // Need to raise event after canvas has been activated to make sure all UI elements have subscribed
        EventManager.Raise(GameEvent.EnterArea);
    }

    public void ResetPlayerOnDeath()
    {
        PlayerContext.Get.currentLocation = RoomRegistry.GetRoom(LocationCode.StartingCamp_r);
        // Always call the delegate to do all the related stuff
        invokeShowMainCanvas();
        CoroutineRunner.Instance.RunCoroutine(CommonCoroutines.Wait(0.5f));
        StoryTextHandler.invokeUpdateStoryDisplay("\nYou died... but it is not the end.\n", TextEffect.Typewriter);

    }

}
