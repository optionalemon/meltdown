using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ResultsManager : MonoBehaviour
{
    // TODO: track food waste time when teleporting (in door controller) and when the end logic for food waste room is there.
    private static ResultsManager instance;

    private Dictionary<FoodItem, bool> scores = new Dictionary<FoodItem, bool>();

    // private float supermarketStartTime;

    private TimeSpan? supermarketDuration;

    // private float foodWasteStartTime;

    private TimeSpan? foodWasteDuration;

    private bool isSupermarketTimeInitialized = false;

    public static ResultsManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }
    public void UpdateScore(FoodItem category, bool isCorrect)
    {
        scores[category] = isCorrect;
    }

    // Method to get scores at the end scene
    public Dictionary<FoodItem, bool> GetResults()
    {
        return new Dictionary<FoodItem, bool>(scores); // Return a copy to prevent modification
    }

    public void StopTrackingSupermarketTime()
    {
        supermarketDuration = DateTime.Now - SceneNavigator.supermarketEntryTime;
    }

    public TimeSpan? GetSupermarketDuration()
    {
        return supermarketDuration;
    }

    public void StopTrackingFoodWasteTime()
    {
        foodWasteDuration = DateTime.Now - SceneNavigator.foodwasteEntryTime;
    }

    public TimeSpan? GetFoodWasteDuration()
    {
        return foodWasteDuration;
    }
    // Optional: Reset scores if needed
    public void ResetScores()
    {
        scores.Clear();
    }

    public int GetSpecificCategoryCount(FoodItem[] relevantItems)
    {
        return scores.Where(pair => relevantItems.Contains(pair.Key) && pair.Value).Count();
    }

}
