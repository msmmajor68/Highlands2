using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FarmingRPG.Inventory;
using FarmingRPG.Interactables;
using System.Collections.Generic;

namespace FarmingRPG.Inventory.UI
{
    /// <summary>
    /// UI for displaying storage container contents
    /// </summary>
    public class StorageUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject storagePanel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Transform storageSlotsContainer;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Button closeButton;
        
        private StorageContainer currentContainer;
        private List<ItemSlotUI> storageSlotUIList = new List<ItemSlotUI>();
        private InventoryManager playerInventory;
        
        private void Start()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseStorage);
            }
            
            // Find player inventory
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerInventory = player.GetComponent<InventoryManager>();
            }
            
            // Start hidden
            if (storagePanel != null)
                storagePanel.SetActive(false);
        }
        
        /// <summary>
        /// Open storage UI for a container
        /// </summary>
        public void OpenStorage(StorageContainer container)
        {
            if (container == null)
                return;
            
            currentContainer = container;
            
            // Set title
            if (titleText != null)
            {
                titleText.text = container.ContainerName;
            }
            
            // Initialize slots
            InitializeSlots();
            
            // Show panel
            if (storagePanel != null)
                storagePanel.SetActive(true);
            
            container.Open();
        }
        
        /// <summary>
        /// Close storage UI
        /// </summary>
        public void CloseStorage()
        {
            if (currentContainer != null)
            {
                currentContainer.Close();
                currentContainer = null;
            }
            
            if (storagePanel != null)
                storagePanel.SetActive(false);
            
            // Clean up slots
            foreach (Transform child in storageSlotsContainer)
            {
                Destroy(child.gameObject);
            }
            storageSlotUIList.Clear();
        }
        
        /// <summary>
        /// Initialize storage slot UIs
        /// </summary>
        private void InitializeSlots()
        {
            if (currentContainer == null || slotPrefab == null || storageSlotsContainer == null)
                return;
            
            // Clear existing
            foreach (Transform child in storageSlotsContainer)
            {
                Destroy(child.gameObject);
            }
            storageSlotUIList.Clear();
            
            // Create slots
            int storageSize = currentContainer.StorageSize;
            for (int i = 0; i < storageSize; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, storageSlotsContainer);
                ItemSlotUI slotUI = slotObj.GetComponent<ItemSlotUI>();
                
                if (slotUI != null)
                {
                    slotUI.Initialize(i, null);
                    storageSlotUIList.Add(slotUI);
                    
                    // Update with current data
                    InventorySlot slot = currentContainer.GetSlot(i);
                    slotUI.UpdateSlot(slot);
                    
                    // Add click handler for transfer
                    int index = i; // Capture for lambda
                    Button slotButton = slotObj.GetComponent<Button>();
                    if (slotButton != null)
                    {
                        slotButton.onClick.AddListener(() => OnStorageSlotClicked(index));
                    }
                }
            }
        }
        
        /// <summary>
        /// Handle storage slot click (transfer to player)
        /// </summary>
        private void OnStorageSlotClicked(int slotIndex)
        {
            if (currentContainer == null || playerInventory == null)
                return;
            
            InventorySlot slot = currentContainer.GetSlot(slotIndex);
            if (slot == null || slot.IsEmpty)
                return;
            
            // Try to transfer to player
            bool success = currentContainer.TransferToPlayer(playerInventory, slot.item, 1);
            
            if (success)
            {
                // Update display
                storageSlotUIList[slotIndex].UpdateSlot(slot);
            }
        }
        
        private void Update()
        {
            // Close with Escape key
            if (storagePanel != null && storagePanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CloseStorage();
                }
            }
        }
    }
}
