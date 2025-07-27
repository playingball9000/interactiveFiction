using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AreaUIManager : MonoBehaviour
{

    public QueueManager queueManager;

    public Transform cardContainer; // Parent object to hold card buttons
    public GameObject cardButtonPrefab; // Prefab for each card button

    private void OnEnable()
    {
        GameController.invokeShowExploreCanvas += ShowCurrentArea;
    }


    public void ShowCurrentArea()
    {
        ExploreControl.IsTimeRunning = false;
        // Assuming the player is in an area
        InitializeArea(WorldState.GetInstance().player.currentArea);
    }

    public void OnCardComplete(CardUI completedCard)
    {
        // remove the Card object from card container
        if (completedCard.gameObject != null)
        {
            Destroy(completedCard.gameObject);
        }

        completedCard.cardReference.isComplete = true;
        CardRunRegistry.Get(completedCard.cardReference.internalCode)?.Invoke();

        List<Card> unlockedCards = UnlockCards();

        foreach (Card card in unlockedCards)
        {
            //TODO: This is repeated, so think of a good util
            GameObject cardObj = Instantiate(cardButtonPrefab, cardContainer);
            CardUI cardUI = cardObj.GetComponent<CardUI>();

            cardUI.cardReference = card;

            cardUI.titleText.text = card.title;
            cardUI.progressText.text = $"Ready";

            cardUI.startButton.onClick.AddListener(() => queueManager.EnqueueCard(cardUI));

        }
    }

    public List<Card> UnlockCards()
    {
        // If perf sucks, can create and update these outside this flow
        List<Card> allCards = CardRegistry.GetAllCards();
        List<Card> completedCards = new List<Card>();
        List<Card> lockedCards = new List<Card>();

        foreach (Card card in allCards)
        {
            if (card.isComplete)
            {
                completedCards.Add(card);
            }
            else if (card.isLocked)
            {
                lockedCards.Add(card);
            }
        }

        QueryRunner.RunCardCompleteFacts(completedCards, lockedCards);
        // InitializeArea(WorldState.GetInstance().player.currentArea);
        return lockedCards.Where(card => !card.isLocked).ToList();
    }

    public void InitializeArea(Area currentArea)
    {
        // Clear previous cards
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        List<Card> showableCards = currentArea.cards.Where(card => !card.isLocked && !card.isComplete).ToList();
        // Create ui prefabs for each card
        foreach (Card card in showableCards)
        {
            //TODO: Can I front load the CardUI creation? Might not matter as this is fast
            GameObject cardObj = Instantiate(cardButtonPrefab, cardContainer);
            CardUI cardUI = cardObj.GetComponent<CardUI>();

            cardUI.cardReference = card;

            cardUI.titleText.text = card.title;
            cardUI.progressText.text = $"Ready";

            cardUI.startButton.onClick.AddListener(() => queueManager.EnqueueCard(cardUI));

        }
    }
}
