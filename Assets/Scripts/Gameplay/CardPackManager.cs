using System.Collections.Generic;
using UnityEngine;

public class CardPackManager : Singleton<CardPackManager>
{
    private List<CardData> allCards;
    public List<PackRarity> commonPackProbabilities;
    public List<PackRarity> rarePackProbabilities;
    public List<PackRarity> epicPackProbabilities;

    [SerializeField] private GameCardDataSO gameCard;
    [SerializeField] private PlayerCardDataSO playerCard;

    private void Start()
    {
        allCards = gameCard.gameCardList;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenPack(GameConstants.Common, 5);
        }

    }

    public List<CardData> OpenPack(string packName, int cartQuantity)
    {
        var openedCards = GetPackCards(packName, cartQuantity);

        playerCard.AddPlayerCards(OwnedCard(openedCards));

        foreach (var card in openedCards)
        {
            Debug.Log($"Card: {card.cardName} || Rare: {card.rarity}");
        }

        return openedCards;
    }

    public List<CardData> OwnedCard(List<CardData> cards)
    {
        if (playerCard.playerCardList == null || playerCard.playerCardList.Count == 0)
            return cards;
        List<CardData> unownedCards = new();
        foreach (var card in cards)
        {
            if (!playerCard.playerCardList.Contains(card))
                unownedCards.Add(card);
        }
        return unownedCards;
    }

    public List<CardData> GetPackCards(string packType, int cardCount)
    {
        List<PackRarity> probabilities = packType switch
        {
            "Common" => commonPackProbabilities,
            "Rare" => rarePackProbabilities,
            "Epic" => epicPackProbabilities,
            _ => commonPackProbabilities
        };

        List<CardData> openedCards = new();

        for (int i = 0; i < cardCount; i++)
        {
            string selectedRarity = GetRandomRarity(probabilities);

            var filteredCards = allCards.FindAll(card => card.rarity == selectedRarity && !openedCards.Contains(card));

            if (filteredCards.Count > 0)
            {
                int randomIndex = Random.Range(0, filteredCards.Count);
                openedCards.Add(filteredCards[randomIndex]);
            }
        }

        return openedCards;
    }

    private string GetRandomRarity(List<PackRarity> probabilities)
    {
        
        float randomValue = Random.Range(0f, 100f);

        float cumulativeProbability = 0;

        foreach (var packRarity in probabilities)
        {
            cumulativeProbability += packRarity.probability;
            if (randomValue <= cumulativeProbability)
            {
                return packRarity.rarity;
            }
        }

        return GameConstants.Common;
    }

    public CardRarityCanOpen CheckCardCanOpen()
    {
        if (playerCard.cardQuantity.rareQuantity >= Config.rarePackRequired && playerCard.cardQuantity.epicQuantity < Config.epicPackRequired)
        {
            return CardRarityCanOpen.Rare;
        }
        else if (playerCard.cardQuantity.epicQuantity >= Config.epicPackRequired && playerCard.cardQuantity.legendQuantity < Config.legendPackRequired)
        {
            return CardRarityCanOpen.Epic;
        }
        else if(playerCard.cardQuantity.legendQuantity >= Config.legendPackRequired)
        {
            return CardRarityCanOpen.Legendary;
        }

        return CardRarityCanOpen.Common;
    }
}
