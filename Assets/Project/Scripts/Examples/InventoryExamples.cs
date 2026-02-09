using UnityEngine;
using FarmingRPG.Inventory;
using FarmingRPG.Player;

namespace FarmingRPG.Examples
{
    /// <summary>
    /// Example script showing how to use the inventory system
    /// This is for reference - attach to a test object or use in your own scripts
    /// </summary>
    public class InventoryExamples : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject player;
        [SerializeField] private ItemDatabase itemDatabase;
        
        [Header("Example Items")]
        [SerializeField] private Item exampleItem;
        
        private InventoryManager inventory;
        
        private void Start()
        {
            // Get references
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");
            
            if (player != null)
                inventory = player.GetComponent<InventoryManager>();
        }
        
        // ============ EXAMPLE METHODS ============
        
        /// <summary>
        /// Example: Add an item to inventory
        /// </summary>
        public void ExampleAddItem()
        {
            if (inventory == null || exampleItem == null)
                return;
            
            bool success = inventory.AddItem(exampleItem, 5);
            if (success)
                Debug.Log($"Added 5x {exampleItem.itemName} to inventory");
            else
                Debug.Log("Inventory full!");
        }
        
        /// <summary>
        /// Example: Remove an item from inventory
        /// </summary>
        public void ExampleRemoveItem()
        {
            if (inventory == null || exampleItem == null)
                return;
            
            bool success = inventory.RemoveItem(exampleItem, 2);
            if (success)
                Debug.Log($"Removed 2x {exampleItem.itemName} from inventory");
            else
                Debug.Log("Not enough items in inventory!");
        }
        
        /// <summary>
        /// Example: Check if player has an item
        /// </summary>
        public void ExampleCheckItem()
        {
            if (inventory == null || exampleItem == null)
                return;
            
            int count = inventory.GetItemCount(exampleItem);
            Debug.Log($"Player has {count}x {exampleItem.itemName}");
            
            bool hasEnough = inventory.HasItem(exampleItem, 10);
            Debug.Log($"Has at least 10? {hasEnough}");
        }
        
        /// <summary>
        /// Example: Get item from database by name
        /// </summary>
        public void ExampleGetItemFromDatabase()
        {
            if (itemDatabase == null)
                return;
            
            Item apple = itemDatabase.GetItemByName("Apple");
            if (apple != null)
            {
                Debug.Log($"Found item: {apple.itemName}");
                inventory?.AddItem(apple, 1);
            }
        }
        
        /// <summary>
        /// Example: Give multiple items at once (quest reward, etc.)
        /// </summary>
        public void ExampleGiveQuestReward()
        {
            if (inventory == null || itemDatabase == null)
                return;
            
            // Quest rewards
            Item gold = itemDatabase.GetItemByName("Gold Coin");
            Item potion = itemDatabase.GetItemByName("Health Potion");
            Item sword = itemDatabase.GetItemByName("Iron Sword");
            
            if (gold != null) inventory.AddItem(gold, 100);
            if (potion != null) inventory.AddItem(potion, 3);
            if (sword != null) inventory.AddItem(sword, 1);
            
            Debug.Log("Quest reward items added!");
        }
        
        /// <summary>
        /// Example: Check if player can afford a purchase
        /// </summary>
        public void ExampleCheckCanAfford()
        {
            if (inventory == null || itemDatabase == null)
                return;
            
            Item gold = itemDatabase.GetItemByName("Gold Coin");
            
            int requiredGold = 50;
            bool canAfford = inventory.HasItem(gold, requiredGold);
            
            if (canAfford)
            {
                inventory.RemoveItem(gold, requiredGold);
                Debug.Log("Purchase successful!");
                // Give purchased item here
            }
            else
            {
                Debug.Log("Not enough gold!");
            }
        }
        
        /// <summary>
        /// Example: Use consumable item
        /// </summary>
        public void ExampleUseConsumable()
        {
            if (inventory == null)
                return;
            
            // Use item in hotbar slot 0
            int slotIndex = 0;
            bool used = inventory.UseItem(slotIndex, player);
            
            if (used)
                Debug.Log("Item used successfully");
        }
        
        /// <summary>
        /// Example: Swap two inventory slots
        /// </summary>
        public void ExampleSwapSlots()
        {
            if (inventory == null)
                return;
            
            // Swap slot 0 and slot 5
            inventory.SwapSlots(0, 5);
            Debug.Log("Slots swapped");
        }
        
        /// <summary>
        /// Example: Get all items of a specific type
        /// </summary>
        public void ExampleGetItemsByType()
        {
            if (itemDatabase == null)
                return;
            
            var allSeeds = itemDatabase.GetItemsByType(ItemType.Seed);
            Debug.Log($"Found {allSeeds.Count} seeds in database");
            
            foreach (Item seed in allSeeds)
            {
                Debug.Log($"- {seed.itemName}");
            }
        }
        
        /// <summary>
        /// Example: Modify player stats
        /// </summary>
        public void ExampleModifyPlayerStats()
        {
            PlayerStats stats = player?.GetComponent<PlayerStats>();
            if (stats == null)
                return;
            
            // Heal player
            stats.RestoreHealth(20);
            
            // Use stamina
            bool hasStamina = stats.UseStamina(10);
            if (hasStamina)
                Debug.Log("Performed action using stamina");
            
            // Feed player
            stats.RestoreHunger(30);
            
            // Apply buff (speed x1.5 for 10 seconds)
            stats.ApplyBuff(1.5f, 1.0f, 10f);
        }
        
        /// <summary>
        /// Example: Check inventory space
        /// </summary>
        public void ExampleCheckInventorySpace()
        {
            if (inventory == null)
                return;
            
            int emptySlots = 0;
            InventorySlot[] slots = inventory.GetAllSlots();
            
            foreach (InventorySlot slot in slots)
            {
                if (slot.IsEmpty)
                    emptySlots++;
            }
            
            Debug.Log($"Empty slots: {emptySlots}/{inventory.InventorySize}");
        }
        
        /// <summary>
        /// Example: Create a custom item at runtime
        /// </summary>
        public void ExampleCreateRuntimeItem()
        {
            // Note: Usually items should be created as ScriptableObjects in the editor
            // But you can create instances at runtime for special cases
            
            ConsumableItem newPotion = ScriptableObject.CreateInstance<ConsumableItem>();
            newPotion.itemName = "Super Potion";
            newPotion.description = "Restores 100 health";
            newPotion.itemType = ItemType.Consumable;
            newPotion.healthRestore = 100;
            newPotion.maxStackSize = 10;
            
            if (inventory != null)
            {
                inventory.AddItem(newPotion, 1);
                Debug.Log("Created and added runtime item");
            }
        }
    }
}
