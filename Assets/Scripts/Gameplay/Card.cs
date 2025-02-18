using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Card : MonoBehaviour
{
    public CardData cardData;
    public Renderer cardImageRenderer;
    public bool isSelected = false;

    public void SelectCard()
    {
        isSelected = true;
        transform.position += Vector3.back * 0.5f;
    }

    public void DeselectCard()
    {
        isSelected = false;
        transform.position -= Vector3.back * 0.5f;
    }

    public void SetCardData(CardData data)
    {
        cardData.cardName = data.cardName;
        cardData.cardColor = data.cardColor;
        cardData.rarity = data.rarity;

        // Load texture
        Texture2D texture = Resources.Load<Texture2D>(data.imagePath);
        if (cardImageRenderer != null)
        {
            Material mat = cardImageRenderer.material;
            mat.mainTexture = texture;
        }
    }
}


