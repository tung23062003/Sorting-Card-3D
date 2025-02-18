using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//[System.Serializable]
public class CardStack : MonoBehaviour
{
    public List<Card> cards = new();

    public List<Card> SelectTopCards()
    {
        List<Card> selectedCards = new();

        if (cards.Count > 0)
        {
            string topCardColor = cards[^1].cardData.cardColor;

            cards.Reverse();
            foreach (var card in cards)
            {
                if (card.cardData.cardColor == topCardColor)
                {
                    card.SelectCard();
                    selectedCards.Add(card);
                }
                else
                {
                    break;
                }
            }
        }

        return selectedCards;
    }

    public void AddCards(List<Card> newCards)
    {
        foreach (var card in newCards)
        {
            card.transform.SetParent(transform);
            cards.Add(card);
        }
    }

    public void RemoveCards(List<Card> removeCards)
    {
        foreach (var card in removeCards)
        {
            cards.Remove(card);
        }
    }

    //public void SortCards()
    //{
    //    for (int i = 0; i < cards.Count; i++)
    //    {
    //        cards[i].transform.localPosition = Vector3.zero + Vector3.back * i * 0.1f;
    //    }
    //}
}
