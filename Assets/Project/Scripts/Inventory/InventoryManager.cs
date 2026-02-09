using System;
using System.Collections.Generic;
using UnityEngine;
using FarmingRPG.Player;

namespace FarmingRPG.Inventory
{
    /// <summary>
    /// Main inventory system that manages item storage
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        [Header("Inventory Settings")]
        [SerializeField] private int inventorySize = 30;
        [SerializeField] private int hotbarSize = 10;
        
        private InventorySlot[] inventorySlots;
        private HotbarSlot[] hotbarSlots;
        private int selectedHotbarIndex = 0;
        
        // Events
        public event Action<int> OnInventoryChanged;
        public event Action<int> OnHotbarChanged;
        public event Action<int, InventorySlot> OnSlotChanged;
        public event Action<int, HotbarSlot> OnHotbarSlotChanged;
        
        public int InventorySize => inventorySize;
        public int HotbarSize => hotbarSize;
        public int SelectedHotbarIndex => selectedHotbarIndex;
        
        private void Awake()
        {
            InitializeInventory();
        }
        
        private void InitializeInventory()
        {
            inventorySlots = new InventorySlot[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                inventorySlots[i] = new InventorySlot();
            }
            
            hotbarSlots = new HotbarSlot[hotbarSize];
            for (int i = 0; i < hotbarSize; i++)
            {
                hotbarSlots[i] = new HotbarSlot();
            }
        }
        
