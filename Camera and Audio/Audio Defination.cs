using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public PlayAudioEventSO playAudioEvent; // ������Ƶ���¼�
    public AudioClip audioClip;             // �洢��ƵƬ�� 
    public bool playOnEnable;

    private void OnEnable()
    {
        if (playOnEnable)
            PlayAudioClip();
    }

    public void PlayAudioClip()  // ������Ƶ�����¼�
    {
        playAudioEvent.RaiseEvent(audioClip);
    }
}