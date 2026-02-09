using UnityEngine;

namespace FarmingRPG.Inventory
{
    /// <summary>
    /// Base class for all items in the game
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Farming RPG/Items/Base Item")]
    public class Item : ScriptableObject
    {
        [Header("Basic Info")]
        public string itemName = "New Item";
        [TextArea(3, 5)]
        public string description = "Item description";
        public Sprite icon;
        public ItemType itemType;
        
        [Header("Stack Settings")]
        public bool isStackable = true;
        public int maxStackSize = 99;
        
        [Header("Value")]
        public int buyPrice = 0;
        public int sellPrice = 0;
        
        [Header("Prefab")]
        public GameObject worldPrefab; // Prefab when dropped in world
        
        /// <summary>
        /// Called when item is used
        /// </summary>
        public virtual bool Use(GameObject user)
        {
            Debug.Log($"Used item: {itemName}");
            return false; // Return true if item should be consumed
        }
        
        /// <summary>
        /// Get a copy of this item
        /// </summary>
        public virtual Item GetCopy()
        {
            return Instantiate(this);
        }
    }
}
