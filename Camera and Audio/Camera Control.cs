using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSO cameraShakeEvent;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void Start()
    {
        GetNewCameraBounds();
    }

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    private void GetNewCameraBounds()
    {
        // �ҵ�����Bounds��ǩ������
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;
        // ��ȡBounds��ǩ��������ʹ�õ�Collider2D������
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        // �����л�ʱ���֮ǰ�����߿�Ļ���
        confiner2D.InvalidateCache();
    }

}
