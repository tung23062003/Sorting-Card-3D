using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CardStackClickHandler : MonoBehaviour
{
    private Camera mainCamera;
    public List<Card> selectedCards = new();
    private CardStack activeStack = null;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance.isMoving)
            return;
#if UNITY_EDITOR || UNITY_STANDALON
        if (Input.GetMouseButtonDown(0))
        {
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
#endif
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.CompareTag(GameConstants.cardStack))
                {
                    Debug.Log($"Clicked on Card: {clickedObject.name}");
                    CardStack clickedStack = clickedObject.GetComponent<CardStack>();
                    HandleStackClick(clickedStack);
                }
                else if (clickedObject.CompareTag(GameConstants.card))
                {
                    Debug.Log($"Clicked on Card: {clickedObject.name}");
                }
            }
        }
#if UNITY_EDITOR || UNITY_STANDALON

#elif UNITY_ANDROID
        }
#endif
    }

    private void HandleStackClick(CardStack clickedStack)
    {
        if (activeStack == null)
        {
            activeStack = clickedStack;
            selectedCards = clickedStack.SelectTopCards();
        }
        else if (activeStack == clickedStack)
        {
            DeselectCards();
        }
        else
        {
            MoveCards(activeStack, clickedStack);
            activeStack.cards.Reverse();
            activeStack = null;
        }
    }

    private void MoveCards(CardStack fromStack, CardStack toStack)
    {
        GameManager.Instance.isMoving = true;

        Vector3 targetPosition = toStack.cards.Count > 0
            ? toStack.cards[^1].transform.position + Vector3.back * 0.1f
            : toStack.transform.position;

        fromStack.RemoveCards(selectedCards);


        toStack.AddCards(selectedCards);

        Sequence moveSequence = DOTween.Sequence();

        for (int i = 0; i < selectedCards.Count; i++)
        {
            Card card = selectedCards[i];

            if (i > 0)
            {
                targetPosition += Vector3.back * 0.1f;
            }

            moveSequence.Append(card.transform.DOMove(targetPosition, 0.4f).SetEase(Ease.InOutCirc));
        }

        moveSequence.OnComplete(() =>
        {
            //toStack.SortCards();

            selectedCards.Clear();
            GameManager.Instance.CheckAndClearStacks();
            GameManager.Instance.isMoving = false;
        });
    }


    private void DeselectCards()
    {
        foreach (var card in selectedCards)
        {
            card.DeselectCard();
        }
        if(selectedCards.Count > 0)
            selectedCards[0].transform.parent.GetComponent<CardStack>().cards.Reverse();

        selectedCards.Clear();
        activeStack = null;
    }
}
