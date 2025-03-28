using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    private static ResultsManager instance;

    private Dictionary<FoodItem, bool> scores = new Dictionary<FoodItem, bool>();

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

    // Optional: Reset scores if needed
    public void ResetScores()
    {
        scores.Clear();
    }
}