        /// <summary>
        /// Add item to inventory
        /// </summary>
        public bool AddItem(Item item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
                return false;
            
            int remainingQuantity = quantity;
            
            // First, try to stack with existing items
            if (item.isStackable)
            {
                for (int i = 0; i < inventorySize; i++)
                {
                    if (!inventorySlots[i].IsEmpty && 
                        inventorySlots[i].item == item && 
                        inventorySlots[i].CanAddMore(item))
                    {
                        remainingQuantity = inventorySlots[i].AddItem(item, remainingQuantity);
                        OnSlotChanged?.Invoke(i, inventorySlots[i]);
                        
                        if (remainingQuantity <= 0)
                        {
                            OnInventoryChanged?.Invoke(i);
                            return true;
                        }
                    }
                }
            }
            
            // Then, add to empty slots
            for (int i = 0; i < inventorySize; i++)
            {
                if (inventorySlots[i].IsEmpty)
                {
                    remainingQuantity = inventorySlots[i].AddItem(item, remainingQuantity);
                    OnSlotChanged?.Invoke(i, inventorySlots[i]);
                    
                    if (remainingQuantity <= 0)
                    {
                        OnInventoryChanged?.Invoke(i);
                        return true;
                    }
                }
            }
            
            // If we still have items left, inventory is full
            if (remainingQuantity > 0)
            {
                Debug.LogWarning($"Inventory full! Could not add {remainingQuantity} of {item.itemName}");
                OnInventoryChanged?.Invoke(-1);
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Remove item from inventory
        /// </summary>
        public bool RemoveItem(Item item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
                return false;
            
            int remainingToRemove = quantity;
            
            for (int i = 0; i < inventorySize; i++)
            {
                if (!inventorySlots[i].IsEmpty && inventorySlots[i].item == item)
                {
                    int amountInSlot = inventorySlots[i].quantity;
                    int amountToRemove = Mathf.Min(remainingToRemove, amountInSlot);
                    
                    inventorySlots[i].RemoveItem(amountToRemove);
                    remainingToRemove -= amountToRemove;
                    OnSlotChanged?.Invoke(i, inventorySlots[i]);
                    UpdateHotbarReferences(i);
                    
                    if (remainingToRemove <= 0)
                    {
                        OnInventoryChanged?.Invoke(i);
                        return true;
                    }
                }
            }
            
            return remainingToRemove <= 0;
        }
        
        /// <summary>
        /// Check if inventory has enough of an item
        /// </summary>
        public bool HasItem(Item item, int quantity = 1)
        {
            int count = GetItemCount(item);
            return count >= quantity;
        }
        
        /// <summary>
        /// Get total count of an item in inventory
        /// </summary>
        public int GetItemCount(Item item)
        {
            if (item == null)
                return 0;
            
            int count = 0;
            for (int i = 0; i < inventorySize; i++)
            {
                if (!inventorySlots[i].IsEmpty && inventorySlots[i].item == item)
                {
                    count += inventorySlots[i].quantity;
                }
            }
            
            return count;
        }
        
        /// <summary>
        /// Get slot at index
        /// </summary>
        public InventorySlot GetSlot(int index)
        {
            if (index < 0 || index >= inventorySize)
                return null;
            
            return inventorySlots[index];
        }
        
        /// <summary>
        /// Get all slots
        /// </summary>
        public InventorySlot[] GetAllSlots()
        {
            return inventorySlots;
        }
        
        /// <summary>
        /// Use item at slot index
        /// </summary>
        public bool UseItem(int slotIndex, GameObject user)
        {
            if (slotIndex < 0 || slotIndex >= inventorySize)
                return false;
            
            InventorySlot slot = inventorySlots[slotIndex];
            if (slot.IsEmpty)
                return false;
            
            bool consumed = slot.item.Use(user);
            
            if (consumed)
            {
                slot.RemoveItem(1);
                OnSlotChanged?.Invoke(slotIndex, slot);
                OnInventoryChanged?.Invoke(slotIndex);
                UpdateHotbarReferences(slotIndex);
            }
            
            return consumed;
        }
        
        /// <summary>
        /// Use item from hotbar slot
        /// </summary>
        public bool UseHotbarItem(int hotbarIndex, GameObject user)
        {
            if (hotbarIndex < 0 || hotbarIndex >= hotbarSize)
                return false;
            
            HotbarSlot hotbarSlot = hotbarSlots[hotbarIndex];
            if (!hotbarSlot.HasItem)
                return false;
            
            // Use the item from the referenced inventory slot
            return UseItem(hotbarSlot.inventorySlotIndex, user);
        }
        
        /// <summary>
        /// Swap items between two slots
        /// </summary>
        public void SwapSlots(int slotIndex1, int slotIndex2)
        {
            if (slotIndex1 < 0 || slotIndex1 >= inventorySize ||
                slotIndex2 < 0 || slotIndex2 >= inventorySize)
                return;
            
            InventorySlot temp = inventorySlots[slotIndex1].Copy();
            inventorySlots[slotIndex1] = inventorySlots[slotIndex2].Copy();
            inventorySlots[slotIndex2] = temp;
            
            OnSlotChanged?.Invoke(slotIndex1, inventorySlots[slotIndex1]);
            OnSlotChanged?.Invoke(slotIndex2, inventorySlots[slotIndex2]);
            OnInventoryChanged?.Invoke(-1);
        }
        
        /// <summary>
        /// Select hotbar slot
        /// </summary>
        public void SelectHotbarSlot(int index)
        {
            if (index < 0 || index >= hotbarSize)
                return;
            
            selectedHotbarIndex = index;
            OnHotbarChanged?.Invoke(selectedHotbarIndex);
        }
        
        /// <summary>
        /// Get currently selected hotbar item
        /// </summary>
        public InventorySlot GetSelectedHotbarSlot()
        {
            if (selectedHotbarIndex < 0 || selectedHotbarIndex >= hotbarSize)
                return null;
            
            HotbarSlot hotbarSlot = hotbarSlots[selectedHotbarIndex];
            if (hotbarSlot == null || !hotbarSlot.HasItem)
                return null;
            
            return GetSlot(hotbarSlot.inventorySlotIndex);
        }
        
        /// <summary>
        /// Assign an inventory slot to a hotbar slot
        /// </summary>
        public void AssignToHotbar(int inventoryIndex, int hotbarIndex)
        {
            if (inventoryIndex < 0 || inventoryIndex >= inventorySize)
                return;
            
            if (hotbarIndex < 0 || hotbarIndex >= hotbarSize)
                return;
            
            // Assign the inventory slot reference to the hotbar
            hotbarSlots[hotbarIndex].inventorySlotIndex = inventoryIndex;
            hotbarSlots[hotbarIndex].item = inventorySlots[inventoryIndex].item;
            
            OnHotbarSlotChanged?.Invoke(hotbarIndex, hotbarSlots[hotbarIndex]);
        }
        
        /// <summary>
        /// Remove item from hotbar slot
        /// </summary>
        public void RemoveFromHotbar(int hotbarIndex)
        {
            if (hotbarIndex < 0 || hotbarIndex >= hotbarSize)
                return;
            
            hotbarSlots[hotbarIndex].Clear();
            OnHotbarSlotChanged?.Invoke(hotbarIndex, hotbarSlots[hotbarIndex]);
        }
        
        /// <summary>
        /// Get hotbar slot
        /// </summary>
        public HotbarSlot GetHotbarSlot(int index)
        {
            if (index < 0 || index >= hotbarSize)
                return null;
            
            return hotbarSlots[index];
        }
        
        /// <summary>
        /// Update hotbar references when inventory changes
        /// </summary>
        private void UpdateHotbarReferences(int changedInventoryIndex)
        {
            // Update any hotbar slots that reference this inventory slot
            for (int i = 0; i < hotbarSize; i++)
            {
                if (hotbarSlots[i].HasItem && hotbarSlots[i].inventorySlotIndex == changedInventoryIndex)
                {
                    InventorySlot invSlot = GetSlot(changedInventoryIndex);
                    
                    // If inventory slot is now empty, clear hotbar slot
                    if (invSlot.IsEmpty)
                    {
                        hotbarSlots[i].Clear();
                    }
                    else
                    {
                        // Update the item reference
                        hotbarSlots[i].item = invSlot.item;
                    }
                    
                    OnHotbarSlotChanged?.Invoke(i, hotbarSlots[i]);
                }
            }
        }
        
        /// <summary>
        /// Clear entire inventory
        /// </summary>
        public void ClearInventory()
        {
            for (int i = 0; i < inventorySize; i++)
            {
                inventorySlots[i].Clear();
                OnSlotChanged?.Invoke(i, inventorySlots[i]);
            }
            
            OnInventoryChanged?.Invoke(-1);
        }
        
        /// <summary>
        /// Drop item from inventory slot into the world
        /// </summary>
        public void DropItem(int slotIndex, Vector3 dropPosition, int quantity = 1)
        {
            if (slotIndex < 0 || slotIndex >= inventorySize)
                return;
            
            InventorySlot slot = inventorySlots[slotIndex];
            if (slot.IsEmpty)
                return;
            
            Item itemToDrop = slot.item;
            int actualQuantity = Mathf.Min(quantity, slot.quantity);
            
            // Remove from inventory
            slot.RemoveItem(actualQuantity);
            OnSlotChanged?.Invoke(slotIndex, slot);
            OnInventoryChanged?.Invoke(slotIndex);
            UpdateHotbarReferences(slotIndex);
            
            // Spawn in world
            SpawnItemInWorld(itemToDrop, dropPosition, actualQuantity);
        }
        
        /// <summary>
        /// Spawn item prefab in the world
        /// </summary>
        private void SpawnItemInWorld(Item item, Vector3 position, int quantity)
        {
            if (item == null)
                return;
            
            GameObject prefabToSpawn = item.worldPrefab;
            
            // If item doesn't have a custom prefab, try to find a generic pickup prefab
            if (prefabToSpawn == null)
            {
                // Look for ItemPickup prefab in the scene as a template
                ItemPickup templatePickup = FindObjectOfType<ItemPickup>();
                if (templatePickup != null)
                {
                    prefabToSpawn = templatePickup.gameObject;
                }
            }
            
            if (prefabToSpawn == null)
            {
                Debug.LogWarning($"Cannot drop item {item.itemName}: no world prefab assigned and no ItemPickup template found");
                return;
            }
            
            // Spawn the item
            GameObject droppedItem = Instantiate(prefabToSpawn, position, Quaternion.identity);
            ItemPickup pickup = droppedItem.GetComponent<ItemPickup>();
            
            if (pickup != null)
            {
                pickup.item = item;
                pickup.quantity = quantity;
                pickup.UpdateVisuals();
                pickup.ResetPickupDelay(); // Reset the delay so it won't immediately be picked up
            }
            
            // Add a small random offset so multiple items don't stack exactly
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-0.3f, 0.3f),
                0,
                UnityEngine.Random.Range(-0.3f, 0.3f)
            );
            droppedItem.transform.position += randomOffset;
        }
    }
    
    /// <summary>
    /// Represents a hotbar slot that references an inventory slot
    /// </summary>
    [System.Serializable]
    public class HotbarSlot
    {
        public int inventorySlotIndex = -1; // Index of the inventory slot this references
        public Item item = null; // Cached reference to the item
        
        public bool HasItem => inventorySlotIndex >= 0 && item != null;
        
        public void Clear()
        {
            inventorySlotIndex = -1;
            item = null;
        }
    }
}
