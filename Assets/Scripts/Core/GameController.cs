using UnityEngine;

public class GameController : MonoBehaviour
{
    DialogueParser dialogueParser;

    public Canvas dialogueCanvas;
    public Canvas mainDisplayCanvas;

    public delegate void ShowDialogueCanvasDelegate();
    public static ShowDialogueCanvasDelegate invokeShowDialogueCanvas;

    public delegate void ShowMainCanvasDelegate();
    public static ShowMainCanvasDelegate invokeShowMainCanvas;

    public delegate void ProcessPlayerActionDelegate(string[] inputTextArray);
    public static ProcessPlayerActionDelegate invokeProcessPlayerAction;

    private void OnEnable()
    {
        GameController.invokeShowDialogueCanvas += ShowDialogueCanvas;
        GameController.invokeShowMainCanvas += ShowMainCanvas;
        GameController.invokeProcessPlayerAction += ProcessPlayerAction;
    }

    private void OnDisable()
    {
        GameController.invokeShowDialogueCanvas -= ShowDialogueCanvas;
        GameController.invokeShowMainCanvas -= ShowMainCanvas;
        GameController.invokeProcessPlayerAction -= ProcessPlayerAction;
    }

    void Start()
    {
        dialogueParser = GetComponent<DialogueParser>();
        // using the delegate here to do other stuff attached to delegate
        invokeShowMainCanvas();
    }


    public void ShowMainCanvas()
    {
        dialogueCanvas.gameObject.SetActive(false);
        mainDisplayCanvas.gameObject.SetActive(true);
    }

    public void ShowDialogueCanvas()
    {
        dialogueCanvas.gameObject.SetActive(true);
        mainDisplayCanvas.gameObject.SetActive(false);
    }

    public void ProcessPlayerAction(string[] inputTextArray)
    {
        string action = inputTextArray[0];
        IPlayerAction playerAction = ActionRegistry.ActionsDict[action];

        if (inputTextArray.Length < playerAction.minInputCount)
        {
            DisplayTextHandler.invokeUpdateTextDisplay(playerAction.tooFewMessage);
        }
        else if (inputTextArray.Length > playerAction.maxInputCount)
        {
            DisplayTextHandler.invokeUpdateTextDisplay(playerAction.tooManyMessage);
        }
        else
        {
            playerAction.Execute(inputTextArray);
        }
    }
}
