using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResultsManager : MonoBehaviour
{
    // TODO: track food waste time when teleporting (in door controller) and when the end logic for food waste room is there.
    private static ResultsManager instance;

    private Dictionary<FoodItem, bool> scores = new Dictionary<FoodItem, bool>();

    private float supermarketStartTime;

    private float supermarketDuration;

    private float foodWasteStartTime;

    private float foodWasteDuration;

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

    public void StartTrackingSupermarketTime()
    {
        if (!isSupermarketTimeInitialized)
        {
            supermarketStartTime = Time.time;
            isSupermarketTimeInitialized = true;
        }

    }

    public void StopTrackingSupermarketTime()
    {
        supermarketDuration = Time.time - supermarketStartTime;
    }

    public float? GetSupermarketDuration()
    {
        return supermarketDuration;
    }

    public void StartTrackingFoodWasteTime()
    {
        foodWasteStartTime = Time.time;
    }

    public void StopTrackingFoodWasteTime()
    {
        foodWasteDuration = Time.time - foodWasteStartTime;
    }

    public float? GetFoodWasteDuration()
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
