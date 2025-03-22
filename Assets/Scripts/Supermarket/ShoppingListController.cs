using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingListController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image tomatoImage;
    [SerializeField] private Image eggsImage;
    [SerializeField] private Image meatImage;
    [SerializeField] private Image milkImage;
    
    [Header("Status Colors")]
    [SerializeField] private Color defaultColor = Color.blue;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    
    private string saveFileName = "supermarket_data.csv";
    
    private Dictionary<FoodItem, Image> foodImages = new Dictionary<FoodItem, Image>();
    private bool allChoicesMade = false;

    private void Awake()
    {
        // Set up the dictionary mapping food types to their UI images
        foodImages[FoodItem.Tomatoes] = tomatoImage;
        foodImages[FoodItem.Eggs] = eggsImage;
        foodImages[FoodItem.Meat] = meatImage;
        foodImages[FoodItem.Milk] = milkImage;
    
    }

    private void Start()
    {
        // Initialize UI with default colors
        ResetAllColors();
        
        // Register for food status change events in SceneNavigator
        SceneNavigator.OnFoodStatusChanged += HandleFoodStatusChanged;
        
        // Update UI based on any existing food statuses
        UpdateAllFoodStatuses();
    }

    private void OnDestroy()
    {
        // Unregister from events when this object is destroyed
        if (SceneNavigator.Instance != null)
        {
            SceneNavigator.OnFoodStatusChanged -= HandleFoodStatusChanged;
        }
    }
    
    private void ResetAllColors()
    {
        foreach (Image image in foodImages.Values)
        {
            if (image != null)
            {
                image.color = defaultColor;
            }
        }
    }
    
    public void UpdateAllFoodStatuses()
    {
        if (SceneNavigator.Instance == null) return;
        
        foreach (FoodItem foodItem in foodImages.Keys)
        {
            Image image = foodImages[foodItem];
            if (image == null) continue;
            
            FoodStatus status = SceneNavigator.Instance.GetFoodStatus(foodItem);
            UpdateFoodImageColor(foodItem, status);
        }
        
        CheckAllChoicesMade();
    }
    
    private void HandleFoodStatusChanged(FoodItem foodItem, FoodStatus status)
    {
        // Update UI color for the changed food item
        UpdateFoodImageColor(foodItem, status);
        
        // Check if all choices have been made
        CheckAllChoicesMade();
    }
    
    private void UpdateFoodImageColor(FoodItem foodItem, FoodStatus status)
    {
        if (!foodImages.TryGetValue(foodItem, out Image image) || image == null)
            return;
            
        switch (status)
        {
            case FoodStatus.RightChoiceChosen:
                image.color = correctColor;
                break;
            case FoodStatus.WrongChoiceChosen:
                image.color = wrongColor;
                break;
            case FoodStatus.NotDone:
                image.color = defaultColor;
                break;
        }
    }
    
    private void CheckAllChoicesMade()
    {
        // If we already determined all choices were made, no need to check again
        if (allChoicesMade) return;
        
        if (SceneNavigator.Instance == null) return;
        
        bool allDone = true;
        
        // Check if all food items have a status other than NotDone
        foreach (FoodItem foodItem in Enum.GetValues(typeof(FoodItem)))
        {
            FoodStatus status = SceneNavigator.Instance.GetFoodStatus(foodItem);
            if (status == FoodStatus.NotDone)
            {
                allDone = false;
                break;
            }
        }
        
        if (allDone)
        {
            allChoicesMade = true;
            
            // Save the data to a file
            SaveDataToFile();
        }
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
        string filePath = Path.Combine(directory, saveFileName);
        bool fileExists = File.Exists(filePath);
        
        // Create a StringBuilder to build our CSV content
        StringBuilder sb = new StringBuilder();
        
        // If file doesn't exist, add header row first
        if (!fileExists)
        {
            sb.AppendLine("tomatoes,meat,eggs,milk,total,time_seconds,entry_timestamp,save_timestamp");
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
        
        int total = tomatoes + meat + eggs + milk;

        string entryTime = SceneNavigator.supermarketEntryTime.HasValue ? SceneNavigator.supermarketEntryTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "N/A";
        string saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string timeSeconds = elapsedTime.HasValue ? elapsedTime.Value.TotalSeconds.ToString("F1") : "N/A";
        
        sb.AppendLine($"{tomatoes},{meat},{eggs},{milk},{total},{timeSeconds},{entryTime},{saveTime}");
        
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