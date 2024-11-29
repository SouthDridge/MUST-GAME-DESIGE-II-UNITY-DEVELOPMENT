using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public PlayAudioEventSO playAudioEvent; // 触发音频的事件
    public AudioClip audioClip;             // 存储音频片段 
    public bool playOnEnable;

    private void OnEnable()
    {
        if (playOnEnable)
            PlayAudioClip();
    }

    public void PlayAudioClip()  // 触发音频播放事件
    {
        playAudioEvent.RaiseEvent(audioClip);
    }
}