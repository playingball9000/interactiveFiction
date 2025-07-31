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
            CardQueueUIEntry currentQueueUIEntry = cardQueue.Peek();
            // Log.Debug("PEEK" + currentQueueUIEntry);

            float duration = currentQueueUIEntry.cardData.timeToComplete;
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
            // Removes entry from queue container
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

}
