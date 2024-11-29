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
        // 找到带有Bounds标签的物体
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;
        // 获取Bounds标签的物体所使用的Collider2D类的组件
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        // 场景切换时清除之前场景边框的缓存
        confiner2D.InvalidateCache();
    }

}
