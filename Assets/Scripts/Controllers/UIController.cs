using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private RectTransform resetButton;

    [SerializeField]
    private RectTransform statsPanel;

    [SerializeField]
    private RectTransform logoPanel;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Image fadeArea;

    // Reset button values
    private Vector2 hidePosition = new Vector2(0, -110);
    private Vector2 showPosition = new Vector2(0, 110);

    // Stats panel values
    private Vector2 panelHidePosition = new Vector2(-1300, -750);
    private Vector2 panelShowPosition = new Vector2(-250, -750);

    // Logo panel values
    private Vector2 logoHidePosition = new Vector2(1200, 0);
    private Vector2 logoShowPosition = new Vector2(0, 0);

    public static event Action OnGameStart = delegate { };
    public static event Action ResetAll = delegate { };

    private void Awake()
    {
        DeathZone.OnDead += OnDead;
    }

    private void Start()
    {
        fadeArea.DOFade(0, 1).OnComplete(() => fadeArea.raycastTarget = false);
    }

    private void OnDead(string zoneTag)
    {
        statsPanel.DOAnchorPos(panelShowPosition, 1);
        resetButton.DOAnchorPos(showPosition, 2);
    }

    public void StartGame()
    {
        logoPanel.DOAnchorPos(logoHidePosition, 1).SetEase(Ease.InOutSine).
            OnComplete(() => startButton.enabled = true);

        startButton.enabled = false;
        OnGameStart();
    }

    public void Reset()
    {
        fadeArea.raycastTarget = true;
        fadeArea.DOFade(1, 0.5f).OnComplete(ShowStartScreen);
    }

    private void ShowStartScreen()
    {
        resetButton.DOKill();
        resetButton.anchoredPosition = hidePosition;
        statsPanel.anchoredPosition = panelHidePosition;
        logoPanel.anchoredPosition = logoShowPosition;

        ResetAll();
        PlatformManager.Instance.Reset();

        fadeArea.DOFade(0, 1).OnComplete(() => fadeArea.raycastTarget = false);
    }
}
