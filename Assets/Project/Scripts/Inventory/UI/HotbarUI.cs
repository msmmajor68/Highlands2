using UnityEngine;
using System.Collections.Generic;

namespace FarmingRPG.Inventory.UI
{
    /// <summary>
    /// Hotbar UI display at bottom of screen
    /// </summary>
    public class HotbarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private Transform hotbarContainer;
        [SerializeField] private GameObject hotbarSlotPrefab;
        
        private List<ItemSlotUI> hotbarSlots = new List<ItemSlotUI>();
        private int currentSelectedIndex = 0;
        private InventoryUI inventoryUI;
        
        private void Start()
        {
            if (inventoryManager == null)
                inventoryManager = FindObjectOfType<InventoryManager>();
            
            inventoryUI = FindObjectOfType<InventoryUI>();
            
            InitializeHotbar();
            
            // Subscribe to events
            if (inventoryManager != null)
            {
                inventoryManager.OnHotbarSlotChanged += UpdateHotbarSlotUI;
                inventoryManager.OnHotbarChanged += OnHotbarSelectionChanged;
            }
            
            UpdateSelectedSlot(0);
        }
        
        private void OnDestroy()
        {
            if (inventoryManager != null)
            {
                inventoryManager.OnHotbarSlotChanged -= UpdateHotbarSlotUI;
                inventoryManager.OnHotbarChanged -= OnHotbarSelectionChanged;
            }
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        /// <summary>
        /// Initialize hotbar slots
        /// </summary>
        private void InitializeHotbar()
        {
            if (inventoryManager == null || hotbarSlotPrefab == null || hotbarContainer == null)
                return;
            
            // Clear existing
            foreach (Transform child in hotbarContainer)
            {
                Destroy(child.gameObject);
            }
            hotbarSlots.Clear();
            
            // Create hotbar slots (first N slots of inventory)
            int hotbarSize = inventoryManager.HotbarSize;
            for (int i = 0; i < hotbarSize; i++)
            {
                GameObject slotObj = Instantiate(hotbarSlotPrefab, hotbarContainer);
                ItemSlotUI slotUI = slotObj.GetComponent<ItemSlotUI>();
                
                if (slotUI != null)
                {
                    slotUI.Initialize(i, inventoryUI);
                    slotObj.SetActive(true);
                    hotbarSlots.Add(slotUI);
                    
                    // Update with hotbar data
                    HotbarSlot hotbarSlot = inventoryManager.GetHotbarSlot(i);
                    UpdateHotbarSlotDisplay(i, hotbarSlot);
                }
            }
        }
        
        /// <summary>
        /// Handle hotbar input
        /// </summary>
        private void HandleInput()
        {
            // Number keys 1-9 for slot selection (slots 0-8)
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    SelectSlot(i);
                }
            }
            
            // Alpha0 for slot 10 (index 9)
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SelectSlot(9);
            }
            
            // Scroll wheel to cycle through hotbar
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                int direction = scroll > 0 ? -1 : 1;
                CycleHotbarSelection(direction);
            }
            
            // Use currently selected item with left mouse button (when not in inventory)
            // Only if inventory is not open to avoid conflicts
            bool inventoryOpen = FindObjectOfType<InventoryUI>()?.IsOpen ?? false;
            if (!inventoryOpen && Input.GetMouseButtonDown(0))
            {
                UseSelectedItem();
            }
        }
        
        /// <summary>
        /// Select a hotbar slot
        /// </summary>
        private void SelectSlot(int index)
        {
            if (index >= 0 && index < hotbarSlots.Count && inventoryManager != null)
            {
                inventoryManager.SelectHotbarSlot(index);
            }
        }
        
        /// <summary>
        /// Cycle hotbar selection
        /// </summary>
        private void CycleHotbarSelection(int direction)
        {
            if (inventoryManager == null)
                return;
            
            int newIndex = currentSelectedIndex + direction;
            
            if (newIndex < 0)
                newIndex = inventoryManager.HotbarSize - 1;
            else if (newIndex >= inventoryManager.HotbarSize)
                newIndex = 0;
            
            SelectSlot(newIndex);
        }
        
        /// <summary>
        /// Use the currently selected hotbar item
        /// </summary>
        private void UseSelectedItem()
        {
            if (inventoryManager != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                inventoryManager.UseHotbarItem(currentSelectedIndex, player);
            }
        }
        
        /// <summary>
        /// Update hotbar slot display from HotbarSlot data
        /// </summary>
        private void UpdateHotbarSlotDisplay(int hotbarIndex, HotbarSlot hotbarSlot)
        {
            if (hotbarIndex < 0 || hotbarIndex >= hotbarSlots.Count)
                return;
            
            if (hotbarSlot != null && hotbarSlot.HasItem && inventoryManager != null)
            {
                // Get the actual inventory slot this hotbar slot references
                InventorySlot invSlot = inventoryManager.GetSlot(hotbarSlot.inventorySlotIndex);
                hotbarSlots[hotbarIndex].UpdateSlot(invSlot);
            }
            else
            {
                // Clear the display
                hotbarSlots[hotbarIndex].UpdateSlot(null);
            }
        }
        
        /// <summary>
        /// Update a specific hotbar slot UI
        /// </summary>
        private void UpdateHotbarSlotUI(int index, HotbarSlot hotbarSlot)
        {
            UpdateHotbarSlotDisplay(index, hotbarSlot);
        }
        
        /// <summary>
        /// Called when hotbar selection changes
        /// </summary>
        private void OnHotbarSelectionChanged(int newIndex)
        {
            UpdateSelectedSlot(newIndex);
        }
        
        /// <summary>
        /// Update visual selection of hotbar slots
        /// </summary>
        private void UpdateSelectedSlot(int newIndex)
        {
            // Deselect old slot
            if (currentSelectedIndex >= 0 && currentSelectedIndex < hotbarSlots.Count)
            {
                hotbarSlots[currentSelectedIndex].SetSelected(false);
            }
            
            // Select new slot
            currentSelectedIndex = newIndex;
            if (currentSelectedIndex >= 0 && currentSelectedIndex < hotbarSlots.Count)
            {
                hotbarSlots[currentSelectedIndex].SetSelected(true);
            }
        }
    }
}
