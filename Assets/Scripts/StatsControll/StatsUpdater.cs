using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUpdater : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text goldText;

    [SerializeField]
    private TMP_Text statsCurrentScore;

    [SerializeField]
    private TMP_Text statsBestScore;

    void Awake()
    {
        StatsCounter.OnStatsChanged += UpdateStats;
    }

    private void UpdateStats(int score, int bestScore, int gold)
    {
        scoreText.text = score.ToString();
        goldText.text = gold.ToString();

        statsCurrentScore.text = score.ToString();
        statsBestScore.text = bestScore.ToString();
    }
}
