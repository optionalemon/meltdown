using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;

public class ResultsManager : MonoBehaviour
{

    private static ResultsManager instance;

    private Dictionary<FoodItem, bool> scores = new Dictionary<FoodItem, bool>();

    private TimeSpan? supermarketDuration;

    private TimeSpan? foodWasteDuration;

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

    public void SaveDataToFile()
    {
        try
        {
            // Create the directory if it doesn't exist
            string directory = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Data");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Full path with platform-safe directory separators
            string filePath = Path.Combine(directory, "data.csv");
            bool fileExists = File.Exists(filePath);

            // Create a StringBuilder to build our CSV content
            StringBuilder sb = new StringBuilder();

            // If file doesn't exist, add header row first
            if (!fileExists)
            {
                sb.AppendLine("total,supermarket_duration,foodwaste_duration,tomatoes,meat,eggs,milk,coffee_grounds,egg_shells,fruit_veg_slices,bones");
            }

            // Calculate elapsed time since entering supermarket
            TimeSpan? elapsedTime = null;
            if (SceneNavigator.supermarketEntryTime != null)
            {
                elapsedTime = DateTime.Now - SceneNavigator.supermarketEntryTime.Value;
            }

            // Add the current data to the CSV, 0 if the food item is wrong and 1 if it's correct
            int tomatoes = SceneNavigator.Instance.GetFoodStatus(FoodItem.Tomatoes) == FoodStatus.RightChoiceChosen ? 1 : 0;
            int meat = SceneNavigator.Instance.GetFoodStatus(FoodItem.Meat) == FoodStatus.RightChoiceChosen ? 1 : 0;
            int eggs = SceneNavigator.Instance.GetFoodStatus(FoodItem.Eggs) == FoodStatus.RightChoiceChosen ? 1 : 0;
            int milk = SceneNavigator.Instance.GetFoodStatus(FoodItem.Milk) == FoodStatus.RightChoiceChosen ? 1 : 0;
            int coffeeGrounds = SceneNavigator.Instance.GetFoodStatus(FoodItem.CoffeeGrounds) == FoodStatus.RightChoiceChosen ? 1 : 0;
            int eggShells = SceneNavigator.Instance.GetFoodStatus(FoodItem.Eggshells) == FoodStatus.RightChoiceChosen ? 1 : 0;
            int fruitVegSlices = SceneNavigator.Instance.GetFoodStatus(FoodItem.VegFruitSlices) == FoodStatus.RightChoiceChosen ? 1 : 0;
            int bones = SceneNavigator.Instance.GetFoodStatus(FoodItem.ChickenBone) == FoodStatus.RightChoiceChosen ? 1 : 0;
            int total = tomatoes + meat + eggs + milk + coffeeGrounds + eggShells + fruitVegSlices + bones;

            sb.AppendLine($"{total},{supermarketDuration?.TotalSeconds},{foodWasteDuration?.TotalSeconds},{tomatoes},{meat},{eggs},{milk},{coffeeGrounds},{eggShells},{fruitVegSlices},{bones}");

            // Append to file (create if doesn't exist)
            File.AppendAllText(filePath, sb.ToString());

            Debug.Log($"Data successfully saved to {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data: {e.Message}");
        }
    }

}
