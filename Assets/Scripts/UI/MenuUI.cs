using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button packBtn;

    public PlayableDirector packDirector;
    [SerializeField] private TimelineAsset[] timelineAsset;
    [SerializeField] private SignalReceiver receiver;
    [SerializeField] private SignalAsset signalAsset;

    [SerializeField] private OpenPackUI openPackUI;

    public UnityEvent onEndPackTimeline;

    public Vector3 positionOffset;
    public Animator animator;

    private void OnTimelineFinished(PlayableDirector director)
    {
        if (director == packDirector)
        {
            Debug.Log("Timeline has finished!");
            OnEndPackTimeLine();
        }
    }

    private void OnTimelinePlaying(PlayableDirector director)
    {
        if (director.state == PlayState.Playing)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnEndPackTimeLine()
    {
        Debug.Log("End Pack Timeline...");
        openPackUI.gameObject.SetActive(true);
        openPackUI.openBtn.gameObject.SetActive(true);
        //if(receiver.GetReaction(signalAsset) != null)
        //    receiver.Remove(signalAsset);

        //if (!packDirector.playableGraph.IsValid())
        //    packDirector.RebuildGraph();
    }

    private void Awake()
    {
        playBtn.onClick.AddListener(HandlePlayBtn);
        packBtn.onClick.AddListener(HandlePackBtn);
        //packDirector.stopped += OnTimelineFinished;
        packDirector.played += OnTimelinePlaying;
        onEndPackTimeline.AddListener(OnEndPackTimeLine);

        
        receiver.AddReaction(signalAsset, onEndPackTimeline);
    }


    private void OnDestroy()
    {
        playBtn.onClick.RemoveAllListeners();
        packBtn.onClick.RemoveAllListeners();
        //packDirector.stopped -= OnTimelineFinished;
        packDirector.played -= OnTimelinePlaying;
        onEndPackTimeline.RemoveAllListeners();
    }

    private void HandlePlayBtn()
    {
        SceneController.Instance.LoadScene(GameConstants.mainScene);
    }

    public void HandlePackBtn()
    {

        //if (packDirector.state == PlayState.Playing)
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
        //packDirector.playableAsset = timelineAsset;
        
        packDirector.Play();
    }
}
