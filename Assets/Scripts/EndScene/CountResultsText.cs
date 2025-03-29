using System;
using TMPro;
using UnityEngine;

public class CountResultsText : MonoBehaviour
{

    [SerializeField] private TMP_Text stats;
    [SerializeField] private ScoreType scoreType;

    [SerializeField] private bool isTimeStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int score = 0;
        if (!isTimeStats)
        {
            if (scoreType == ScoreType.SUPERMARKET)
            {
                score = CountSupermarketStats();
                stats.text = $"{score}/4";
            }
            else if (scoreType == ScoreType.FOODWASTE)
            {
                score = CountFoodWasteStats();
                stats.text = $"{score}/4";
            }
        }
        else
        {
            if (scoreType == ScoreType.SUPERMARKET)
            {
                TimeSpan? time = ResultsManager.Instance.GetSupermarketDuration();
                if (time.HasValue)
                {
                    int minutes = time.Value.Minutes;
                    int seconds = time.Value.Seconds;
                    stats.text = $"{minutes:D2}m {seconds:D2}s";
                }
                else
                {
                    stats.text = "00m 00s";
                }
            }
            else if (scoreType == ScoreType.FOODWASTE)
            {
                TimeSpan? time = ResultsManager.Instance.GetFoodWasteDuration();
                if (time.HasValue)
                {

                    int minutes = time.Value.Minutes;
                    int seconds = time.Value.Seconds;
                    stats.text = $"{minutes:D2}m {seconds:D2}s";
                }
                else
                {
                    stats.text = "00m 00s";
                }
            }
        }
    }

    int CountSupermarketStats()
    {
        return ResultsManager.Instance.GetSpecificCategoryCount(new FoodItem[] { FoodItem.Tomatoes, FoodItem.Milk, FoodItem.Meat, FoodItem.Eggs });
    }

    int CountFoodWasteStats()
    {
        return ResultsManager.Instance.GetSpecificCategoryCount(new FoodItem[] { FoodItem.ChickenBone, FoodItem.Eggshells, FoodItem.CoffeeGrounds, FoodItem.VegFruitWaste });
    }


}
