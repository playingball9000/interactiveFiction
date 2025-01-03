public class GameController : MonoBehaviour
{

    DialogueParser dialogueParser;

    public Canvas dialogueCanvas;
    public Canvas mainDisplayCanvas;

    public delegate void StartDialogueDelegate();
    public static StartDialogueDelegate invokeStartDialogue;

    public delegate void ShowMainCanvasDelegate();
    public static ShowMainCanvasDelegate invokeShowMainCanvas;

    public delegate void ProcessPlayerActionDelegate(string[] inputTextArray);
    public static ProcessPlayerActionDelegate invokeProcessPlayerAction;

    private void OnEnable()
    {
        GameController.invokeStartDialogue += StartDialogue;
        GameController.invokeShowMainCanvas += ShowMainCanvas;
        GameController.invokeProcessPlayerAction += ProcessPlayerAction;
    }

    private void OnDisable()
    {
        GameController.invokeStartDialogue -= StartDialogue;
        GameController.invokeShowMainCanvas -= ShowMainCanvas;
        GameController.invokeProcessPlayerAction -= ProcessPlayerAction;
    }

    void Start()
    {
        dialogueParser = GetComponent<DialogueParser>();

        ShowMainCanvas();
        //StartDialogue();
    }

    public void StartDialogue()
    {
        dialogueParser.DisplayDialogue("node1");

        dialogueCanvas.gameObject.SetActive(true);
        mainDisplayCanvas.gameObject.SetActive(false);
    }

    public void ShowMainCanvas()
    {
        dialogueCanvas.gameObject.SetActive(false);
        mainDisplayCanvas.gameObject.SetActive(true);
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
