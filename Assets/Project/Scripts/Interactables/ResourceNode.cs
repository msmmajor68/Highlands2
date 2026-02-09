using UnityEngine;
using FarmingRPG.Player;
using FarmingRPG.Inventory;

namespace FarmingRPG.Interactables
{
    /// <summary>
    /// Resource node that can be harvested (trees, rocks, plants, etc.)
    /// </summary>
    public class ResourceNode : MonoBehaviour, IInteractable
    {
        [Header("Resource Info")]
        [SerializeField] private Item resourceItem;
        [SerializeField] private int minYield = 1;
        [SerializeField] private int maxYield = 3;
        
        [Header("Requirements")]
        [SerializeField] private ToolType requiredTool = ToolType.Axe;
        [SerializeField] private int minToolPower = 1;
        
        [Header("Node Settings")]
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private float respawnTime = 60f; // Seconds, 0 = no respawn
        
        [Header("Visuals")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject harvestEffect;
        [SerializeField] private GameObject depletedVisual;
        
        private int currentHealth;
        private bool isDepleted = false;
        private float respawnTimer = 0f;
        
        private void Start()
        {
            currentHealth = maxHealth;
        }
        
        private void Update()
        {
            if (isDepleted && respawnTime > 0)
            {
                respawnTimer += Time.deltaTime;
                
                if (respawnTimer >= respawnTime)
                {
                    Respawn();
                }
            }
        }
        
        /// <summary>
        /// Interact with the resource node
        /// </summary>
        public void Interact(GameObject player)
        {
            if (isDepleted)
            {
                Debug.Log($"This {resourceItem?.itemName ?? "resource"} is depleted");
                return;
            }
            
            // Check if player has the right tool equipped
            InventoryManager inventory = player.GetComponent<InventoryManager>();
            if (inventory == null)
                return;
            
            InventorySlot selectedSlot = inventory.GetSelectedHotbarSlot();
            
            if (selectedSlot == null || selectedSlot.IsEmpty)
            {
                Debug.Log($"You need a {requiredTool} to harvest this");
                return;
            }
            
            // Check if it's the right tool
            if (selectedSlot.item is ToolItem tool)
            {
                if (tool.toolType != requiredTool)
                {
                    Debug.Log($"You need a {requiredTool} to harvest this");
                    return;
                }
                
                if (tool.power < minToolPower)
                {
                    Debug.Log($"Your {tool.itemName} is not strong enough");
                    return;
                }
                
                // Harvest the resource
                Harvest(player, tool);
            }
            else
            {
                Debug.Log($"You need a {requiredTool} to harvest this");
            }
        }
        
        /// <summary>
        /// Harvest the resource
        /// </summary>
        private void Harvest(GameObject player, ToolItem tool)
        {
            currentHealth--;
            
            // Use the tool (reduces durability)
            tool.Use(player);
            
            // Spawn harvest effect
            if (harvestEffect != null)
            {
                Instantiate(harvestEffect, transform.position, Quaternion.identity);
            }
            
            // Check if depleted
            if (currentHealth <= 0)
            {
                GiveReward(player);
                Deplete();
            }
        }
        
        /// <summary>
        /// Give reward to player
        /// </summary>
        private void GiveReward(GameObject player)
        {
            if (resourceItem == null)
                return;
            
            int yield = Random.Range(minYield, maxYield + 1);
            
            InventoryManager inventory = player.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                inventory.AddItem(resourceItem, yield);
                Debug.Log($"Harvested {yield}x {resourceItem.itemName}");
            }
        }
        
        /// <summary>
        /// Mark node as depleted
        /// </summary>
        private void Deplete()
        {
            isDepleted = true;
            respawnTimer = 0f;
            
            // Hide or change sprite
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
            
            if (depletedVisual != null)
            {
                depletedVisual.SetActive(true);
            }
            
            Debug.Log($"Resource node depleted. Respawn in {respawnTime}s");
        }
        
        /// <summary>
        /// Respawn the resource
        /// </summary>
        private void Respawn()
        {
            isDepleted = false;
            currentHealth = maxHealth;
            respawnTimer = 0f;
            
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
            
            if (depletedVisual != null)
            {
                depletedVisual.SetActive(false);
            }
            
            Debug.Log("Resource node respawned");
        }
    }
}
