using System;
using UnityEngine;

namespace FarmingRPG.Inventory
{
    /// <summary>
    /// Represents a single inventory slot that can hold items
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        public Item item;
        public int quantity;
        
        public InventorySlot()
        {
            item = null;
            quantity = 0;
        }
        
        public InventorySlot(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
        
        /// <summary>
        /// Check if this slot is empty
        /// </summary>
        public bool IsEmpty => item == null || quantity <= 0;
        
        /// <summary>
        /// Check if slot can accept more of this item
        /// </summary>
        public bool CanAddMore(Item itemToAdd)
        {
            if (IsEmpty) return true;
            if (item != itemToAdd) return false;
            if (!item.isStackable) return false;
            return quantity < item.maxStackSize;
        }
        
        /// <summary>
        /// Add items to this slot
        /// </summary>
        /// <returns>Number of items that couldn't be added</returns>
        public int AddItem(Item itemToAdd, int amount)
        {
            if (IsEmpty)
            {
                item = itemToAdd;
                int amountToAdd = itemToAdd.isStackable ? 
                    Mathf.Min(amount, itemToAdd.maxStackSize) : 1;
                quantity = amountToAdd;
                return amount - amountToAdd;
            }
            
            if (item != itemToAdd || !item.isStackable)
                return amount;
            
            int spaceAvailable = item.maxStackSize - quantity;
            int stackAmount = Mathf.Min(amount, spaceAvailable);
            quantity += stackAmount;
            
            return amount - stackAmount;
        }
        
        /// <summary>
        /// Remove items from this slot
        /// </summary>
        public bool RemoveItem(int amount)
        {
            if (quantity < amount)
                return false;
            
            quantity -= amount;
            
            if (quantity <= 0)
                Clear();
            
            return true;
        }
        
        /// <summary>
        /// Clear the slot
        /// </summary>
        public void Clear()
        {
            item = null;
            quantity = 0;
        }
        
        /// <summary>
        /// Get a copy of this slot
        /// </summary>
        public InventorySlot Copy()
        {
            return new InventorySlot(item, quantity);
        }
    }
}
