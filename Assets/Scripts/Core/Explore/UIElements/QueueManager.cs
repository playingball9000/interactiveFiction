using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class QueueManager : MonoBehaviour
{
    public AreaUIManager areaUIManager;

    public Transform queueContainer; // Vertical Layout Group in the Queue Canvas
    public GameObject queueItemPrefab;

    private Queue<CardQueueUIEntry> cardQueue = new Queue<CardQueueUIEntry>();
    private bool isWorking = false;
    private Coroutine runningTaskCoroutine = null;

    public TaskMessageUI taskMessageUI;

    private void OnEnable()
    {
        GameEvents.OnEnterArea += ClearQueue;
        GameEvents.OnDieInArea += ClearQueue;
    }

    public void EnqueueCard(CardUI cardUI)
    {
        cardUI.startButton.interactable = false;

        GameObject entryObj = Instantiate(queueItemPrefab, queueContainer, false);
        CardQueueUIEntry cardQueueUIEntry = entryObj.GetComponent<CardQueueUIEntry>();

        cardQueueUIEntry.Init(cardUI, OnQueueItemCanceled);

        // I do want to queue up the CardQueueUIEntry because it represents the actual thing in progress
        cardQueue.Enqueue(cardQueueUIEntry);
        OnQueueStateChanged();

        if (!isWorking)
            runningTaskCoroutine = StartCoroutine(ProcessQueue());
    }

    void OnQueueItemCanceled(CardQueueUIEntry queueItem)
    {
        CardQueueUIEntry currentRunningEntry = cardQueue.Peek();
        if (queueItem == currentRunningEntry)
        {
            // Debug.Log("Canceled current running task.");
            StopCoroutine(runningTaskCoroutine);
            cardQueue.Dequeue();
            runningTaskCoroutine = StartCoroutine(ProcessQueue());
        }
        else
        {
            // Queue does not have a remove function so putting in list
            List<CardQueueUIEntry> temp = new List<CardQueueUIEntry>(cardQueue);
            temp.Remove(queueItem);
            cardQueue = new Queue<CardQueueUIEntry>(temp);
        }
        queueItem.originalCardUI.startButton.interactable = true;
        Destroy(queueItem.gameObject);
        OnQueueStateChanged();
    }

    private IEnumerator ProcessQueue()
    {
        isWorking = true;

        while (cardQueue.Count > 0)
        {
            CardQueueUIEntry currentQueueUIEntry = cardQueue.Peek();
            // Log.Debug("PEEK" + currentQueueUIEntry);

            float duration = currentQueueUIEntry.cardData.GetCurrentTimeToComplete();
            float timeLeft = duration;

            while (timeLeft > 0)
            {
                float progress = 1f - (timeLeft / duration);
                currentQueueUIEntry.SetProgress(progress);

                timeLeft -= Time.deltaTime;
                yield return null;
            }
            // Log.Debug("POST COMPLETE CARD" + currentQueueUIEntry.originalCardUI.cardReference);

            currentQueueUIEntry.MarkComplete();
            cardQueue.Dequeue();
            Destroy(currentQueueUIEntry.gameObject);
            taskMessageUI.ShowMessage($" Task '{currentQueueUIEntry.cardData.title}' completed!");

            areaUIManager.OnCardComplete(currentQueueUIEntry.originalCardUI);

            OnQueueStateChanged();
        }

        isWorking = false;
    }

    void OnQueueStateChanged()
    {
        if (cardQueue.Count > 0)
        {
            ExploreControl.IsTimeRunning = true;
        }
        else if (cardQueue.Count == 0)
        {
            ExploreControl.IsTimeRunning = false;
        }
    }

    void ClearQueue()
    {
        isWorking = false;
        cardQueue.Clear();
        runningTaskCoroutine = null;
        UiUtilMb.Instance.DestroyChildrenInContainer(queueContainer);

    }

}
