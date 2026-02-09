using UnityEngine;

namespace FarmingRPG.Inventory
{
    /// <summary>
    /// Seeds that can be planted
    /// </summary>
    [CreateAssetMenu(fileName = "New Seed", menuName = "Farming RPG/Items/Seed")]
    public class SeedItem : Item
    {
        [Header("Seed Properties")]
        public GameObject cropPrefab; // Plant that grows
        public int growthTime = 3; // Days to grow
        public Item harvestedCrop; // What crop produces when harvested
        public int minYield = 1;
        public int maxYield = 3;
        
        [Header("Growing Seasons")]
        public bool springGrowth = true;
        public bool summerGrowth = false;
        public bool fallGrowth = false;
        public bool winterGrowth = false;
        
        public override bool Use(GameObject user)
        {
            // Plant the seed (handled by farming system)
            Debug.Log($"Planted {itemName}");
            return true; // Consume the seed
        }
        
        public bool CanGrowInSeason(Season season)
        {
            return season switch
            {
                Season.Spring => springGrowth,
                Season.Summer => summerGrowth,
                Season.Fall => fallGrowth,
                Season.Winter => winterGrowth,
                _ => false
            };
        }
    }
    
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
}
