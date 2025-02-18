using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public GameObject cardPrefab;
    public CardStack[] cardStacks;

    [SerializeField] private PlayerCardDataSO playerCards;
    [SerializeField] private PlayerInfoDataSO playerInfo;

    [SerializeField] private int colorPerLevel;
    public bool isMoving;

    private void Start()
    {
        var colorList = RandomColor(colorPerLevel);
        foreach (var cardStack in cardStacks)
        {
            CreateStack(cardStack.transform, Random.Range(5, 10), colorList);
        }
    }

    

    private void CreateStack(Transform stackPosition, int cardCount, Dictionary<string, CardData> colorList)
    {
        CardStack cardStack = stackPosition.GetComponent<CardStack>();
        if (cardStack == null) return;

        List<Card> cards = new();

        

        for (int i = 0; i < cardCount; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, stackPosition);
            cardObject.transform.localPosition = Vector3.zero + 0.1f * i * Vector3.back;
            Card card = cardObject.GetComponent<Card>();

            

            var cardData = playerCards.GetPlayerCardByName(GetRandomPlayerCard(colorList).cardName);
            card.SetCardData(cardData);
            cards.Add(card);

            Renderer renderer = cardObject.GetComponent<Renderer>();
            renderer.material.color = GetColor(card.cardData.cardColor);

        }

        cardStack.cards = cards;
    }

    private Dictionary<string, int> CalculateTotalCardsPerColor()
    {
        Dictionary<string, int> colorCounts = new();

        foreach (var stack in cardStacks)
        {
            foreach (var card in stack.cards)
            {
                if (colorCounts.ContainsKey(card.cardData.cardColor))
                {
                    colorCounts[card.cardData.cardColor]++;
                }
                else
                {
                    colorCounts[card.cardData.cardColor] = 1;
                }
            }
        }

        return colorCounts;
    }

    public void CheckAndClearStacks()
    {
        Dictionary<string, int> totalCardsPerColor = CalculateTotalCardsPerColor();

        foreach (var color in totalCardsPerColor.Keys)
        {
            int totalCount = totalCardsPerColor[color];

            foreach (var stack in cardStacks)
            {
                List<Card> matchingCards = new();

                foreach (var card in stack.cards)
                {
                    if (card.cardData.cardColor == color)
                    {
                        matchingCards.Add(card);
                    }
                    else if (matchingCards.Count > 0)
                    {
                        break;
                    }
                }

                if (matchingCards.Count == totalCount)
                {
                    ClearCards(matchingCards);
                    return;
                }
            }
        }
    }

    private void ClearCards(List<Card> cardsToClear)
    {
        int coins = cardsToClear.Count;

        foreach (var card in cardsToClear)
        {
            card.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                Destroy(card.gameObject);
            });
        }

        foreach (var stack in cardStacks)
        {
            stack.cards.RemoveAll(card => cardsToClear.Contains(card));
        }

        CheckWinLevel();
        GameEvent.OnCardScore?.Invoke(coins);
        Debug.Log($"Get {coins} coins!");
    }

    private void CheckWinLevel()
    {
        bool allStacksEmpty = true;

        foreach (var stack in cardStacks)
        {
            if (stack.cards.Count > 0)
            {
                allStacksEmpty = false;
                break;
            }
        }

        if (allStacksEmpty)
        {
            GameEvent.OnWinLevel?.Invoke();
            playerInfo.NextLevel();
        }
    }


    public CardData GetRandomPlayerCard(Dictionary<string, CardData> colorList)
    {
        var cardValues = new List<CardData>(colorList.Values);
        int num = Random.Range(0, cardValues.Count);
        return cardValues[num];
    }

    private Dictionary<string, CardData> RandomColor(int colorPerLevel)
    {
        var cards = playerCards.playerCardList;
        
        Dictionary<string, CardData> cardColors = new();
        while (cardColors.Count < colorPerLevel)
        {
            int num = Random.Range(0, cards.Count);
            if (!cardColors.ContainsKey(cards[num].cardColor))
            {
                var test = cards.FindAll(item => item.cardColor == cards[num].cardColor);
                if (test == null || test.Count == 0)
                    return null;
                int num2 = Random.Range(0, test.Count);
                cardColors.Add(cards[num].cardColor, test[num2]);
            }
        }
        return cardColors;
    }

    public static Color GetColor(string cardColor)
    {
        var color = cardColor switch
        {
            "Red" => Color.red,
            "Green" => Color.green,
            "Blue" => Color.blue,
            "Yellow" => Color.yellow,
            _ => Color.red,
        };
        return color;
    }
}


