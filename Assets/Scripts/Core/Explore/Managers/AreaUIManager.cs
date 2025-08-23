using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class AreaUIManager : MonoBehaviour
{

    public QueueManager queueManager; // Injected from Unity UI

    public Transform cardContainer; // Parent object to hold card buttons
    public GameObject cardButtonPrefab; // Prefab for each card button

    public TaskMessageUI taskMessageUI;
    public Transform areaLogContainer;
    public ScrollRect logScrollRect;
    public GameObject logPrefab;


    private void OnEnable()
    {
        EventManager.Subscribe(GameEvent.EnterArea, ShowCurrentArea);
        EventManager.Subscribe(GameEvent.DieInArea, ResetAreas);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvent.EnterArea, ShowCurrentArea);
        EventManager.Unsubscribe(GameEvent.DieInArea, ResetAreas);
    }

    public void ShowCurrentArea()
    {
        ExploreControl.IsTimeRunning = false;
        // Assuming the player is in an area
        Area currentArea = PlayerContext.Get.currentArea;

        UiUtilMb.Instance.DestroyChildrenInContainer(cardContainer);

        List<Card> showableCards = currentArea.cards.Where(card => !card.isLocked && !card.isComplete).ToList();
        // Create ui prefabs for each showable card
        foreach (Card card in showableCards)
        {
            CreateCardUI(card);
        }
    }

    public void OnCardComplete(CardUI completedCard)
    {
        completedCard.cardRef.MarkCompleted();
        completedCard.cardRef.RecalculateCurrentTimeToComplete();

        if (completedCard.cardRef.isComplete)
        {
            // remove the Card object from card container only if it is complete. ie. repeatable cards are not marked complete
            Destroy(completedCard.gameObject);
        }
        else
        {
            completedCard.startButton.interactable = true;
        }

        // Debug.Log(completedCard.cardReference);
        // Run associated code if it exists
        CardRunRegistry.Get(completedCard.cardRef.internalCode)?.Invoke();

        taskMessageUI.ShowMessage($" Task '{completedCard.cardRef.title}' completed!");

        GameObject newTextObj = Instantiate(logPrefab, areaLogContainer);
        TextMeshProUGUI tmp = newTextObj.GetComponent<TextMeshProUGUI>();
        tmp.text = completedCard.cardRef.completionLog;
        UiUtilMb.Instance.ScrollToBottom(logScrollRect);


        List<Card> unlockedCards = CardUtil.UnlockCardsPostComplete();
        List<Card> cardsInArea = PlayerContext.Get.currentArea.cards;

        // Only create new cards for unlocked cards in the area
        foreach (var card in unlockedCards.Intersect(cardsInArea, new CardCodeComparer()))
        {
            CreateCardUI(card);
        }
    }

    public CardUI CreateCardUI(Card card)
    {
        GameObject cardObj = Instantiate(cardButtonPrefab, cardContainer);
        CardUI cardUI = cardObj.GetComponent<CardUI>();
        cardUI.Init(card, queueManager.EnqueueCard);

        return cardUI;
    }

    public void ResetAreas()
    {
        ExploreControl.IsTimeRunning = false;
        UiUtilMb.Instance.DestroyChildrenInContainer(cardContainer);
        UiUtilMb.Instance.DestroyChildrenInContainer(areaLogContainer);

        CardRegistry.GetAllCards().ForEach(c => c.ResetCard());
    }
}
