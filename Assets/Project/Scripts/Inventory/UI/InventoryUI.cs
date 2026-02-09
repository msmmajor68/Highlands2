using UnityEngine;
using System.Collections.Generic;

namespace FarmingRPG.Inventory.UI
{
    /// <summary>
    /// Main UI controller for the inventory system
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private GameObject slotPrefab;
        
        [Header("Settings")]
        [SerializeField] private bool startClosed = true;
        
        private List<ItemSlotUI> slotUIList = new List<ItemSlotUI>();
        private bool isOpen = false;
        
        public bool IsOpen => isOpen;
        
        private void Start()
        {
            if (inventoryManager == null)
                inventoryManager = FindObjectOfType<InventoryManager>();
            
            InitializeSlots();
            
            // Subscribe to inventory events
            if (inventoryManager != null)
            {
                inventoryManager.OnSlotChanged += UpdateSlotUI;
                inventoryManager.OnInventoryChanged += RefreshInventory;
            }
            
            if (startClosed)
                Close();
            else
                Open();
        }
        
        private void OnDestroy()
        {
            if (inventoryManager != null)
            {
                inventoryManager.OnSlotChanged -= UpdateSlotUI;
                inventoryManager.OnInventoryChanged -= RefreshInventory;
            }
        }
        
        private void Update()
        {
            // Toggle inventory with Tab or I key
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
        }
        
        /// <summary>
        /// Initialize all inventory slot UIs
        /// </summary>
        private void InitializeSlots()
        {
            if (inventoryManager == null || slotPrefab == null || slotsContainer == null)
                return;
            
            // Clear existing slots
            foreach (Transform child in slotsContainer)
            {
                Destroy(child.gameObject);
            }
            slotUIList.Clear();
            
            // Create slots
            int totalSlots = inventoryManager.InventorySize;
            for (int i = 0; i < totalSlots; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
                ItemSlotUI slotUI = slotObj.GetComponent<ItemSlotUI>();
                
                if (slotUI != null)
                {
                    slotUI.Initialize(i, this);
                    slotUIList.Add(slotUI);
                    
                    // Update with current inventory data
                    InventorySlot slot = inventoryManager.GetSlot(i);
                    slotUI.UpdateSlot(slot);
                }
            }
        }
        
        /// <summary>
        /// Update a specific slot UI
        /// </summary>
        private void UpdateSlotUI(int index, InventorySlot slot)
        {
            if (index >= 0 && index < slotUIList.Count)
            {
                slotUIList[index].UpdateSlot(slot);
            }
        }
        
        /// <summary>
        /// Refresh entire inventory display
        /// </summary>
        private void RefreshInventory(int changedIndex)
        {
            if (inventoryManager == null)
                return;
            
            InventorySlot[] slots = inventoryManager.GetAllSlots();
            for (int i = 0; i < slots.Length && i < slotUIList.Count; i++)
            {
                slotUIList[i].UpdateSlot(slots[i]);
            }
        }
        
        /// <summary>
        /// Called when a slot is clicked
        /// </summary>
        public void OnSlotClicked(int slotIndex)
        {
            // Shift+Click to drop single item
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                DropItemFromSlot(slotIndex, 1);
            }
            else
            {
                Debug.Log($"Slot {slotIndex} clicked");
                // Can be used for item selection, quick move, etc.
            }
        }
        
        /// <summary>
        /// Drop item from slot into the world
        /// </summary>
        private void DropItemFromSlot(int slotIndex, int quantity)
        {
            if (inventoryManager == null)
                return;
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return;
            
            // Drop in front of the player
            Vector3 dropPosition = player.transform.position + player.transform.forward * 1.5f;
            inventoryManager.DropItem(slotIndex, dropPosition, quantity);
        }
        
        /// <summary>
        /// Public method for dropping items from inventory (called by ItemSlotUI)
        /// </summary>
        public void DropItemFromInventory(int slotIndex, int quantity)
        {
            DropItemFromSlot(slotIndex, quantity);
        }
        
        /// <summary>
        /// Called when a slot is right-clicked
        /// </summary>
        public void OnSlotRightClicked(int slotIndex)
        {
            if (inventoryManager == null)
                return;
            
            InventorySlot slot = inventoryManager.GetSlot(slotIndex);
            
            // If slot has an item, try to assign to hotbar
            if (!slot.IsEmpty)
            {
                // Check if item is already in hotbar
                int existingHotbarSlot = FindItemInHotbar(slot.item);
                
                if (existingHotbarSlot >= 0)
                {
                    // Already in hotbar, remove it
                    inventoryManager.RemoveFromHotbar(existingHotbarSlot);
                }
                else
                {
                    // Find first empty hotbar slot
                    int emptySlot = FindEmptyHotbarSlot();
                    if (emptySlot >= 0)
                    {
                        inventoryManager.AssignToHotbar(slotIndex, emptySlot);
                    }
                }
            }
        }
        
        /// <summary>
        /// Find if an item is already in a hotbar slot
        /// </summary>
        private int FindItemInHotbar(Item item)
        {
            if (inventoryManager == null || item == null)
                return -1;
            
            for (int i = 0; i < inventoryManager.HotbarSize; i++)
            {
                HotbarSlot hotbarSlot = inventoryManager.GetHotbarSlot(i);
                if (hotbarSlot != null && hotbarSlot.HasItem && hotbarSlot.item == item)
                {
                    return i;
                }
            }
            
            return -1;
        }
        
        /// <summary>
        /// Find first empty hotbar slot
        /// </summary>
        private int FindEmptyHotbarSlot()
        {
            if (inventoryManager == null)
                return -1;
            
            for (int i = 0; i < inventoryManager.HotbarSize; i++)
            {
                HotbarSlot hotbarSlot = inventoryManager.GetHotbarSlot(i);
                if (hotbarSlot == null || !hotbarSlot.HasItem)
                {
                    return i;
                }
            }
            
            return -1;
        }
        
        /// <summary>
        /// Swap two slots
        /// </summary>
        public void SwapSlots(int slotIndex1, int slotIndex2)
        {
            if (inventoryManager != null)
            {
                inventoryManager.SwapSlots(slotIndex1, slotIndex2);
            }
        }
        
        /// <summary>
        /// Toggle inventory open/closed
        /// </summary>
        public void ToggleInventory()
        {
            if (isOpen)
                Close();
            else
                Open();
        }
        
        /// <summary>
        /// Open inventory
        /// </summary>
        public void Open()
        {
            if (inventoryPanel != null)
                inventoryPanel.SetActive(true);
            
            // Ensure all slot GameObjects are active when opening
            foreach (var slotUI in slotUIList)
            {
                if (slotUI != null)
                    slotUI.gameObject.SetActive(true);
            }
            
            isOpen = true;
            RefreshInventory(-1);
        }
        
        /// <summary>
        /// Close inventory
        /// </summary>
        public void Close()
        {
            if (inventoryPanel != null)
                inventoryPanel.SetActive(false);
            
            isOpen = false;
            ItemTooltip.Instance?.Hide();
        }
    }
}
