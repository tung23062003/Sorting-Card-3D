using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class OpenPackUI : MonoBehaviour
{
    public Button openBtn;
    [SerializeField] private Transform card2DPrefab;
    [SerializeField] private Transform parent;

    [SerializeField] private Button backBtn;
    [SerializeField] private PlayableDirector packDirector;
    [SerializeField] private TimelineAsset[] timelineAsset;
    [SerializeField] private SignalReceiver receiver;
    [SerializeField] private SignalAsset signalAsset;

    [SerializeField] private MenuUI menuUI;

    [SerializeField] private DesignDataSO designData;
    [SerializeField] private PlayerCardDataSO playerCard;
    public UnityEvent onEndBackMenuTimeline;

    private void Awake()
    {
        openBtn.onClick.AddListener(HandleOpenPack);
        backBtn.onClick.AddListener(HandleBackBtn);
        onEndBackMenuTimeline.AddListener(OnEndBackMenuTimeline);
        receiver.AddReaction(signalAsset, onEndBackMenuTimeline);
    }


    private void OnDestroy()
    {
        openBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
    }

    private void HandleOpenPack()
    {
        var cardsOpened = CardPackManager.Instance.OpenPack(GameConstants.Common, 5);
        foreach (var cardOpen in cardsOpened)
        {
            Transform card = Instantiate(card2DPrefab, parent);
            card.GetComponent<Image>().color = GameManager.GetColor(cardOpen.cardColor);
            card.GetChild(0).GetComponent<Image>().sprite = designData.GetCardVisual(cardOpen.cardName).sprite;
        }
        openBtn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(true);
    }

    private void HandleBackBtn()
    {
        //ResetTimeline();
        backBtn.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
        //if(packDirector.state == PlayState.Playing)
        //    packDirector.Stop();


        var cardCanOpen = CardPackManager.Instance.CheckCardCanOpen();
        packDirector.playableAsset = cardCanOpen switch
        {
            CardRarityCanOpen.Common => timelineAsset[0],
            CardRarityCanOpen.Rare => timelineAsset[1],
            CardRarityCanOpen.Epic => timelineAsset[2],
            //CardRarityCanOpen.Legendary => timelineAsset[3],
            _ => throw new NotImplementedException(),
        };
        packDirector.time = 0;
        packDirector.Play();
    }

    public void OnEndBackMenuTimeline()
    {
        Debug.Log("End Back Menu Timeline...");
        menuUI.gameObject.SetActive(true);
        //packDirector.Stop();
        //if (receiver.GetReaction(signalAsset) != null)
        //    receiver.Remove(signalAsset);


        playerCard.SetCardsQuanityPerRarity();
    }

    private void ResetTimeline()
    {
        packDirector.Stop();
        packDirector.time = 0;
        packDirector.Evaluate();
    }
}
