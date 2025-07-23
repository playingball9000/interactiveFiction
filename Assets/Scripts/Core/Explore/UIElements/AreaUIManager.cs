using UnityEngine;
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

    public Transform cardContainer; // Parent object to hold card buttons
    public GameObject cardButtonPrefab; // Prefab for each card button

    private Area currentArea;


    private void OnEnable()
    {
        LoggingUtil.Log("AreaUIManager Enable");

        // Create example area and cards
        GameController.invokeShowExploreCanvas += ShowCurrentArea;
    }



    public void ShowCurrentArea()
    {
        ExploreControl.IsTimeRunning = false;
        // Assuming the player is in an area
        ShowArea(WorldState.GetInstance().player.currentArea);
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
            cardUI.startButton.onClick.AddListener(() => EnqueueCard(selectedCard, cardUI));

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
        }
        else if (cardQueue.Count == 0)
        {
            ExploreControl.IsTimeRunning = false;
        }
    }

}
