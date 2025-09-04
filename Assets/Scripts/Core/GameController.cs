using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
explrot canvas -> panel
tooltip look better
explore log look better
explore dialgoue box
*/

public class GameController : MonoBehaviour
{
    public Canvas dialogueCanvas;
    public Canvas mainDisplayCanvas;
    public Canvas exploreCanvas;

    public Canvas blackScreen;
    public Canvas rpgTextBoxCanvas;
    public TextMeshProUGUI introTextBox;

    float fadeDuration = 0.3f;

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

        if (WorldState.GetInstance().FLAG_showIntro)
        {
            blackScreen.gameObject.SetActive(true);
            rpgTextBoxCanvas.gameObject.SetActive(false);
            mainDisplayCanvas.gameObject.SetActive(false);
            dialogueCanvas.gameObject.SetActive(false);
            exploreCanvas.gameObject.SetActive(false);

            StartCoroutine(IntroSequence());
        }
        else
        {
            invokeShowMainCanvas();
            // PlayerContext.Get.currentLocation = AreaRegistry.GetArea(LocationCode.Abyss_a);
            // invokeShowExploreCanvas();
        }
    }

    IEnumerator IntroSequence()
    {
        var paragraphs = Intro.introParagraphs;
        rpgTextBoxCanvas.gameObject.SetActive(true);
        CanvasGroup rpgTextBoxCanvasCG = rpgTextBoxCanvas.GetComponent<CanvasGroup>();
        rpgTextBoxCanvasCG.alpha = 0;
        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(UiUtilMb.Instance.FadeHelper(rpgTextBoxCanvasCG, 0f, 1f, fadeDuration));

        for (int i = 0; i < paragraphs.Count; i++)
        {
            introTextBox.text = "";
            yield return StartCoroutine(UiUtilMb.Instance.TypewriterAppend(paragraphs[i], introTextBox));
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return new WaitForSeconds(0.05f);

        }
        rpgTextBoxCanvas.gameObject.SetActive(false);

        invokeShowMainCanvas();

        StoryTextHandler.invokeUpdateStoryDisplay(Intro.introStory, TextEffect.Typewriter);
    }

    public void ShowMainCanvas()
    {
        StartCoroutine(UiUtilMb.Instance.FadeHelper(blackScreen.GetComponent<CanvasGroup>(), 1f, 0f, fadeDuration));
        WorldState.GetInstance().FLAG_dialogWindowActive = false;
        dialogueCanvas.gameObject.SetActive(false);
        mainDisplayCanvas.gameObject.SetActive(true);
        exploreCanvas.gameObject.SetActive(false);
        rpgTextBoxCanvas.gameObject.SetActive(false);
    }

    public void ShowDialogueCanvas()
    {
        WorldState.GetInstance().FLAG_dialogWindowActive = true;
        dialogueCanvas.gameObject.SetActive(true);
        mainDisplayCanvas.gameObject.SetActive(false);
        exploreCanvas.gameObject.SetActive(false);
        rpgTextBoxCanvas.gameObject.SetActive(false);
    }


    public void ShowExploreCanvas()
    {
        StartCoroutine(UiUtilMb.Instance.FadeHelper(blackScreen.GetComponent<CanvasGroup>(), 1f, 0f, fadeDuration));
        WorldState.GetInstance().FLAG_dialogWindowActive = false;

        dialogueCanvas.gameObject.SetActive(false);
        mainDisplayCanvas.gameObject.SetActive(false);
        exploreCanvas.gameObject.SetActive(true);
        rpgTextBoxCanvas.gameObject.SetActive(false);

        // Need to raise event after canvas has been activated to make sure all UI elements have subscribed
        EventManager.Raise(GameEvent.EnterArea);
    }

    public void ResetPlayerOnDeath()
    {
        Debug.Log("Player Reset on Death");
        PlayerContext.Get.currentLocation = RoomRegistry.GetRoom(LocationCode.StartingCamp_r);
        // Always call the delegate to do all the related stuff
        invokeShowMainCanvas();
        StoryTextHandler.invokeUpdateStoryDisplay("\nYou died... but it is not the end.\n", TextEffect.Typewriter);

    }

}
