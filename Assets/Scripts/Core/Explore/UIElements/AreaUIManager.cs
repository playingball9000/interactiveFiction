using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AreaUIManager : MonoBehaviour
{

    public QueueManager queueManager; // Injected from Unity UI

    public Transform cardContainer; // Parent object to hold card buttons
    public GameObject cardButtonPrefab; // Prefab for each card button

    private void OnEnable()
    {
        GameEvents.OnEnterArea += ShowCurrentArea;
        GameEvents.OnDieInArea += ResetCards;

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
        // remove the Card object from card container
        Destroy(completedCard.gameObject);

        completedCard.cardReference.MarkCompleted();
        // Debug.Log(completedCard.cardReference);
        // Run associated code if it exists
        CardRunRegistry.Get(completedCard.cardReference.internalCode)?.Invoke();

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

    public void ResetCards()
    {
        UiUtilMb.Instance.DestroyChildrenInContainer(cardContainer);

        CardRegistry.GetAllCards().ForEach(c => c.ResetCard());
    }
}
