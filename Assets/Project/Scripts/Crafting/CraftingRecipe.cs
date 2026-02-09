using UnityEngine;
using System.Collections.Generic;
using FarmingRPG.Inventory;

namespace FarmingRPG.Crafting
{
    /// <summary>
    /// Recipe for crafting items
    /// </summary>
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Farming RPG/Crafting/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        [Header("Recipe Info")]
        public string recipeName = "New Recipe";
        [TextArea(2, 4)]
        public string description = "Recipe description";
        
        [Header("Result")]
        public Item resultItem;
        public int resultQuantity = 1;
        
        [Header("Requirements")]
        public List<CraftingIngredient> ingredients = new List<CraftingIngredient>();
        
        [Header("Crafting Settings")]
        public float craftTime = 2f; // Seconds
        public CraftingStation requiredStation = CraftingStation.None;
        
        /// <summary>
        /// Check if player has all required ingredients
        /// </summary>
        public bool CanCraft(Inventory.InventoryManager inventory)
        {
            foreach (CraftingIngredient ingredient in ingredients)
            {
                if (!inventory.HasItem(ingredient.item, ingredient.quantity))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Consume ingredients and craft the item
        /// </summary>
        public bool Craft(Inventory.InventoryManager inventory)
        {
            if (!CanCraft(inventory))
                return false;
            
            // Remove ingredients
            foreach (CraftingIngredient ingredient in ingredients)
            {
                inventory.RemoveItem(ingredient.item, ingredient.quantity);
            }
            
            // Add result
            inventory.AddItem(resultItem, resultQuantity);
            
            Debug.Log($"Crafted {resultQuantity}x {resultItem.itemName}");
            return true;
        }
    }
    
    [System.Serializable]
    public class CraftingIngredient
    {
        public Item item;
        public int quantity = 1;
    }
    
    public enum CraftingStation
    {
        None,
        Workbench,
        Furnace,
        Anvil,
        CookingPot,
        Alchemy
    }
}
