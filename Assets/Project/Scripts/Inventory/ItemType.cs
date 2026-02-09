using UnityEngine;

namespace FarmingRPG.Inventory
{
    /// <summary>
    /// Defines the different types of items in the game
    /// </summary>
    public enum ItemType
    {
        None,
        Consumable,     // Food, potions
        Tool,           // Farming tools, weapons
        Seed,           // Seeds for planting
        Resource,       // Wood, stone, ore
        Crop,           // Harvested crops
        Equipment,      // Armor, accessories
        Quest,          // Quest items
        Furniture,      // Placeable objects
        Crafting        // Crafting materials
    }
}
