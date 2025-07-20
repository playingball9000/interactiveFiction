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
        GameController.invokeShowDialogueCanvas += ShowDialogueCanvas;
        GameController.invokeShowMainCanvas += ShowMainCanvas;
        GameController.invokeShowExploreCanvas += ShowExploreCanvas;
    }

    private void OnDisable()
    {
        GameController.invokeShowDialogueCanvas -= ShowDialogueCanvas;
        GameController.invokeShowMainCanvas -= ShowMainCanvas;
        GameController.invokeShowExploreCanvas -= ShowExploreCanvas;
    }

    void Start()
    {
        // LoggingUtil.Log("GameController Start");
        // using the delegate here to do other stuff attached to delegate
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
        WorldState.GetInstance().FLAG_dialogWindowActive = true;
        dialogueCanvas.gameObject.SetActive(false);
        mainDisplayCanvas.gameObject.SetActive(false);
        exploreCanvas.gameObject.SetActive(true);
    }

}
