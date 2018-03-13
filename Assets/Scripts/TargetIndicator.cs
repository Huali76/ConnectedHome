﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform m_icon;
    private Image m_iconImage;
    private Canvas mainCanvas;

    private Vector3 m_cameraOffsetUp;
    private Vector3 m_cameraOffsetRight;
    private Vector3 m_cameraOffsetForward;


    public Sprite m_targetIconOnScreen;
    public Sprite m_targetIconOffScreen;

    [Range(0, 100)]
    public float m_edgeBuffer;
    public Vector3 m_targetIconScale;

    public bool m_showOffScreen;

    void Start ()
    {
        mainCamera = Camera.main;
        mainCanvas = FindObjectOfType<Canvas>();
        InstainateTargetIcon();
	}
	
	void Update ()
    {
        Vector3 directionFromCamera = transform.position - mainCamera.transform.position;

        Vector3 cameraForwad = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraUp = mainCamera.transform.up;
        
        cameraForwad *= Vector3.Dot(cameraForwad, directionFromCamera);
        cameraRight *= Vector3.Dot(cameraRight, directionFromCamera);
        cameraUp *= Vector3.Dot(cameraUp, directionFromCamera);

        Debug.DrawRay(mainCamera.transform.position, directionFromCamera, Color.magenta);

        Vector3 forwardPlaneCenter = mainCamera.transform.position + cameraForwad;

        Debug.DrawLine(mainCamera.transform.position, forwardPlaneCenter, Color.blue);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraUp, Color.green);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraRight, Color.red);

        UpdateTargetIconPosition();
    }



    private void InstainateTargetIcon()
    {
        m_icon = new GameObject().AddComponent<RectTransform>();
        m_icon.transform.SetParent(mainCanvas.transform);
        m_icon.localScale = m_targetIconScale;

        m_icon.name = name + ": OTI icon";

        m_iconImage = m_icon.gameObject.AddComponent<Image>();
        m_iconImage.sprite = m_targetIconOnScreen;
    }


    private void UpdateTargetIconPosition()
    {
        Vector3 newPos = transform.position;

        newPos = mainCamera.WorldToViewportPoint(newPos);

        if(newPos.z < 0)
        {
            newPos.x = 1f - newPos.x;
            newPos.y = 1f - newPos.y;
            newPos.z = 0;
        }
        print(newPos);

        newPos = mainCamera.ViewportToScreenPoint(newPos);
        print(newPos);


        newPos.x = Mathf.Clamp(newPos.x, m_edgeBuffer, Screen.width - m_edgeBuffer);
        newPos.y = Mathf.Clamp(newPos.y, m_edgeBuffer, Screen.height - m_edgeBuffer);

        m_icon.transform.position = newPos;
    }
}
