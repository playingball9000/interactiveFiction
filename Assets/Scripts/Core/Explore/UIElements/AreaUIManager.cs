using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//TODO: This class contains too much, need to splitit up

public class AreaUIManager : MonoBehaviour
{
    public Transform queueContainer; // Vertical Layout Group in the Queue Canvas
    public GameObject queueItemPrefab;

    private Queue<CardQueueUIEntry> cardQueue = new Queue<CardQueueUIEntry>();
    private bool isWorking = false;

    private Coroutine runningTaskCoroutine = null;


    public TaskMessageUI taskMessageUI;

    // private Queue<UICard> cardQueue = new Queue<UICard>();
    // private bool isWorking = false;

    public Transform cardContainer; // Parent object to hold card buttons
    public GameObject cardButtonPrefab; // Prefab for each card button

    private Area currentArea;


    private void OnEnable()
    {
        LoggingUtil.Log("AreaUIManager Enable");

        // Create example area and cards
        currentArea = new Area("Castle");

        currentArea.AddCard(new Card("Find the key", 10f));
        currentArea.AddCard(new Card("Unlock the door", 5f));
        currentArea.AddCard(new Card("Defeat the guard", 15f));
        GameController.invokeShowExploreCanvas += ShowCurrentArea;
    }



    public void ShowCurrentArea()
    {
        ShowArea(currentArea);
    }

    public void ShowArea(Area area)
    {
        currentArea = area;

        // Clear previous cards
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for each card
        foreach (Card card in area.cards)
        {
            GameObject cardObj = Instantiate(cardButtonPrefab, cardContainer);
            CardUI cardUI = cardObj.GetComponent<CardUI>();

            cardUI.cardReference = card;

            // LoggingUtil.Log(card.title);
            cardUI.titleText.text = card.title;
            cardUI.progressText.text = $"Ready";

            // Capture for closure
            Card selectedCard = card;

            // Set button callback
            // cardUI.startButton.onClick.AddListener(() => StartCoroutine(StartCardTimer(selectedCard, cardUI)));
            cardUI.startButton.onClick.AddListener(() => EnqueueCard(selectedCard, cardUI));


            // // Button btn = buttonObj.GetComponent<Button>();
            // // Text btnText = buttonObj.GetComponentInChildren<Text>();
            // // LoggingUtil.Log(card.title);
            // // LoggingUtil.Log(btnText);// <---this is null

            // // btnText.text = card.title;

            // // Capture card in a local variable for closure
            // Card selectedCard = card;
            // btn.onClick.AddListener(() => StartCoroutine(StartCardTimer(selectedCard, buttonObj)));
        }
    }

    void EnqueueCard(Card card, CardUI cardUI)
    {
        cardUI.startButton.interactable = false;

        GameObject entryObj = Instantiate(queueItemPrefab, queueContainer, false);
        CardQueueUIEntry entryUI = entryObj.GetComponent<CardQueueUIEntry>();


        // fix this up later
        entryUI.originalCardUI = cardUI;
        entryUI.Init(card, OnQueueItemCanceled);

        //TODO: I should probably make a wrapper around queue for this state change
        cardQueue.Enqueue(entryUI);
        OnQueueStateChanged();

        if (!isWorking)
            runningTaskCoroutine = StartCoroutine(ProcessQueue());
    }

    void OnQueueItemCanceled(CardQueueUIEntry entry)
    {
        // If it’s the currently running one, skip removal
        // if (cardQueue.Count > 0 && cardQueue.Peek() == entry && isWorking)
        // {
        //     Debug.Log("Can't cancel running task.");
        //     return;
        // }
        CardQueueUIEntry currentRunningEntry = cardQueue.Peek();
        if (entry == currentRunningEntry)
        {
            Debug.Log("Canceled current running task.");

            StopCoroutine(runningTaskCoroutine);
            Destroy(currentRunningEntry.gameObject);

            cardQueue.Dequeue();
            runningTaskCoroutine = StartCoroutine(ProcessQueue());

            OnQueueStateChanged();
            entry.originalCardUI.startButton.interactable = true;
            return;
        }
        // Remove it from the queue
        List<CardQueueUIEntry> temp = new List<CardQueueUIEntry>(cardQueue);
        temp.Remove(entry);

        cardQueue = new Queue<CardQueueUIEntry>(temp);

        entry.originalCardUI.startButton.interactable = true;
        Destroy(entry.gameObject);
        OnQueueStateChanged();
    }

    private IEnumerator ProcessQueue()
    {
        isWorking = true;

        while (cardQueue.Count > 0)
        {
            CardQueueUIEntry current = cardQueue.Peek();

            float duration = current.cardData.timeToComplete;
            float timeLeft = duration;

            while (timeLeft > 0)
            {
                float progress = 1f - (timeLeft / duration);
                current.SetProgress(progress);

                timeLeft -= Time.deltaTime;
                yield return null;
            }

            current.MarkComplete();
            if (current.originalCardUI.gameObject != null)
            {
                Destroy(current.originalCardUI.gameObject);
            }
            cardQueue.Dequeue();
            Destroy(current.gameObject); // or keep it for history
            taskMessageUI.ShowMessage($" Task '{current.cardData.title}' completed!");

            OnQueueStateChanged();
        }

        isWorking = false;
    }

    void OnQueueStateChanged()
    {
        LoggingUtil.Log("OnQueueStateChanged - cardQueue=" + cardQueue.Count);
        if (cardQueue.Count > 0)
        {
            ExploreControl.IsTimeRunning = true;
            // isWorking = true;
        }
        else if (cardQueue.Count == 0)
        {
            ExploreControl.IsTimeRunning = false;
            // isWorking = false;
        }
    }


    private IEnumerator StartCardTimer(Card card, CardUI buttonObj)
    {
        buttonObj.startButton.interactable = false;
        // Button btn = buttonObj.GetComponent<Button>();
        // btn.interactable = false;

        float timeRemaining = card.timeToComplete;

        // Text btnText = buttonObj.GetComponentInChildren<Text>();

        while (timeRemaining > 0)
        {
            buttonObj.progressText.text = $"{card.title} ({timeRemaining:F1}s)";
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        buttonObj.titleText.text = $"{card.title} (Completed!)";
    }
}
