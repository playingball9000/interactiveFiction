using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QueueManager : MonoBehaviour
{
    public AreaUIManager areaUIManager;

    public Transform queueContainer; // Vertical Layout Group in the Queue Canvas
    public GameObject queueItemPrefab;

    private Queue<CardQueueUIEntry> cardQueue = new Queue<CardQueueUIEntry>();
    private bool isWorking = false;


    private CardQueueUIEntry currentQueueUIEntry;
    private float taskDuration;
    private float taskTimeLeft;


    private void OnEnable()
    {
        EventManager.Subscribe(GameEvent.DieInArea, ClearQueue);
        TickSystem.OnTick += OnTick;
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvent.DieInArea, ClearQueue);
        TickSystem.OnTick -= OnTick;
    }

    public void EnqueueCard(CardUI cardUI)
    {
        cardUI.startButton.interactable = false;

        GameObject entryObj = Instantiate(queueItemPrefab, queueContainer, false);
        CardQueueUIEntry cardQueueUIEntry = entryObj.GetComponent<CardQueueUIEntry>();
        cardQueueUIEntry.Init(cardUI, OnQueueItemCanceled);

        cardQueue.Enqueue(cardQueueUIEntry);
        OnQueueStateChanged();

        if (!isWorking)
            StartNextTask();
    }

    private void OnTick()
    {
        if (!isWorking || currentQueueUIEntry == null)
            return;

        taskTimeLeft -= .02f; // decrease by tick length
        float progress = 1f - (taskTimeLeft / taskDuration);
        currentQueueUIEntry.SetProgress(progress);

        if (taskTimeLeft <= 0f)
        {
            CompleteTask();
        }
    }

    private void StartNextTask()
    {
        if (cardQueue.Count == 0)
        {
            isWorking = false;
            return;
        }

        isWorking = true;
        currentQueueUIEntry = cardQueue.Peek();

        taskDuration = currentQueueUIEntry.cardUIRef.cardRef.GetCurrentTimeToComplete();
        taskTimeLeft = taskDuration - currentQueueUIEntry.cardUIRef.elapsedTime;

        currentQueueUIEntry.SetProgress(0f);
    }

    private void CompleteTask()
    {
        cardQueue.Dequeue();
        Destroy(currentQueueUIEntry.gameObject);

        areaUIManager.OnCardComplete(currentQueueUIEntry.cardUIRef);
        OnQueueStateChanged();

        currentQueueUIEntry = null;
        StartNextTask();
    }

    void OnQueueItemCanceled(CardQueueUIEntry queueItem)
    {
        // Reset button + remove UI
        queueItem.cardUIRef.startButton.interactable = true;
        Destroy(queueItem.gameObject);

        if (currentQueueUIEntry == queueItem)
        {
            cardQueue.Dequeue();
            currentQueueUIEntry = null;
            isWorking = false;
            queueItem.cardUIRef.elapsedTime = taskDuration - taskTimeLeft;

            StartNextTask();
        }
        else
        {
            // Cancel a queued (not yet running) task
            List<CardQueueUIEntry> temp = new List<CardQueueUIEntry>(cardQueue);
            temp.Remove(queueItem);
            cardQueue = new Queue<CardQueueUIEntry>(temp);
        }

        OnQueueStateChanged();
    }

    void OnQueueStateChanged()
    {
        if (cardQueue.Count > 0)
        {
            ExploreControl.IsTimeRunning = true;
        }
        else
        {
            ExploreControl.IsTimeRunning = false;
        }
    }

    void ClearQueue()
    {
        isWorking = false;
        currentQueueUIEntry = null;
        cardQueue.Clear();

        UiUtilMb.Instance.DestroyChildrenInContainer(queueContainer);

        OnQueueStateChanged();
    }

}
