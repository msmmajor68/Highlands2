using UnityEngine;
using UnityEngine.UI;
using FarmingRPG.Player;

namespace FarmingRPG.UI
{
    /// <summary>
    /// Permanent in-game HUD displaying player stats in real-time
    /// Shows Health, Stamina, and Hunger bars and values
    /// </summary>
    public class StatHUDUI : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        
        [Header("Health UI")]
        [SerializeField] private Image healthBar;
        [SerializeField] private Text healthText;
        [SerializeField] private Image healthFillColor = Color.red;
        
        [Header("Stamina UI")]
        [SerializeField] private Image staminaBar;
        [SerializeField] private Text staminaText;
        [SerializeField] private Image staminaFillColor = Color.yellow;
        
        [Header("Hunger UI")]
        [SerializeField] private Image hungerBar;
        [SerializeField] private Text hungerText;
        [SerializeField] private Image hungerFillColor = Color.green;
        
        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindObjectOfType<PlayerStats>();
            }
            
            if (playerStats == null)
            {
                Debug.LogError("StatHUDUI: PlayerStats not found!");
                return;
            }
            
            // Subscribe to stat changes
            playerStats.OnHealthChanged += UpdateHealthDisplay;
            playerStats.OnStaminaChanged += UpdateStaminaDisplay;
            playerStats.OnHungerChanged += UpdateHungerDisplay;
            
            // Initial display
            UpdateHealthDisplay(playerStats.CurrentHealth, playerStats.MaxHealth);
            UpdateStaminaDisplay(playerStats.CurrentStamina, playerStats.MaxStamina);
            UpdateHungerDisplay(playerStats.CurrentHunger, playerStats.MaxHunger);
        }
        
        private void OnDestroy()
        {
            if (playerStats != null)
            {
                playerStats.OnHealthChanged -= UpdateHealthDisplay;
                playerStats.OnStaminaChanged -= UpdateStaminaDisplay;
                playerStats.OnHungerChanged -= UpdateHungerDisplay;
            }
        }
        
        /// <summary>
        /// Update health bar and text display
        /// </summary>
        private void UpdateHealthDisplay(int current, int max)
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = (float)current / max;
            }
            
            if (healthText != null)
            {
                healthText.text = $"Health: {current}/{max}";
            }
        }
        
        /// <summary>
        /// Update stamina bar and text display
        /// </summary>
        private void UpdateStaminaDisplay(int current, int max)
        {
            if (staminaBar != null)
            {
                staminaBar.fillAmount = (float)current / max;
            }
            
            if (staminaText != null)
            {
                staminaText.text = $"Stamina: {current}/{max}";
            }
        }
        
        /// <summary>
        /// Update hunger bar and text display
        /// </summary>
        private void UpdateHungerDisplay(int current, int max)
        {
            if (hungerBar != null)
            {
                hungerBar.fillAmount = (float)current / max;
            }
            
            if (hungerText != null)
            {
                hungerText.text = $"Hunger: {current}/{max}";
            }
        }
    }
}
