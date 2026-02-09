using UnityEngine;
using System.Collections.Generic;

namespace FarmingRPG.Inventory
{
    /// <summary>
    /// Central database for all items in the game
    /// </summary>
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Farming RPG/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        [Header("Database")]
        [SerializeField] private List<Item> allItems = new List<Item>();
        
        private Dictionary<string, Item> itemDictionary;
        
        /// <summary>
        /// Initialize the database (call this on game start)
        /// </summary>
        public void Initialize()
        {
            itemDictionary = new Dictionary<string, Item>();
            
            foreach (Item item in allItems)
            {
                if (item != null && !itemDictionary.ContainsKey(item.itemName))
                {
                    itemDictionary.Add(item.itemName, item);
                }
            }
            
            Debug.Log($"Item Database initialized with {itemDictionary.Count} items");
        }
        
        /// <summary>
        /// Get an item by name
        /// </summary>
        public Item GetItemByName(string itemName)
        {
            if (itemDictionary == null)
                Initialize();
            
            if (itemDictionary.TryGetValue(itemName, out Item item))
            {
                return item;
            }
            
            Debug.LogWarning($"Item not found: {itemName}");
            return null;
        }
        
        /// <summary>
        /// Get all items of a specific type
        /// </summary>
        public List<Item> GetItemsByType(ItemType itemType)
        {
            List<Item> items = new List<Item>();
            
            foreach (Item item in allItems)
            {
                if (item != null && item.itemType == itemType)
                {
                    items.Add(item);
                }
            }
            
            return items;
        }
        
        /// <summary>
        /// Get all items
        /// </summary>
        public List<Item> GetAllItems()
        {
            return new List<Item>(allItems);
        }
        
        /// <summary>
        /// Add item to database (editor only)
        /// </summary>
        public void AddItem(Item item)
        {
            if (item != null && !allItems.Contains(item))
            {
                allItems.Add(item);
                #if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
                #endif
            }
        }
        
        /// <summary>
        /// Remove item from database (editor only)
        /// </summary>
        public void RemoveItem(Item item)
        {
            if (allItems.Contains(item))
            {
                allItems.Remove(item);
                #if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
                #endif
            }
        }
    }
}
