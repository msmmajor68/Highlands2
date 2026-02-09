using UnityEngine;
using FarmingRPG.Inventory;

namespace FarmingRPG.Core
{
    /// <summary>
    /// Main game manager that initializes core systems
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("Database References")]
        [SerializeField] private ItemDatabase itemDatabase;
        
        [Header("Settings")]
        [SerializeField] private bool debugMode = true;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Initialize game systems
        /// </summary>
        private void Initialize()
        {
            // Initialize item database
            if (itemDatabase != null)
            {
                itemDatabase.Initialize();
            }
            else
            {
                Debug.LogError("ItemDatabase not assigned in GameManager!");
            }
            
            // Set target frame rate
            Application.targetFrameRate = 60;
            
            if (debugMode)
            {
                Debug.Log("Game Manager initialized");
            }
        }
        
        /// <summary>
        /// Get the item database
        /// </summary>
        public ItemDatabase GetItemDatabase()
        {
            return itemDatabase;
        }
        
        private void Update()
        {
            // Debug commands
            if (debugMode)
            {
                HandleDebugCommands();
            }
        }
        
        /// <summary>
        /// Handle debug input commands
        /// </summary>
        private void HandleDebugCommands()
        {
            // Example: Press F1 to add test items to inventory
            if (Input.GetKeyDown(KeyCode.F1))
            {
                AddTestItems();
            }
            
            // F2 to clear inventory
            if (Input.GetKeyDown(KeyCode.F2))
            {
                ClearPlayerInventory();
            }
        }
        
        /// <summary>
        /// Add test items to player inventory (for testing)
        /// </summary>
        private void AddTestItems()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                InventoryManager inventory = player.GetComponent<InventoryManager>();
                if (inventory != null && itemDatabase != null)
                {
                    var allItems = itemDatabase.GetAllItems();
                    if (allItems.Count > 0)
                    {
                        // Add a few random items
                        for (int i = 0; i < Mathf.Min(5, allItems.Count); i++)
                        {
                            Item item = allItems[Random.Range(0, allItems.Count)];
                            inventory.AddItem(item, Random.Range(1, 10));
                        }
                        Debug.Log("Added test items to inventory");
                    }
                }
            }
        }
        
        /// <summary>
        /// Clear player inventory
        /// </summary>
        private void ClearPlayerInventory()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                InventoryManager inventory = player.GetComponent<InventoryManager>();
                if (inventory != null)
                {
                    inventory.ClearInventory();
                    Debug.Log("Cleared player inventory");
                }
            }
        }
    }
}
