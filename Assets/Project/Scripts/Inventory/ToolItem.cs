using UnityEngine;

namespace FarmingRPG.Inventory
{
    /// <summary>
    /// Tools like axes, hoes, watering cans, weapons
    /// </summary>
    [CreateAssetMenu(fileName = "New Tool", menuName = "Farming RPG/Items/Tool")]
    public class ToolItem : Item
    {
        [Header("Tool Properties")]
        public ToolType toolType;
        public int power = 1; // Tool effectiveness
        public int durability = 100;
        public int maxDurability = 100;
        
        [Header("Attack (for weapons)")]
        public int damage = 0;
        public float attackRange = 1f;
        public float attackSpeed = 1f;
        
        public override bool Use(GameObject user)
        {
            // Handle tool usage
            Debug.Log($"Using {itemName} - Durability: {durability}/{maxDurability}");
            
            // Reduce durability
            durability--;
            
            if (durability <= 0)
            {
                Debug.Log($"{itemName} broke!");
                return true; // Tool broke, remove from inventory
            }
            
            return false;
        }
        
        public void Repair(int amount)
        {
            durability = Mathf.Min(durability + amount, maxDurability);
        }
    }
    
    public enum ToolType
    {
        Axe,
        Pickaxe,
        Hoe,
        WateringCan,
        Sword,
        Scythe,
        FishingRod,
        Hammer
    }
}
