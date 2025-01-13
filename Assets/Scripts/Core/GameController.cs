using UnityEngine;

public class GameController : MonoBehaviour
{
    public Canvas dialogueCanvas;
    public Canvas mainDisplayCanvas;

    public delegate void ShowDialogueCanvasDelegate();
    public static ShowDialogueCanvasDelegate invokeShowDialogueCanvas;

    public delegate void ShowMainCanvasDelegate();
    public static ShowMainCanvasDelegate invokeShowMainCanvas;

    private void OnEnable()
    {
        GameController.invokeShowDialogueCanvas += ShowDialogueCanvas;
        GameController.invokeShowMainCanvas += ShowMainCanvas;
    }

    private void OnDisable()
    {
        GameController.invokeShowDialogueCanvas -= ShowDialogueCanvas;
        GameController.invokeShowMainCanvas -= ShowMainCanvas;
    }

    void Start()
    {
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

}
