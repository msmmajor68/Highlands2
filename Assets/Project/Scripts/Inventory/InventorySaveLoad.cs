using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace FarmingRPG.Inventory
{
    /// <summary>
    /// Save and load inventory data
    /// </summary>
    public class InventorySaveLoad : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string saveFileName = "inventory_save.json";
        [SerializeField] private bool autoSave = true;
        [SerializeField] private float autoSaveInterval = 60f; // Seconds
        
        private InventoryManager inventoryManager;
        private string savePath;
        private float autoSaveTimer = 0f;
        
        private void Awake()
        {
            inventoryManager = GetComponent<InventoryManager>();
            savePath = Path.Combine(Application.persistentDataPath, saveFileName);
        }
        
        private void Start()
        {
            LoadInventory();
        }
        
        private void Update()
        {
            if (autoSave)
            {
                autoSaveTimer += Time.deltaTime;
                
                if (autoSaveTimer >= autoSaveInterval)
                {
                    SaveInventory();
                    autoSaveTimer = 0f;
                }
            }
        }
        
        private void OnApplicationQuit()
        {
            SaveInventory();
        }
        
        /// <summary>
        /// Save inventory to file
        /// </summary>
        public void SaveInventory()
        {
            if (inventoryManager == null)
                return;
            
            try
            {
                InventorySaveData saveData = new InventorySaveData();
                InventorySlot[] slots = inventoryManager.GetAllSlots();
                
                saveData.slots = new List<SlotSaveData>();
                
                for (int i = 0; i < slots.Length; i++)
                {
                    if (!slots[i].IsEmpty)
                    {
                        SlotSaveData slotData = new SlotSaveData
                        {
                            slotIndex = i,
                            itemName = slots[i].item.itemName,
                            quantity = slots[i].quantity
                        };
                        
                        saveData.slots.Add(slotData);
                    }
                }
                
                string json = JsonUtility.ToJson(saveData, true);
                File.WriteAllText(savePath, json);
                
                Debug.Log($"Inventory saved to: {savePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save inventory: {e.Message}");
            }
        }
        
        /// <summary>
        /// Load inventory from file
        /// </summary>
        public void LoadInventory()
        {
            if (inventoryManager == null)
                return;
            
            if (!File.Exists(savePath))
            {
                Debug.Log("No save file found. Starting with empty inventory.");
                return;
            }
            
            try
            {
                string json = File.ReadAllText(savePath);
                InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);
                
                // Clear current inventory
                inventoryManager.ClearInventory();
                
                // Load items
                ItemDatabase database = FindObjectOfType<ItemDatabase>();
                if (database == null)
                {
                    Debug.LogError("ItemDatabase not found! Cannot load inventory.");
                    return;
                }
                
                database.Initialize();
                
                foreach (SlotSaveData slotData in saveData.slots)
                {
                    Item item = database.GetItemByName(slotData.itemName);
                    if (item != null)
                    {
                        inventoryManager.AddItem(item, slotData.quantity);
                    }
                }
                
                Debug.Log($"Inventory loaded from: {savePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load inventory: {e.Message}");
            }
        }
        
        /// <summary>
        /// Delete save file
        /// </summary>
        public void DeleteSave()
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("Save file deleted");
            }
        }
    }
    
    [System.Serializable]
    public class InventorySaveData
    {
        public List<SlotSaveData> slots;
    }
    
    [System.Serializable]
    public class SlotSaveData
    {
        public int slotIndex;
        public string itemName;
        public int quantity;
    }
}
