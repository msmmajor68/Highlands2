using UnityEngine;
using FarmingRPG.Player;

namespace FarmingRPG.Inventory
{
    /// <summary>
    /// Items that can be consumed (food, potions, etc.)
    /// </summary>
    [CreateAssetMenu(fileName = "New Consumable", menuName = "Farming RPG/Items/Consumable")]
    public class ConsumableItem : Item
    {
        [Header("Consumable Properties")]
        public int healthRestore = 0;
        public int staminaRestore = 0;
        public int hungerRestore = 0;
        public float duration = 0f; // For buff effects
        
        [Header("Buff Effects")]
        public float speedMultiplier = 1f;
        public float strengthMultiplier = 1f;
        
        public override bool Use(GameObject user)
        {
            // Apply effects to player
            PlayerStats stats = user.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.RestoreHealth(healthRestore);
                stats.RestoreStamina(staminaRestore);
                stats.RestoreHunger(hungerRestore);
                
                if (duration > 0)
                {
                    stats.ApplyBuff(speedMultiplier, strengthMultiplier, duration);
                }
                
                Debug.Log($"Consumed {itemName}");
                return true; // Consume the item
            }
            
            return false;
        }
    }
}
