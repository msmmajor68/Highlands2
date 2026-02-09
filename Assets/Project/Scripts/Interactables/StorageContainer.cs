using UnityEngine;
using FarmingRPG.Inventory;
using FarmingRPG.Player;

namespace FarmingRPG.Interactables
{
    /// <summary>
    /// Storage container (chest, crate, etc.) with its own inventory
    /// </summary>
    public class StorageContainer : MonoBehaviour, IInteractable
    {
        [Header("Storage Settings")]
        [SerializeField] private int storageSize = 20;
        [SerializeField] private string containerName = "Chest";
        
        [Header("Visuals")]
        [SerializeField] private Animator animator;
        [SerializeField] private string openAnimationTrigger = "Open";
        [SerializeField] private string closeAnimationTrigger = "Close";
        
        private InventorySlot[] storageSlots;
        private bool isOpen = false;
        
        // Events
        public event System.Action<StorageContainer> OnContainerOpened;
        public event System.Action<StorageContainer> OnContainerClosed;
        
        public string ContainerName => containerName;
        public int StorageSize => storageSize;
        public bool IsOpen => isOpen;
        
        private void Awake()
        {
            InitializeStorage();
        }
        
        /// <summary>
        /// Initialize storage slots
        /// </summary>
        private void InitializeStorage()
        {
            storageSlots = new InventorySlot[storageSize];
            for (int i = 0; i < storageSize; i++)
            {
                storageSlots[i] = new InventorySlot();
            }
        }
        
        /// <summary>
        /// Interact to open/close the container
        /// </summary>
        public void Interact(GameObject player)
        {
            if (isOpen)
                Close();
            else
                Open();
        }
        
        /// <summary>
        /// Open the container
        /// </summary>
        public void Open()
        {
            isOpen = true;
            
            if (animator != null && !string.IsNullOrEmpty(openAnimationTrigger))
            {
                animator.SetTrigger(openAnimationTrigger);
            }
            
            OnContainerOpened?.Invoke(this);
            Debug.Log($"Opened {containerName}");
        }
        
        /// <summary>
        /// Close the container
        /// </summary>
        public void Close()
        {
            isOpen = false;
            
            if (animator != null && !string.IsNullOrEmpty(closeAnimationTrigger))
            {
                animator.SetTrigger(closeAnimationTrigger);
            }
            
            OnContainerClosed?.Invoke(this);
            Debug.Log($"Closed {containerName}");
        }
        
        /// <summary>
        /// Add item to storage
        /// </summary>
        public bool AddItem(Item item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
                return false;
            
            int remainingQuantity = quantity;
            
            // Try to stack with existing items
            if (item.isStackable)
            {
                for (int i = 0; i < storageSize; i++)
                {
                    if (!storageSlots[i].IsEmpty && 
                        storageSlots[i].item == item && 
                        storageSlots[i].CanAddMore(item))
                    {
                        remainingQuantity = storageSlots[i].AddItem(item, remainingQuantity);
                        
                        if (remainingQuantity <= 0)
                            return true;
                    }
                }
            }
            
            // Add to empty slots
            for (int i = 0; i < storageSize; i++)
            {
                if (storageSlots[i].IsEmpty)
                {
                    remainingQuantity = storageSlots[i].AddItem(item, remainingQuantity);
                    
                    if (remainingQuantity <= 0)
                        return true;
                }
            }
            
            return remainingQuantity <= 0;
        }
        
        /// <summary>
        /// Remove item from storage
        /// </summary>
        public bool RemoveItem(Item item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
                return false;
            
            int remainingToRemove = quantity;
            
            for (int i = 0; i < storageSize; i++)
            {
                if (!storageSlots[i].IsEmpty && storageSlots[i].item == item)
                {
                    int amountInSlot = storageSlots[i].quantity;
                    int amountToRemove = Mathf.Min(remainingToRemove, amountInSlot);
                    
                    storageSlots[i].RemoveItem(amountToRemove);
                    remainingToRemove -= amountToRemove;
                    
                    if (remainingToRemove <= 0)
                        return true;
                }
            }
            
            return remainingToRemove <= 0;
        }
        
        /// <summary>
        /// Get storage slot at index
        /// </summary>
        public InventorySlot GetSlot(int index)
        {
            if (index < 0 || index >= storageSize)
                return null;
            
            return storageSlots[index];
        }
        
        /// <summary>
        /// Get all storage slots
        /// </summary>
        public InventorySlot[] GetAllSlots()
        {
            return storageSlots;
        }
        
        /// <summary>
        /// Transfer item from player to storage
        /// </summary>
        public bool TransferToStorage(InventoryManager playerInventory, Item item, int quantity)
        {
            if (!playerInventory.HasItem(item, quantity))
                return false;
            
            if (AddItem(item, quantity))
            {
                playerInventory.RemoveItem(item, quantity);
                Debug.Log($"Transferred {quantity}x {item.itemName} to {containerName}");
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Transfer item from storage to player
        /// </summary>
        public bool TransferToPlayer(InventoryManager playerInventory, Item item, int quantity)
        {
            if (playerInventory.AddItem(item, quantity))
            {
                RemoveItem(item, quantity);
                Debug.Log($"Transferred {quantity}x {item.itemName} from {containerName}");
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Clear all items
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < storageSize; i++)
            {
                storageSlots[i].Clear();
            }
        }
        
        /// <summary>
        /// Get total item count in storage
        /// </summary>
        public int GetItemCount(Item item)
        {
            if (item == null)
                return 0;
            
            int count = 0;
            for (int i = 0; i < storageSize; i++)
            {
                if (!storageSlots[i].IsEmpty && storageSlots[i].item == item)
                {
                    count += storageSlots[i].quantity;
                }
            }
            
            return count;
        }
        
        /// <summary>
        /// Check if storage is full
        /// </summary>
        public bool IsFull()
        {
            foreach (InventorySlot slot in storageSlots)
            {
                if (slot.IsEmpty)
                    return false;
            }
            return true;
        }
        
        /// <summary>
        /// Get number of empty slots
        /// </summary>
        public int GetEmptySlotCount()
        {
            int count = 0;
            foreach (InventorySlot slot in storageSlots)
            {
                if (slot.IsEmpty)
                    count++;
            }
            return count;
        }
    }
}
