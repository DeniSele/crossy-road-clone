using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCounter : MonoBehaviour
{   
    private int score = 0;
    private int bestScore;

    private int currentGold;

    private float offset = 5;
    private float maxZPosition;

    private IStoreData dataStorage;

    private const string SCORE_KEY = "SCORE_KEY";
    private const string GOLD_KEY = "GOLD_KEY";

    public static event Action<int, int, int> OnStatsChanged = delegate { };

    private void Awake()
    {
        maxZPosition = offset;

        PlayerController.OnJump += UpdateScore;
        GoldPicker.OnGoldPick += UpdateGold;
        UIController.ResetAll += Reset;
    }

    private void Start()
    {
        dataStorage = GetComponent<IStoreData>();
        bestScore = dataStorage.LoadInt(SCORE_KEY);
        currentGold = dataStorage.LoadInt(GOLD_KEY);

        OnStatsChanged(score, bestScore, currentGold);
    }

    public void Reset()
    {
        score = 0;
        maxZPosition = offset;
        OnStatsChanged(score, bestScore, currentGold);
    }

    private void UpdateGold(int goldToAdd)
    {
        currentGold += goldToAdd;
        dataStorage.SaveInt(currentGold, GOLD_KEY);
        OnStatsChanged(score, bestScore, currentGold);
    }

    private void UpdateScore(Vector3 targetPosition)
    {
        if(targetPosition.z > maxZPosition)
        {
            maxZPosition = targetPosition.z;
            score = (int)(maxZPosition - offset);

            OnStatsChanged(score, bestScore, currentGold);

            if (score > bestScore)
            {
                bestScore = score;
                dataStorage.SaveInt(bestScore, SCORE_KEY);
            }
        }
    }
}
